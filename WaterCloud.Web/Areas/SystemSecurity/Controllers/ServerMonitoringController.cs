/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterCloud.Application.SystemSecurity;
using WaterCloud.Code;

namespace WaterCloud.Web.Areas.SystemSecurity.Controllers
{
    public class ServerMonitoringController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetServerDataJson()
        {
            //windows环境
            var arm = ServerStateHelper.GetUseARM();
            var cpu = ServerStateHelper.GetCPU();
            var iis = ServerStateHelper.GetIISConnection();
            return Content(new { ARM = arm, CPU = cpu, IIS = iis }.ToJson());
        }
        [HttpGet]
        public ActionResult GetServerData()
        {
            var data = new ServerStateApp().GetList(2).OrderBy(a => a.F_Date).ToList() ;
            return Content(data.ToJson());
        }
    }
}
