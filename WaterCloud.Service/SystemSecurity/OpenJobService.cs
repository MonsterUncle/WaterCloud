/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Repository.SystemSecurity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaterCloud.Service.SystemManage;
using System.Linq;
using Quartz;
using WaterCloud.Service.AutoJob;

namespace WaterCloud.Service.SystemSecurity
{
    public class OpenJobService : DataFilterService<OpenJobEntity>, IDenpendency
    {
        private IOpenJobRepository service = new OpenJobRepository();
        private IScheduler _scheduler=JobScheduler.GetScheduler();
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_openjob_";// IP过滤
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        /// <summary>
        /// 加载列表
        /// </summary>
        public async Task<List<OpenJobEntity>> GetLookList(Pagination pagination, string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(u => u.F_JobName.Contains(keyword) || u.F_Description.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark == false);
            return GetFieldsFilterData(await service.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }
        public async Task<List<OpenJobEntity>> GetList(string keyword = "")
        {
            var cachedata = await service.CheckCacheList(cacheKey+"list");
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_JobName.Contains(keyword)).ToList();
            }
            return cachedata.Where(a => a.F_DeleteMark == false).ToList();
        }
        public async Task<OpenJobEntity> GetForm(string keyValue)
        {
            var cachedata = await service.CheckCache(cacheKey, keyValue);
            return cachedata;
        }

        public async Task SubmitForm(OpenJobEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                await service.Update(entity);
                await RedisHelper.DelAsync(cacheKey + keyValue);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
            else
            {
                entity.Create();
                await service.Insert(entity);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
        }
        public async Task DeleteForm(string keyValue)
        {
            await service.Delete(t => t.F_Id == keyValue);
            await RedisHelper.DelAsync(cacheKey + keyValue);
            await RedisHelper.DelAsync(cacheKey + "list");
        }
        #region 定时任务运行相关操作

        /// <summary>
        /// 返回系统的job接口
        /// </summary>
        /// <returns></returns>
        public List<string> QueryLocalHandlers()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces()
                    .Contains(typeof(IJobTask))))
                .ToArray();
            return types.Select(u => u.FullName).ToList();
        }

        public async Task ChangeJobStatus(string keyValue, int status)
        {
            var job = await service.FindEntity(u => u.F_Id == keyValue);
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
                await _scheduler.Start();
                job.F_EnabledMark = true;
                job.F_StarRunTime = DateTime.Now;
            }
            job.Modify(job.F_Id);
            await service.Update(job);
            await RedisHelper.DelAsync(cacheKey + keyValue);
            await RedisHelper.DelAsync(cacheKey + "list");
        }
        #endregion
    }
}
