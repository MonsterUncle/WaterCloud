/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Service.SystemSecurity;

namespace WaterCloud.Web.Areas.SystemSecurity.Controllers
{
	[Area("SystemSecurity")]
	public class ServerMonitoringController : BaseController
	{
		public ServerStateService _serverStateService { get; set; }

		[HttpGet]
		public async Task<ActionResult> GetServerDataJson()
		{
			return await Task.Run(() =>
			{
				//windows环境
				var computer = ComputerHelper.GetComputerInfo();
				var arm = computer.RAMRate;
				var cpu = computer.CPURate;
				var iis = computer.RunTime;
				var TotalRAM = computer.TotalRAM;
				string ip = WebHelper.GetWanIp();
				string ipLocation = WebHelper.GetIpLocation(ip);
				var IP = string.Format("{0} ({1})", ip, ipLocation);
				return Content(new { ARM = arm, CPU = cpu, IIS = iis, TotalRAM = TotalRAM, IP = IP }.ToJson());
			});
		}

		[HttpGet]
		public async Task<ActionResult> GetServerData()
		{
			var data = (await _serverStateService.GetList(2)).OrderBy(a => a.F_Date).ToList();
			return Content(data.ToJson());
		}
	}
}