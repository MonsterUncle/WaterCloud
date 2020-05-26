using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quartz;
using WaterCloud.Code;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Service.SystemSecurity;

namespace WaterCloud.Service.AutoJob
{
    public class JobCenter
    {
        public void Start()
        {
            Task.Run(async () =>
            {
                List<OpenJobEntity> obj = await new OpenJobService().GetList(null);
                if (obj.Count > 0)
                {
                    AddScheduleJob(obj);
                }
                //if (!GlobalContext.SystemConfig.Debug)
                //{
                //    List<OpenJobEntity> obj = await new OpenJobService().GetList(null);
                //    if (obj.Count>0)
                //    {
                //        AddScheduleJob(obj);
                //    }
                //}
            });
        }

        #region 添加任务计划
        /// <summary>
        /// 添加任务计划
        /// </summary>
        /// <returns></returns>
        private void AddScheduleJob(List<OpenJobEntity> entityList)
        {
            try
            {
                foreach (OpenJobEntity entity in entityList)
                {
                    if (entity.F_StarRunTime == null)
                    {
                        entity.F_StarRunTime = DateTime.Now;
                    }
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(entity.F_StarRunTime, 1);
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(DateTime.MaxValue.AddDays(-1), 1);
                    new OpenJobService().SubmitForm(entity, entity.F_Id);
                    var scheduler = JobScheduler.GetScheduler();
                    IJobDetail job = JobBuilder.Create<JobExecute>().WithIdentity(entity.F_JobName, entity.F_JobGroup).Build();
                    job.JobDataMap.Add("F_Id", entity.F_Id);

                    ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                                 .StartAt(starRunTime)
                                                 .EndAt(endRunTime)
                                                 .WithIdentity(entity.F_JobName, entity.F_JobGroup)
                                                 .WithCronSchedule(entity.F_CronExpress)
                                                 .Build();

                    scheduler.ScheduleJob(job, trigger);
                    scheduler.Start();
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
        public void ClearScheduleJob()
        {
            try
            {
                JobScheduler.GetScheduler().Clear();
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
        }
        #endregion
    }
}
