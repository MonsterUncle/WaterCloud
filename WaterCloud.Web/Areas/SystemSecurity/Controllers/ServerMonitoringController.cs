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
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using WaterCloud.Service.SystemSecurity;
using WaterCloud.Code;

namespace WaterCloud.Web.Areas.SystemSecurity.Controllers
{
    [Area("SystemSecurity")]
    public class ServerMonitoringController : ControllerBase
    {
        private readonly ServerStateService _serverStateService;
        public ServerMonitoringController(ServerStateService serverStateService)
        {
            _serverStateService = serverStateService;
        }
        [HttpGet]
        public ActionResult GetServerDataJson()
        {
            //windows环境
            var computer = ComputerHelper.GetComputerInfo();
            var arm = computer.RAMRate;
            var cpu = computer.CPURate;
            var iis = computer.RunTime;
            var TotalRAM = computer.TotalRAM;
            string ip = NetHelper.GetWanIp();
            string ipLocation = IpLocationHelper.GetIpLocation(ip);
            var IP = string.Format("{0} ({1})", ip, ipLocation);
            return Content(new { ARM = arm, CPU = cpu, IIS = iis , TotalRAM = TotalRAM,IP=IP }.ToJson());
        }
        [HttpGet]
        public ActionResult GetServerData()
        {
            var data = _serverStateService.GetList(2).OrderBy(a => a.F_Date).ToList() ;
            return Content(data.ToJson());
        }
    }
}
