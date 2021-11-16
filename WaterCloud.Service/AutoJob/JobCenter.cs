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
using Quartz.Impl.Triggers;
using System.Collections.ObjectModel;

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

        public JobCenter(OpenJobsService service, ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            _service = service;
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
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

            List<OpenJobEntity> obj = await _service.GetList();
            obj = obj.Where(a => a.F_EnabledMark == true).ToList();
            if (obj.Count > 0)
            {
                foreach (OpenJobEntity entity in obj)
                {
                    entity.F_StarRunTime = DateTime.Now;
                    entity.F_EndRunTime = DateTime.Now.AddSeconds(-1);
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(entity.F_StarRunTime, 1);
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(DateTime.MaxValue.AddDays(-1), 1);
                    //更新数据库
                    await _service.SubmitForm(entity, entity.F_Id);
                    //注册并启动作业
                    await _service.AddJob(entity);
                }
                if (!_scheduler.IsStarted)
                {
                    await _scheduler.Start();
                }
            }
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken)
            => await _scheduler?.Shutdown(cancellationToken);


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