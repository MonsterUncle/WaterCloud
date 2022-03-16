using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl.Triggers;
using Quartz.Spi;
using SqlSugar;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Service.SystemSecurity;

namespace WaterCloud.Service.AutoJob
{
    public class JobExecute : IJob
    {
        private IScheduler _scheduler;
        private ISchedulerFactory _schedulerFactory;
        private IJobFactory _iocJobfactory;
        private readonly IHttpClientFactory _httpClient;

        public JobExecute(ISchedulerFactory schedulerFactory, IJobFactory iocJobfactory, IHttpClientFactory httpClient)
        {
            _scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            _scheduler.JobFactory = iocJobfactory;
            _schedulerFactory = schedulerFactory;
            _iocJobfactory = iocJobfactory;
            _httpClient = httpClient;
        }
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(async () =>
            {
                string jobId = "";
                JobDataMap jobData = null;
                OpenJobEntity dbJobEntity = null;
                DateTime now = DateTime.Now;
                using (UnitOfWork unitwork = GlobalContext.ServiceProvider.GetService(typeof(IUnitOfWork)) as UnitOfWork)
                {
                    try
                    {
                        jobData = context.JobDetail.JobDataMap;
                        jobId = jobData.GetString("Id");
                        OpenJobsService autoJobService = new OpenJobsService(unitwork, _schedulerFactory, _iocJobfactory, _httpClient);
                        // 获取数据库中的任务
                        dbJobEntity = await autoJobService.GetForm(jobId);
                        if (dbJobEntity != null)
                        {
                            if (dbJobEntity.EnabledMark == true)
                            {
                                CronTriggerImpl trigger = context.Trigger as CronTriggerImpl;
                                if (trigger != null)
                                {
                                    if (trigger.CronExpressionString != dbJobEntity.CronExpress)
                                    {
                                        // 更新任务周期
                                        trigger.CronExpressionString = dbJobEntity.CronExpress;
                                        await _scheduler.RescheduleJob(trigger.Key, trigger);
                                        return;
                                    }
                                    #region 执行任务
                                    OpenJobLogEntity log = new OpenJobLogEntity();
                                    log.Id = Utils.GuId();
                                    log.JobId = jobId;
                                    log.JobName = dbJobEntity.JobName;
                                    log.CreatorTime = now;
                                    unitwork.BeginTrans();
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
                                        unitwork.GetDbClient().ChangeDatabase(dbJobEntity.DbNumber);
                                        var obj = System.Activator.CreateInstance(implementType, unitwork);       // 创建实例(带参数)
                                        MethodInfo method = implementType.GetMethod("Start", new Type[] { });      // 获取方法信息
                                        object[] parameters = null;
                                        result = ((Task<AlwaysResult>)method.Invoke(obj, parameters)).GetAwaiter().GetResult();     // 调用方法，参数为空
                                        unitwork.GetDbClient().ChangeDatabase(GlobalContext.SystemConfig.MainDbNumber);
                                        if (result.state.ToString() == ResultType.success.ToString())
                                        {
                                            log.EnabledMark = true;
                                            log.Description = "执行成功，" + result.message.ToString();
                                            await unitwork.GetDbClient().Updateable<OpenJobEntity>(a => new OpenJobEntity
                                            {
                                                LastRunMark = true,
                                                LastRunTime = now,
                                            }).Where(a => a.Id == jobId).ExecuteCommandAsync();
                                        }
                                        else
                                        {
                                            log.EnabledMark = false;
                                            log.Description = "执行失败，" + result.message.ToString();
                                            await unitwork.GetDbClient().Updateable<OpenJobEntity>(a => new OpenJobEntity
                                            {
                                                LastRunMark = false,
                                                LastRunTime = now,
                                                LastRunErrTime = now,
                                                LastRunErrMsg = log.Description
                                            }).Where(a => a.Id == jobId).ExecuteCommandAsync();
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
                                        unitwork.GetDbClient().ChangeDatabase(GlobalContext.SystemConfig.MainDbNumber);
                                        try
                                        {
                                            var temp = await new HttpWebClient(_httpClient).ExecuteAsync(dbJobEntity.RequestUrl, method, dbJobEntity.RequestString, dic);
                                            log.EnabledMark = true;
                                            log.Description = "执行成功。";
                                            await unitwork.GetDbClient().Updateable<OpenJobEntity>(a => new OpenJobEntity
                                            {
                                                LastRunMark = true,
                                                LastRunTime = now,
                                            }).Where(a => a.Id == jobId).ExecuteCommandAsync();
                                        }
                                        catch (Exception ex)
                                        {
                                            log.EnabledMark = false;
                                            log.Description = "执行失败，" + ex.Message.ToString();
                                            await unitwork.GetDbClient().Updateable<OpenJobEntity>(a => new OpenJobEntity
                                            {
                                                LastRunMark = false,
                                                LastRunTime = now,
                                                LastRunErrTime = now,
                                                LastRunErrMsg = log.Description
                                            }).Where(a => a.Id == jobId).ExecuteCommandAsync();
                                        }
                                    }

                                    #endregion
                                    if (dbJobEntity.IsLog == "是")
                                    {
                                        string HandleLogProvider = GlobalContext.SystemConfig.HandleLogProvider;
                                        if (HandleLogProvider != Define.CACHEPROVIDER_REDIS)
                                        {
                                            unitwork.GetDbClient().Insertable(log).ExecuteCommand();
                                        }
                                        else
                                        {
                                            await HandleLogHelper.HSetAsync(log.JobId, log.Id, log);
                                        }
                                    }
                                    unitwork.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        unitwork.CurrentRollback();
                        LogHelper.WriteWithTime(ex);
                    }
                }
            });
        }
    }
}
