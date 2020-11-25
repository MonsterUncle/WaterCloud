using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterCloud.Code;
using WaterCloud.DingTalk;
using WaterCloud.DingTalk.Entities;
using WaterCloud.Application.DingTalkManage;
using WaterCloud.Application.CommonService;
using FluentScheduler;
using WaterCloud.Application.SystemSecurity;
using WaterCloud.Entity.SystemSecurity;

namespace WaterCloud.SystemTask
{
    public class EveryMinRemindTask : IJob
    {
        /// <summary>
        /// 开启执行任务
        /// </summary>
        void IJob.Execute()
        {
            RemindDemo();
        }

        /// <summary>
        /// 保存当前服务器状态
        /// </summary>
        public void RemindDemo()
        {
            DateTime nowtime = DateTime.Now;
            try
            {
                //保存数据
                ServerStateEntity entity = new ServerStateEntity();
                entity.F_ARM = ServerStateHelper.GetUseARM();
                entity.F_CPU = ServerStateHelper.GetCPU();
                entity.F_IIS = ServerStateHelper.GetIISConnection();
                entity.F_WebSite = ServerStateHelper.GetSiteName();
                new ServerStateApp().SubmitForm(entity);
            }
            catch (Exception ex)
            {
                return;
            }
        }

       

    }
   
   
}
