using System;
using System.IO;
using System.Linq;
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

        public JobExecute(ISchedulerFactory schedulerFactory, IJobFactory iocJobfactory)
        {
            _scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            _scheduler.JobFactory = iocJobfactory;
            _schedulerFactory = schedulerFactory;
            _iocJobfactory = iocJobfactory;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(async () =>
            {
                string jobId="";
                JobDataMap jobData = null;
                OpenJobEntity dbJobEntity = null;
                DateTime now = DateTime.Now;
                try
                {
                    jobData = context.JobDetail.JobDataMap;
                    jobId = jobData["F_Id"].ToString();
                    using (var _context= new SqlSugarClient(DBContexHelper.Contex()))
                    {
                        OpenJobsService autoJobService = new OpenJobsService(_context, _schedulerFactory,_iocJobfactory);
                        // 获取数据库中的任务
                        dbJobEntity = await autoJobService.GetForm(jobId);
                        if (dbJobEntity != null)
                        {
                            if (dbJobEntity.F_EnabledMark == true)
                            {
                                CronTriggerImpl trigger = context.Trigger as CronTriggerImpl;
                                if (trigger != null)
                                {
                                    if (trigger.CronExpressionString != dbJobEntity.F_CronExpress)
                                    {
                                        // 更新任务周期
                                        trigger.CronExpressionString = dbJobEntity.F_CronExpress;
                                        await _scheduler.RescheduleJob(trigger.Key, trigger);
                                        return;
                                    }
                                    #region 执行任务
                                    _context.BeginTran();
                                    //反射执行就行
                                    var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
                                    //反射取指定前后缀的dll
                                    var referencedAssemblies = Directory.GetFiles(path, "WaterCloud.*.dll").Select(Assembly.LoadFrom).ToArray();
                                    var types = referencedAssemblies
                                        .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces()
                                        .Contains(typeof(IJobTask)))).ToArray();
                                    string filename = dbJobEntity.F_FileName;
                                    var implementType = types.Where(x => x.IsClass && x.FullName == filename).First();
                                    var obj = System.Activator.CreateInstance(implementType,new UnitOfWork(_context));       // 创建实例(带参数)
                                    MethodInfo method = implementType.GetMethod("Start", new Type[] { });      // 获取方法信息
                                    object[] parameters = null;
                                    var temp = (Task<AlwaysResult>)method.Invoke(obj, parameters);     // 调用方法，参数为空
                                    #endregion
                                    //需要同步，不然数据库连接会断开
                                    _context.Updateable<OpenJobEntity>(a => new OpenJobEntity
                                    {
                                        F_LastRunTime = now
                                    }).Where(t => t.F_Id == jobId).ExecuteCommand();
                                    OpenJobLogEntity log = new OpenJobLogEntity();
                                    log.F_Id = Utils.GuId();
                                    log.F_JobId = jobId;
                                    log.F_JobName = dbJobEntity.F_JobName;
                                    log.F_CreatorTime = now;
                                    if (temp.Result.state.ToString() == ResultType.success.ToString())
                                    {
                                        log.F_EnabledMark = true;
                                        log.F_Description = "执行成功，" + temp.Result.message.ToString();
                                    }
                                    else
                                    {
                                        log.F_EnabledMark = false;
                                        log.F_Description = "执行失败，" + temp.Result.message.ToString();
                                    }
                                    string HandleLogProvider = GlobalContext.SystemConfig.HandleLogProvider;
                                    if (HandleLogProvider != Define.CACHEPROVIDER_REDIS)
                                    {
                                        _context.Insertable(log).ExecuteCommand();
                                    }
                                    else
                                    {
                                        await HandleLogHelper.HSetAsync(log.F_JobId, log.F_Id, log);
                                    }
                                    _context.CommitTran();
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.WriteWithTime(ex);
                }
            });
        }
    }
}
