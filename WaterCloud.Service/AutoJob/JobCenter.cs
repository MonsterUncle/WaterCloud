using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SqlSugar;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using WaterCloud.Code;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Service.SystemSecurity;

namespace WaterCloud.Service.AutoJob
{
    /// <summary>
    /// quartz 主机服务
    /// </summary>
    [DisallowConcurrentExecution]
    public class JobCenter : IHostedService
    {
        /// <summary>
        /// 定时作业计划生成工厂，这一项在startup有配置集群模式
        /// </summary>
        private readonly ISchedulerFactory _schedulerFactory;
        /// <summary>
        /// 定时作业工厂
        /// </summary>
        private readonly IJobFactory _jobFactory;

        private OpenJobsService _service;
        private IScheduler _scheduler;

        /// <summary>
        /// 构造注入
        /// </summary>
        public JobCenter(OpenJobsService service, ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            _service = service;
            _jobFactory = jobFactory;
            _schedulerFactory = schedulerFactory;
        }
        /// <summary>
        /// 批量启动定时任务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            _scheduler.JobFactory = _jobFactory;

            List<OpenJobEntity> obj = await _service.GetAllList(null);
            obj = obj.Where(a => a.F_EnabledMark == true).ToList();
            if (obj.Count > 0)
            {
                await AddScheduleJob(obj, cancellationToken);
            }
            await _scheduler.Start();
            //if (!GlobalContext.SystemConfig.Debug)
            //{
            //    List<OpenJobEntity> obj = await new OpenJobService().GetList(null);
            //    if (obj.Count>0)
            //    {
            //        AddScheduleJob(obj);
            //    }
            //}
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await ClearScheduleJob();
        }
        #region 添加任务计划
        /// <summary>
        /// 添加任务计划
        /// </summary>
        /// <returns></returns>
        private async Task AddScheduleJob(List<OpenJobEntity> entityList, CancellationToken cancellationToken)
        {
            try
            {
                foreach (OpenJobEntity entity in entityList)
                {
                    entity.F_StarRunTime = DateTime.Now;
                    entity.F_EndRunTime = DateTime.Now.AddSeconds(-1);
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(entity.F_StarRunTime, 1);
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(DateTime.MaxValue.AddDays(-1), 1);
                    await _service.SubmitForm(entity, entity.F_Id);
                    
                    ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                                 .StartAt(starRunTime)
                                                 .EndAt(endRunTime)
                                                 .WithIdentity(entity.F_JobName, entity.F_JobGroup)
                                                 .WithCronSchedule(entity.F_CronExpress)
                                                 .Build();

                    // 判断数据库中有没有记录过，有的话，quartz会自动从数据库中提取信息创建 schedule
                    if (!await _scheduler.CheckExists(new JobKey(entity.F_JobName,entity.F_JobGroup)))
                    {
                        IJobDetail job = JobBuilder.Create<JobExecute>().WithIdentity(entity.F_JobName, entity.F_JobGroup).Build();
                        job.JobDataMap.Add("F_Id", entity.F_Id);

                        await _scheduler.ScheduleJob(job, trigger, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
        }
        #endregion

        #region 清除任务计划
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
        #endregion
    }
}
