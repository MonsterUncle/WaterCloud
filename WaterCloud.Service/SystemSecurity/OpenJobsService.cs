/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemSecurity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Quartz;
using WaterCloud.Service.AutoJob;
using Quartz.Spi;
using WaterCloud.DataBase;
using SqlSugar;
using System.IO;
using System.Reflection;
using System.Net.Http;
using Quartz.Impl.Triggers;

namespace WaterCloud.Service.SystemSecurity
{
    public class OpenJobsService : IDenpendency
    {
        private RepositoryBase<OpenJobEntity> repository;
        private IScheduler _scheduler;
        private string HandleLogProvider = GlobalContext.SystemConfig.HandleLogProvider;
        private HttpWebClient _httpClient;

        public OpenJobsService(IUnitOfWork unitOfWork, ISchedulerFactory schedulerFactory, IJobFactory iocJobfactory, IHttpClientFactory httpClient)
        {
            var uniwork = unitOfWork;
            repository = new RepositoryBase<OpenJobEntity>(uniwork);
            _scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            _scheduler.JobFactory = iocJobfactory;
            _httpClient = new HttpWebClient(httpClient);
        }
        /// <summary>
        /// 加载列表
        /// </summary>
        public async Task<List<OpenJobEntity>> GetLookList(Pagination pagination, string keyword = "")
        {
            var DbNumber = OperatorProvider.Provider.GetCurrent().DbNumber;
            var query = repository.IQueryable().Where(a => a.DbNumber == DbNumber);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.JobName.Contains(keyword) || a.Description.Contains(keyword));
            }
            query = query.Where(a => a.DeleteMark == false);
            return await query.ToPageListAsync(pagination);
        }

        public async Task<List<OpenJobLogEntity>> GetLogList(string keyValue)
        {
            return await Task.Run(() => {
                if (HandleLogProvider != Define.CACHEPROVIDER_REDIS)
                {
                    return repository.Db.Queryable<OpenJobLogEntity>().Where(a => a.JobId == keyValue).OrderBy(a => a.CreatorTime, OrderByType.Desc).ToList();
                }
                else
                {
                    return HandleLogHelper.HGetAll<OpenJobLogEntity>(keyValue).Values.OrderByDescending(a => a.CreatorTime).ToList(); ;
                }
            });
        }
        public async Task<List<OpenJobEntity>> GetList(string keyword = "")
        {
            var DbNumber = OperatorProvider.Provider.GetCurrent().DbNumber;
            var query = repository.IQueryable().Where(a => a.DbNumber == DbNumber);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.JobName.Contains(keyword));
            }
            return await query.Where(a => a.DeleteMark == false).ToListAsync();
        }
        public async Task<List<OpenJobEntity>> GetAllList(string keyword = "")
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.JobName.Contains(keyword));
            }
            return await query.Where(a => a.DeleteMark == false).ToListAsync();
        }
        public async Task<OpenJobEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        public async Task SubmitForm(OpenJobEntity entity, string keyValue)
        {
            bool IsTrue = CronExpression.IsValidExpression(entity.CronExpress);
            if (IsTrue == false)
            {
                throw new Exception("定时任务的Cron表达式不正确！");
            }
            repository.unitOfWork.CurrentBeginTrans();
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                await repository.Update(entity);
            }
            else
            {
                entity.Create();
                entity.DbNumber = OperatorProvider.Provider.GetCurrent().DbNumber;
                await repository.Insert(entity);
            }
            //启动任务
            if (entity.DoItNow == true)
            {
                await ChangeJobStatus(entity.Id, 1);
            }
            repository.unitOfWork.CurrentCommit();
            //执行任务
            if (entity.DoItNow == true)
            {
                await DoNow(entity.Id, false);
            }
        }
        /// <summary>
        /// 清除任务计划
        /// </summary>
        /// <returns></returns>
        public async Task ClearScheduleJob()
        {
            try
            {
                await _scheduler.Clear();
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
        }
        public async Task DeleteForm(string keyValue)
        {
            var job = await repository.FindEntity(keyValue);
            TriggerKey triggerKey = new TriggerKey(job.JobName, job.JobGroup);
            await _scheduler.PauseTrigger(triggerKey);
            await _scheduler.UnscheduleJob(triggerKey);
            await _scheduler.DeleteJob(new JobKey(job.JobName, job.JobGroup));
            await repository.Delete(a => a.Id == keyValue);
        }
        #region 定时任务运行相关操作

        /// <summary>
        /// 返回系统的job接口
        /// </summary>
        /// <returns></returns>
        public List<KeyValue> QueryLocalHandlers()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(a => a.GetInterfaces()
                    .Contains(typeof(IJobTask))))
                .ToArray();
            var list = new List<KeyValue>();
			foreach (var item in types)
			{
                list.Add(new KeyValue { 
                    Key=item.FullName,
                    Description = item.GetCustomAttribute<ServiceDescriptionAttribute>(false)!.ClassDescription
                });
			}
            return list;
        }

        public async Task ChangeJobStatus(string keyValue, int status)
        {
            var job = await repository.FindEntity(a => a.Id == keyValue);
            if (job == null)
            {
                throw new Exception("任务不存在");
            }
            if (status == 0) //停止
            {
                TriggerKey triggerKey = new TriggerKey(job.JobName, job.JobGroup);
                // 停止触发器
                await _scheduler.PauseTrigger(triggerKey);
                // 移除触发器
                await _scheduler.UnscheduleJob(triggerKey);
                // 删除任务
                await _scheduler.DeleteJob(new JobKey(job.JobName, job.JobGroup));
                job.EnabledMark = false;
                job.EndRunTime = DateTime.Now;
            }
            else  //启动
            {
                DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(job.StarRunTime, 1);
                DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(DateTime.MaxValue.AddDays(-1), 1);

                ITrigger trigger = TriggerBuilder.Create()
                                                 .StartAt(starRunTime)
                                                 .EndAt(endRunTime)
                                                 .WithIdentity(job.JobName, job.JobGroup)
                                                 .WithCronSchedule(job.CronExpress)
                                                 .Build();
                ((CronTriggerImpl)trigger).MisfireInstruction = MisfireInstruction.CronTrigger.DoNothing;
                // 判断数据库中有没有记录过，有的话，quartz会自动从数据库中提取信息创建 schedule
                if (!await _scheduler.CheckExists(new JobKey(job.JobName, job.JobGroup)))
                {
                    IJobDetail jobdetail = JobBuilder.Create<JobExecute>().WithIdentity(job.JobName, job.JobGroup).Build();
                    jobdetail.JobDataMap.Add("Id", job.Id);

                    await _scheduler.ScheduleJob(jobdetail, trigger);
                }

                job.EnabledMark = true;
                job.StarRunTime = DateTime.Now;
            }
            job.Modify(job.Id);
            await repository.Update(job);
        }

        public async Task DoNow(string keyValue, bool commit = true)
        {
            // 获取数据库中的任务
            var dbJobEntity = await GetForm(keyValue);
            if (dbJobEntity != null)
            {
                DateTime now = DateTime.Now;
                #region 执行任务
                OpenJobLogEntity log = new OpenJobLogEntity();
                log.Id = Utils.GuId();
                log.JobId = keyValue;
                log.JobName = dbJobEntity.JobName;
                log.CreatorTime = now;
                repository.unitOfWork.CurrentBeginTrans();
                AlwaysResult result = new AlwaysResult();
                if (dbJobEntity.JobType == 0)
                {
                    //反射执行就行
                    var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
                    //反射取指定前后缀的dll
                    var referencedAssemblies = Directory.GetFiles(path, "WaterCloud.*.dll").Select(Assembly.LoadFrom).ToArray();
                    var types = referencedAssemblies
                        .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces()
                        .Contains(typeof(IJobTask)))).ToArray();
                    string filename = dbJobEntity.FileName;
                    var implementType = types.Where(x => x.IsClass && x.FullName == filename).FirstOrDefault();
                    var obj = System.Activator.CreateInstance(implementType, repository.unitOfWork);       // 创建实例(带参数)
                    MethodInfo method = implementType.GetMethod("Start", new Type[] { });      // 获取方法信息
                    object[] parameters = null;
                    result = ((Task<AlwaysResult>)method.Invoke(obj, parameters)).GetAwaiter().GetResult();     // 调用方法，参数为空
                    if (result.state.ToString() == ResultType.success.ToString())
                    {
                        log.EnabledMark = true;
                        log.Description = "执行成功，" + result.message.ToString();
                    }
                    else
                    {
                        log.EnabledMark = false;
                        log.Description = "执行失败，" + result.message.ToString();
                    }
                }
                else if (dbJobEntity.JobType == 5)
                {
                    try
                    {
                        repository.ChangeEntityDb(GlobalContext.SystemConfig.MainDbNumber);
                        repository.unitOfWork.CurrentBeginTrans();
                        if (!string.IsNullOrEmpty(dbJobEntity.JobSqlParm))
                        {
                            var dic = dbJobEntity.JobSqlParm.ToObject<Dictionary<string, object>>();
                            List<SugarParameter> list = new List<SugarParameter>();
                            foreach (var item in dic)
                            {
                                list.Add(new SugarParameter(item.Key, item.Value));
                            }
                            var dbResult = await repository.Db.Ado.SqlQueryAsync<dynamic>(dbJobEntity.JobSql, list);
                            log.EnabledMark = true;
                            log.Description = "执行成功，" + dbResult.ToJson();
                        }
                        else
                        {
                            var dbResult = await repository.Db.Ado.SqlQueryAsync<dynamic>(dbJobEntity.JobSql);
                            log.EnabledMark = true;
                            log.Description = "执行成功，" + dbResult.ToJson();
                        }
                        repository.unitOfWork.CurrentCommit();
                    }
                    catch (Exception ex)
                    {
                        log.EnabledMark = false;
                        log.Description = "执行失败，" + ex.Message;
                        repository.unitOfWork.CurrentRollback();
                    }
                }
                else
                {
                    HttpMethod method = HttpMethod.Get;
                    switch (dbJobEntity.JobType)
                    {
                        case 1:
                            method = HttpMethod.Get;
                            break;
                        case 2:
                            method = HttpMethod.Post;
                            break;
                        case 3:
                            method = HttpMethod.Put;
                            break;
                        case 4:
                            method = HttpMethod.Delete;
                            break;
                    }
                    var dic = dbJobEntity.RequestHeaders.ToObject<Dictionary<string, string>>();
                    if (dic == null)
                    {
                        dic = new Dictionary<string, string>();
                    }
                    //请求头添加租户号
                    dic.Add("dbNumber", dbJobEntity.DbNumber);
                    try
                    {
                        var temp = await _httpClient.ExecuteAsync(dbJobEntity.RequestUrl, method, dbJobEntity.RequestString, dic);
                        log.EnabledMark = true;
                        log.Description = $"执行成功，{temp}";
                    }
                    catch (Exception ex)
                    {
                        log.EnabledMark = false;
                        log.Description = "执行失败，" + ex.Message.ToString();
                    }
                }
                #endregion
                repository.ChangeEntityDb(GlobalContext.SystemConfig.MainDbNumber);
                repository.unitOfWork.CurrentBeginTrans();
                //记录执行日志
                if (log.EnabledMark == true)
                {
                    await repository.Update(a => a.Id == keyValue, a => new OpenJobEntity
                    {
                        LastRunMark = true,
                        LastRunTime = now
                    });
                }
                else
                {
                    await repository.Update(a => a.Id == keyValue, a => new OpenJobEntity
                    {
                        LastRunMark = false,
                        LastRunTime = now,
                        LastRunErrTime = now,
                        LastRunErrMsg = log.Description
                    });
                }
                if (dbJobEntity.IsLog=="是")
				{
                    string HandleLogProvider = GlobalContext.SystemConfig.HandleLogProvider;
                    if (HandleLogProvider != Define.CACHEPROVIDER_REDIS)
                    {
                        await repository.Db.Insertable(log).ExecuteCommandAsync();
                    }
                    else
                    {
                        await HandleLogHelper.HSetAsync(log.JobId, log.Id, log);
                    }
                }
                if (commit)
                {
                    repository.unitOfWork.CurrentCommit();
                }
            }
        }

        public async Task DeleteLogForm(string keyValue)
        {
            if (HandleLogProvider != Define.CACHEPROVIDER_REDIS)
            {
                await repository.Db.Deleteable<OpenJobLogEntity>(a => a.JobId == keyValue).ExecuteCommandAsync();
            }
            else
            {
                string[] list = HandleLogHelper.HGetAll<OpenJobLogEntity>(keyValue).Keys.ToArray();
                await HandleLogHelper.HDelAsync(keyValue, list);
            }
        }
        #endregion
    }
}
