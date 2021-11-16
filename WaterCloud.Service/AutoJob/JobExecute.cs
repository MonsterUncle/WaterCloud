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
                string jobId="";
                JobDataMap jobData = null;
                OpenJobEntity dbJobEntity = null;
                DateTime now = DateTime.Now;
                using (UnitOfWork unitwork = GlobalContext.ServiceProvider.GetService(typeof(IUnitOfWork)) as UnitOfWork)
                {
                    try
                    {
                        jobData = context.JobDetail.JobDataMap;
                        jobId = jobData["F_Id"].ToString();
                        OpenJobsService autoJobService = new OpenJobsService(unitwork, _schedulerFactory, _iocJobfactory, _httpClient);
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
                                }

                                await autoJobService.JobExecute(dbJobEntity, unitwork);
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
