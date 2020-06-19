using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Chloe;
using NPOI.HSSF.Record.Chart;
using Quartz;
using Quartz.Impl.Triggers;
using WaterCloud.Code;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Service.SystemSecurity;

namespace WaterCloud.Service.AutoJob
{
    public class JobExecute : IJob
    {
        private OpenJobsService autoJobService;
        public JobExecute(IDbContext dbContext)
        {
            autoJobService = new OpenJobsService(dbContext);
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(async () =>
            {
                String jobId="";
                JobDataMap jobData = null;
                OpenJobEntity dbJobEntity = null;
                try
                {
                    jobData = context.JobDetail.JobDataMap;
                    jobId = jobData["F_Id"].ToString();
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
                                    await JobScheduler.GetScheduler().RescheduleJob(trigger.Key, trigger);
                                }

                                #region 执行任务
                                //switch (context.JobDetail.Key.Name)
                                //{
                                //    case "数据库备份":
                                //        obj = await new DatabasesBackupJob().Start();
                                //        break;
                                //    case "服务器状态更新":
                                //        obj = await new SaveServerStateJob().Start();
                                //        break;
                                //}
                                //反射执行就行
                                var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
                                var referencedAssemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();
                                var types = referencedAssemblies
                                    .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces()
                                    .Contains(typeof(IJobTask)))).ToArray();
                                string filename = dbJobEntity.F_FileName;
                                var implementType = types.Where(x => x.IsClass&&x.FullName== filename).FirstOrDefault();
                                var obj = System.Activator.CreateInstance(implementType);       // 创建实例
                                MethodInfo method = implementType.GetMethod("Start", new Type[] { });      // 获取方法信息
                                object[] parameters = null;
                                method.Invoke(obj, parameters);                           // 调用方法，参数为空
                                #endregion
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
