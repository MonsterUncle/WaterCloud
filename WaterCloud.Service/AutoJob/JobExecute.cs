using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Chloe;
using Quartz;
using Quartz.Impl.Triggers;
using Quartz.Spi;
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
            _scheduler = schedulerFactory.GetScheduler().Result;
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
                try
                {
                    jobData = context.JobDetail.JobDataMap;
                    jobId = jobData["F_Id"].ToString();
                    using (IDbContext _context= DBContexHelper.Contex())
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
                                    }
                                    #region 执行任务
                                    //反射执行就行
                                    var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
                                    var referencedAssemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();
                                    var types = referencedAssemblies
                                        .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces()
                                        .Contains(typeof(IJobTask)))).ToArray();
                                    string filename = dbJobEntity.F_FileName;
                                    var implementType = types.Where(x => x.IsClass && x.FullName == filename).FirstOrDefault();
                                    var obj = System.Activator.CreateInstance(implementType, _context);       // 创建实例(带参数)
                                    MethodInfo method = implementType.GetMethod("Start", new Type[] { });      // 获取方法信息
                                    object[] parameters = null;
                                    method.Invoke(obj, parameters);                           // 调用方法，参数为空
                                    #endregion
                                    dbJobEntity.F_LastRunTime = DateTime.Now;
                                    await autoJobService.SubmitForm(dbJobEntity, jobId);
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
