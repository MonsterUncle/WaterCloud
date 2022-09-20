/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WaterCloud.Code;
using WaterCloud.Code.Model;

namespace WaterCloud.Web.Areas.SystemSecurity.Controllers
{
	[Area("SystemSecurity")]
	public class AppLogController : BaseController
	{
		[HttpGet]
		[HandlerAjaxOnly]
		public async Task<ActionResult> GetGridJson(Pagination pagination, int timetype = 2)
		{
			return await Task.Run(() =>
			{
				//导出全部页使用
				if (pagination.rows == 0 && pagination.page == 0)
				{
					pagination.rows = 99999999;
					pagination.page = 1;
				}
				List<AppLogEntity> list = new List<AppLogEntity>();
				string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
				getDirectory(list, logPath);
				DateTime startTime = DateTime.Now.ToString("yyyy-MM-dd").ToDate();
				DateTime endTime = DateTime.Now.ToString("yyyy-MM-dd").ToDate().AddDays(1);
				switch (timetype)
				{
					case 1:
						break;

					case 2:
						startTime = startTime.AddDays(-7);
						break;

					case 3:
						startTime = startTime.AddMonths(-1);
						break;

					case 4:
						startTime = startTime.AddMonths(-3);
						break;

					default:
						break;
				}
				list = list.Where(a => (a.FileName.Split('.')[0]).CompareTo(startTime.ToString("yyyy-MM-dd")) >= 0 && (a.FileName.Split('.')[0]).CompareTo(endTime.ToString("yyyy-MM-dd")) <= 0).ToList();
				pagination.records = list.Count();
				list = list.OrderBy(a => a.FileName).Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows).ToList();
				return Success(pagination.records, list);
			});
		}

		[HttpGet]
		[HandlerAjaxOnly]
		public async Task<ActionResult> GetFormJson(string keyValue)
		{
			return await Task.Run(() =>
			{
				string content;
				string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", keyValue.Split('.')[0].Substring(0, 7), keyValue);
				using (StreamReader sr = new StreamReader(logPath))
				{
					content = sr.ReadToEnd();
				}
				return Success("操作成功", HttpUtility.HtmlEncode(content));
			});
		}

		/// <summary>
		/// 获得指定路径下所有文件名
		/// </summary>
		/// <param name="sw">列表</param>
		/// <param name="path">文件夹路径</param>
		public static void getDirectory(List<AppLogEntity> sw, string path)
		{
			DirectoryInfo root = new DirectoryInfo(path);
			foreach (FileInfo f in root.GetFiles())
			{
				AppLogEntity app = new AppLogEntity();
				app.FileName = f.Name;
				sw.Add(app);
			}
			foreach (DirectoryInfo d in root.GetDirectories())
			{
				getDirectory(sw, d.FullName);
			}
		}
	}
}