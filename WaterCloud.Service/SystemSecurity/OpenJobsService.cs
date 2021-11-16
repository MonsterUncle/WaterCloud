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
using System.Collections.ObjectModel;

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
            var list = repository.IQueryable().Where(a => a.F_DbNumber == DbNumber);
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(a => a.F_JobName.Contains(keyword) || a.F_Description.Contains(keyword));
            }
            list = list.Where(a => a.F_DeleteMark == false);
            return await repository.OrderList(list, pagination);
        }

        public async Task<List<OpenJobLogEntity>> GetLogList(string keyValue)
        {
            if (HandleLogProvider != Define.CACHEPROVIDER_REDIS)
            {
                return repository.Db.Queryable<OpenJobLogEntity>().Where(a => a.F_JobId == keyValue).OrderBy(a => a.F_CreatorTime, OrderByType.Desc).ToList();
            }
            else
            {
                return HandleLogHelper.HGetAll<OpenJobLogEntity>(keyValue).Values.OrderByDescending(a => a.F_CreatorTime).ToList(); ;
            }
        }
        public async Task<List<OpenJobEntity>> GetList(string keyword = "")
        {
            var DbNumber = OperatorProvider.Provider.GetCurrent().DbNumber;
            var query = repository.IQueryable().Where(a => a.F_DbNumber == DbNumber);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_JobName.Contains(keyword));
            }
            return query.Where(a => a.F_DeleteMark == false).ToList();
        }
        public async Task<List<OpenJobEntity>> GetAllList(string keyword = "")
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_JobName.Contains(keyword));
            }
            return query.Where(a => a.F_DeleteMark == false).ToList();
        }
        public async Task<OpenJobEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        public async Task SubmitForm(OpenJobEntity entity, string keyValue)
        {
            repository.unitOfWork.CurrentBeginTrans();
            if (!string.IsNullOrEmpty(keyValue))
            {
                TriggerKey triggerKey = new TriggerKey(entity.F_JobName, entity.F_JobGroup);
                // 停止触发器
                await _scheduler.PauseTrigger(triggerKey);
                // 移除触发器
                await _scheduler.UnscheduleJob(triggerKey);
                // 删除任务
                await _scheduler.DeleteJob(new JobKey(entity.F_JobName, entity.F_JobGroup));

                //注册并启动作业
                await AddJob(entity);

                entity.Modify(keyValue);
                await repository.Update(entity);
            }
            else
            {
                entity.Create();
                await repository.Insert(entity);
            }
            if (entity.F_DoItNow == true)
            {
                await ChangeJobStatus(entity.F_Id, 1);
                await DoNow(entity.F_Id);
            }
            repository.unitOfWork.CurrentCommit();
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
            TriggerKey triggerKey = new TriggerKey(job.F_JobName, job.F_JobGroup);
            await _scheduler.PauseTrigger(triggerKey);
            await _scheduler.UnscheduleJob(triggerKey);
            await _scheduler.DeleteJob(new JobKey(job.F_JobName, job.F_JobGroup));
            await repository.Delete(a => a.F_Id == keyValue);
        }
        #region 定时任务运行相关操作

        /// <summary>
        /// 返回系统的job接口
        /// </summary>
        /// <returns></returns>
        public List<string> QueryLocalHandlers()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(a => a.GetInterfaces()
                    .Contains(typeof(IJobTask))))
                .ToArray();
            return types.Select(a => a.FullName).ToList();
        }

        public async Task ChangeJobStatus(string keyValue, int status)
        {
            var job = await repository.FindEntity(a => a.F_Id == keyValue);
            if (job == null)
            {
                throw new Exception("任务不存在");
            }
            if (status == 0) //停止
            {
                TriggerKey triggerKey = new TriggerKey(job.F_JobName, job.F_JobGroup);
                // 停止触发器
                await _scheduler.PauseTrigger(triggerKey);
                // 移除触发器
                await _scheduler.UnscheduleJob(triggerKey);
                // 删除任务
                await _scheduler.DeleteJob(new JobKey(job.F_JobName, job.F_JobGroup));
                job.F_EnabledMark = false;
                job.F_EndRunTime = DateTime.Now;
            }
            else  //启动
            {
                //注册并启动作业
                await AddJob(job);

                job.F_EnabledMark = true;
                job.F_StarRunTime = DateTime.Now;
            }
            job.Modify(job.F_Id);
            await repository.Update(job);
        }

        public async Task DoNow(string keyValue)
        {
            string jobId = keyValue;
            DateTime now = DateTime.Now;

            var dbJobEntity = await repository.FindEntity(u => u.F_Id == jobId);
            if (dbJobEntity == null)
            {
                throw new Exception("任务不存在");
            }
            try
            {
                using (UnitOfWork unitwork = GlobalContext.ServiceProvider.GetService(typeof(IUnitOfWork)) as UnitOfWork)
                {
                    await JobExecute(dbJobEntity, unitwork);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
        }

        public async Task DeleteLogForm(string keyValue)
        {
            if (HandleLogProvider != Define.CACHEPROVIDER_REDIS)
            {
                await repository.Db.Deleteable<OpenJobLogEntity>(a => a.F_JobId == keyValue).ExecuteCommandAsync();
            }
            else
            {
                string[] list = HandleLogHelper.HGetAll<OpenJobLogEntity>(keyValue).Keys.ToArray();
                await HandleLogHelper.HDelAsync(keyValue, list);
            }
        }
        /// <summary>
        /// 注册作业
        /// </summary>
        /// <param name="entity">任务实体</param>
        /// <returns></returns>
        public async Task AddJob(OpenJobEntity entity)
        {
            DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(entity.F_StarRunTime, 1);
            DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(DateTime.MaxValue.AddDays(-1), 1);
            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                        .StartAt(starRunTime)
                        .EndAt(endRunTime)
                        .WithIdentity(entity.F_JobName, entity.F_JobGroup)
                        .WithCronSchedule(entity.F_CronExpress)
                        .Build();

            //https://www.cnblogs.com/chriskwok/p/12905288.html
            //比如15:28启动，预想结果应该是，在16:00第一次执行，然后类推。
            //结果经常是，15:28就进行了第一次执行，也就是说quartz在启动时不按照预定排程执行了一次。这在实际操作中会带来不小的麻烦。
            ((CronTriggerImpl)trigger).MisfireInstruction = MisfireInstruction.CronTrigger.DoNothing;
            IList<ICronTrigger> triggers = new List<ICronTrigger> { trigger };

            JobKey jobKey = new JobKey(entity.F_JobName, entity.F_JobGroup);

            // 这里一定要先判断是否已经从数据库中加载了Job和Trigger
            if (!await _scheduler.CheckExists(jobKey))
            {
                IJobDetail job = JobBuilder.Create<JobExecute>().WithIdentity(entity.F_JobName, entity.F_JobGroup).Build();
                job.JobDataMap.Add("F_Id", entity.F_Id);

                //如果存在相同名字的Job或Trigger参数replace设置为true可以更新作业的调度计划(如Cron重设)而不报错
                await _scheduler.ScheduleJob(job, new ReadOnlyCollection<ICronTrigger>(triggers), true);
            }
            if (!_scheduler.IsStarted)
            {
                await _scheduler.Start();
            }
        }
        /// <summary>
        /// 执行一次任务
        /// </summary>
        /// <param name="entity">任务实体</param>
        /// <returns></returns>
        public async Task JobExecute(OpenJobEntity dbJobEntity, UnitOfWork unitwork)
        {
            string jobId = "";
            DateTime now = DateTime.Now;
            jobId = dbJobEntity.F_Id;
            if (dbJobEntity != null)
            {
                if (dbJobEntity.F_EnabledMark == true)
                {
                    #region 执行任务
                    OpenJobLogEntity log = new OpenJobLogEntity();
                    log.F_Id = Utils.GetGuid();
                    log.F_JobId = jobId;
                    log.F_JobName = dbJobEntity.F_JobName;
                    log.F_CreatorTime = now;
                    repository.unitOfWork.CurrentBeginTrans();
                    if (dbJobEntity.F_JobType == 0)
                    {
                        //反射执行就行
                        var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
                        //反射取指定前后缀的dll
                        var referencedAssemblies = Directory.GetFiles(path, "WaterCloud.*.dll").Select(Assembly.LoadFrom).ToArray();
                        var types = referencedAssemblies
                            .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces()
                            .Contains(typeof(IJobTask)))).ToArray();
                        string filename = dbJobEntity.F_FileName;
                        var implementType = types.Where(x => x.IsClass && x.FullName == filename).FirstOrDefault();
                        var obj = System.Activator.CreateInstance(implementType, unitwork);       // 创建实例(带参数)
                        MethodInfo method = implementType.GetMethod("Start", new Type[] { });      // 获取方法信息
                        object[] parameters = null;
                        var temp = (Task<AlwaysResult>)method.Invoke(obj, parameters);     // 调用方法，参数为空
                        #endregion
                        if (temp.Result.state.ToString() == ResultType.success.ToString())
                        {
                            log.F_EnabledMark = true;
                            log.F_Description = "执行成功，" + temp.Result.message.ToString();
                            await repository.Update(a => a.F_Id == jobId, a => new OpenJobEntity
                            {
                                F_LastRunMark = true,
                                F_LastRunTime = now
                            });
                        }
                        else
                        {
                            log.F_EnabledMark = false;
                            log.F_Description = "执行失败，" + temp.Result.message.ToString();
                            await repository.Update(a => a.F_Id == jobId, a => new OpenJobEntity
                            {
                                F_LastRunMark = false,
                                F_LastRunTime = now,
                                F_LastRunErrTime = now,
                                F_LastRunErrMsg = temp.Result.message
                            });
                        }
                    }
                    else
                    {
                        HttpMethod method = HttpMethod.Get;
                        switch (dbJobEntity.F_JobType)
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
                        var dic = dbJobEntity.F_RequestHeaders.ToObject<Dictionary<string, string>>();
                        try
                        {
                            var temp = await _httpClient.ExecuteAsync(dbJobEntity.F_RequestUrl, method, dbJobEntity.F_RequestString, dic);
                            log.F_EnabledMark = true;
                            log.F_Description = "执行成功。";

                            await repository.Update(a => a.F_Id == jobId, a => new OpenJobEntity
                            {
                                F_LastRunMark = true,
                                F_LastRunTime = now
                            });
                        }
                        catch (Exception ex)
                        {
                            log.F_EnabledMark = false;
                            log.F_Description = "执行失败，" + ex.Message.ToString();

                            await repository.Update(a => a.F_Id == jobId, a => new OpenJobEntity
                            {
                                F_LastRunMark = false,
                                F_LastRunTime = now,
                                F_LastRunErrTime = now,
                                F_LastRunErrMsg = ex.Message.ToString()
                            });
                        }
                    }
                    //是否记录日志
                    if (dbJobEntity.F_IsLog == "是")
                    {
                        string HandleLogProvider = GlobalContext.SystemConfig.HandleLogProvider;
                        if (HandleLogProvider != Define.CACHEPROVIDER_REDIS)
                        {
                            await repository.Db.Insertable(log).ExecuteCommandAsync();
                        }
                        else
                        {
                            await HandleLogHelper.HSetAsync(log.F_JobId, log.F_Id, log);
                        }
                    }
                    repository.unitOfWork.CurrentCommit();

                }
            }
        }
        #endregion
    }
}
