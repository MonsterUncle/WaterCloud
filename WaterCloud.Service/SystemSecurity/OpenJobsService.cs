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
            _httpClient =new HttpWebClient(httpClient);
        }
        /// <summary>
        /// 加载列表
        /// </summary>
        public async Task<List<OpenJobEntity>> GetLookList(Pagination pagination, string keyword = "")
        {
            var DbNumber = OperatorProvider.Provider.GetCurrent().DbNumber;
            var list = repository.IQueryable().Where(a=>a.F_DbNumber == DbNumber);
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
                return repository.Db.Queryable<OpenJobLogEntity>().Where(a => a.F_JobId == keyValue).OrderBy(a => a.F_CreatorTime,OrderByType.Desc).ToList();
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
                entity.Modify(keyValue);
                await repository.Update(entity);
            }
            else
            {
                entity.Create();
                entity.F_DbNumber = OperatorProvider.Provider.GetCurrent().DbNumber;
                await repository.Insert(entity);
            }
			if (entity.F_DoItNow==true)
			{
                await ChangeJobStatus(entity.F_Id,1);
                await DoNow(entity.F_Id,false);
            }
            repository.unitOfWork.CurrentCommit();
        }

        public async Task DeleteForm(string keyValue)
        {
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
                DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(job.F_StarRunTime, 1);
                DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(DateTime.MaxValue.AddDays(-1), 1);
                IJobDetail jobdetail = JobBuilder.Create<JobExecute>().WithIdentity(job.F_JobName, job.F_JobGroup).Build();
                jobdetail.JobDataMap.Add("F_Id", job.F_Id);
                ITrigger trigger = TriggerBuilder.Create()
                                                 .StartAt(starRunTime)
                                                 .EndAt(endRunTime)
                                                 .WithIdentity(job.F_JobName, job.F_JobGroup)
                                                 .WithCronSchedule(job.F_CronExpress)
                                                 .Build();
                await _scheduler.ScheduleJob(jobdetail, trigger);
                job.F_EnabledMark = true;
                job.F_StarRunTime = DateTime.Now;
            }
            job.Modify(job.F_Id);
            await repository.Update(job);
        }

        public async Task DoNow(string keyValue,bool commit = true)
        {
            // 获取数据库中的任务
            var dbJobEntity = await GetForm(keyValue);
            if (dbJobEntity != null)
            {
                DateTime now = DateTime.Now;
                #region 执行任务
                OpenJobLogEntity log = new OpenJobLogEntity();
                log.F_Id = Utils.GuId();
                log.F_JobId = keyValue;
                log.F_JobName = dbJobEntity.F_JobName;
                log.F_CreatorTime = now;
                repository.unitOfWork.CurrentBeginTrans();
                AlwaysResult result=new AlwaysResult();
                if (dbJobEntity.F_JobType==0)
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
                    var obj = System.Activator.CreateInstance(implementType, repository.unitOfWork);       // 创建实例(带参数)
                    MethodInfo method = implementType.GetMethod("Start", new Type[] { });      // 获取方法信息
                    object[] parameters = null;
                    result = ((Task<AlwaysResult>)method.Invoke(obj, parameters)).GetAwaiter().GetResult();     // 调用方法，参数为空
                    if (result.state.ToString() == ResultType.success.ToString())
                    {
                        log.F_EnabledMark = true;
                        log.F_Description = "执行成功，" + result.message.ToString();
                    }
                    else
                    {
                        log.F_EnabledMark = false;
                        log.F_Description = "执行失败，" + result.message.ToString();
                    }
                }
				else
				{
                    HttpMethod method=HttpMethod.Get;
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
                    }
                    catch (Exception ex)
					{
                        log.F_EnabledMark = false;
                        log.F_Description = "执行失败，" + ex.Message.ToString();
                    }
                }
                #endregion
                await repository.Update(a => a.F_Id == keyValue, a => new OpenJobEntity
                {
                    F_LastRunTime = now
                });
                string HandleLogProvider = GlobalContext.SystemConfig.HandleLogProvider;
                if (HandleLogProvider != Define.CACHEPROVIDER_REDIS)
                {
                    await repository.Db.Insertable(log).ExecuteCommandAsync();
                }
                else
                {
                    await HandleLogHelper.HSetAsync(log.F_JobId, log.F_Id, log);
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
                await repository.Db.Deleteable<OpenJobLogEntity>(a => a.F_JobId == keyValue).ExecuteCommandAsync();
            }
            else
            {
                string[] list= HandleLogHelper.HGetAll<OpenJobLogEntity>(keyValue).Keys.ToArray();
                await HandleLogHelper.HDelAsync(keyValue, list);
            }
        }
        #endregion
    }
}
