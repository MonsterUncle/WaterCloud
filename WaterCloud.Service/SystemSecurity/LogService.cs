/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Service.SystemManage;

namespace WaterCloud.Service.SystemSecurity
{
	public class LogService : BaseService<LogEntity>, IDenpendency
	{
		public ModuleService moduleservice { get; set; }
		//获取类名

		public LogService(ISqlSugarClient context) : base(context)
		{
		}

		public async Task<List<LogEntity>> GetList(Pagination pagination, int timetype, string keyword = "")
		{
			//获取数据权限
			var result = new List<LogEntity>();
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
			var query = repository.IQueryable();
			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(a => a.F_Account.Contains(keyword) || a.F_Description.Contains(keyword) || a.F_ModuleName.Contains(keyword));
			}

			query = query.Where(a => a.F_Date >= startTime && a.F_Date <= endTime);

			return GetFieldsFilterData(await query.ToPageListAsync(pagination));
		}

		public async Task<List<LogEntity>> GetList()
		{
			return await repository.IQueryable().ToListAsync();
		}

		public async Task RemoveLog(string keepTime)
		{
			DateTime operateTime = DateTime.Now;
			if (keepTime == "7")            //保留近一周
			{
				operateTime = DateTime.Now.AddDays(-7);
			}
			else if (keepTime == "1")       //保留近一个月
			{
				operateTime = DateTime.Now.AddMonths(-1);
			}
			else if (keepTime == "3")       //保留近三个月
			{
				operateTime = DateTime.Now.AddMonths(-3);
			}
			var expression = ExtLinq.True<LogEntity>();
			expression = expression.AndAlso(a => a.F_Date <= operateTime);
			await repository.Delete(expression);
		}

		public async Task WriteDbLog(LogEntity logEntity, OperatorModel user = null)
		{
			logEntity.F_Id = Utils.GuId();
			logEntity.F_Date = DateTime.Now;
			currentuser = OperatorProvider.Provider.GetCurrent();
			if (user == null || string.IsNullOrEmpty(user.UserId))
			{
				user = currentuser;
			}
			var dbNumber = GlobalContext.SystemConfig.MainDbNumber;
			if (user != null)
			{
				dbNumber = user.DbNumber;
			}
			repository.ChangeEntityDb(GlobalContext.SystemConfig.MainDbNumber);
			var systemSet = await repository.Db.Queryable<SystemSetEntity>().Where(a => a.F_DbNumber == GlobalContext.SystemConfig.MainDbNumber).FirstAsync();
			repository.ChangeEntityDb(dbNumber);
			try
			{
				if (user == null || string.IsNullOrEmpty(user.UserId))
				{
					logEntity.F_IPAddress = WebHelper.Ip;
					if (GlobalContext.SystemConfig.LocalLAN != false)
					{
						logEntity.F_IPAddressName = "本地局域网";
					}
					else
					{
						logEntity.F_IPAddressName = WebHelper.GetIpLocation(logEntity.F_IPAddress);
					}
					logEntity.F_CompanyId = systemSet.F_Id;
				}
				else
				{
					logEntity.F_IPAddress = user.LoginIPAddress;
					logEntity.F_IPAddressName = user.LoginIPAddressName;
					logEntity.F_CompanyId = user.CompanyId;
				}
				logEntity.Create();
				if (!string.IsNullOrEmpty(logEntity.F_KeyValue))
				{
					//批量删除时，循环拆分F_KeyValue，以免截断二进制错误
					//方便以后根据F_KeyValue查询；
					var keylist = logEntity.F_KeyValue.Split(",").ToList();
					var loglist = new List<LogEntity>();
					foreach (var key in keylist)
					{
						var log = new LogEntity();
						log = logEntity.ToJson().ToObject<LogEntity>();
						log.F_KeyValue = key;
						log.F_Id = Utils.GuId();
						loglist.Add(log);
					}
					await repository.Insert(loglist);
				}
				else
				{
					await repository.Insert(logEntity);
				}
			}
			catch (Exception)
			{
				logEntity.F_IPAddress = WebHelper.Ip;
				if (GlobalContext.SystemConfig.LocalLAN != false)
				{
					logEntity.F_IPAddressName = "本地局域网";
				}
				else
				{
					logEntity.F_IPAddressName = WebHelper.GetIpLocation(logEntity.F_IPAddress);
				}
				logEntity.F_CompanyId = systemSet.F_Id;
				logEntity.Create();
				await repository.Insert(logEntity);
			}
		}

		private async Task<LogEntity> CreateLog(string className, DbLogType type)
		{
			try
			{
				var moduleitem = (await moduleservice.GetList()).Where(a => a.F_IsExpand == false && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
				if (moduleitem == null)
				{
					throw new Exception();
				}
				var module = (await moduleservice.GetList()).Where(a => a.F_Id == moduleitem.F_ParentId).First();
				return new LogEntity(await CreateModule(module), moduleitem == null ? "" : moduleitem.F_FullName, type.ToString());
			}
			catch (Exception)
			{
				return new LogEntity(className, "", type.ToString());
			}
		}

		private async Task<string> CreateModule(ModuleEntity module, string str = "")
		{
			if (module == null)
			{
				return str;
			}
			str = module.F_FullName + "-" + str;
			if (module.F_ParentId == "0")
			{
				return str;
			}
			else
			{
				var temp = (await moduleservice.GetList()).Where(a => a.F_Id == module.F_ParentId).First();
				return await CreateModule(temp, str);
			}
		}

		public async Task<LogEntity> CreateLog(string message, string className, string keyValue = "", DbLogType? logType = null, bool isError = false)
		{
			LogEntity logEntity;
			if (logType != null)
			{
				logEntity = await CreateLog(className, (DbLogType)logType);
				logEntity.F_Description += logType.ToDescription();
			}
			else
			{
				if (string.IsNullOrEmpty(keyValue))
				{
					logEntity = await CreateLog(className, DbLogType.Create);
					logEntity.F_Description += DbLogType.Create.ToDescription();
				}
				else
				{
					logEntity = await CreateLog(className, DbLogType.Update);
					logEntity.F_Description += DbLogType.Update.ToDescription();
				}
			}
			logEntity.F_KeyValue = keyValue;
			if (isError)
			{
				logEntity.F_Result = false;
				logEntity.F_Description += "操作失败，" + message;
			}
			else
			{
				logEntity.F_Description += message;
			}
			if (currentuser != null && currentuser.UserId != null)
			{
				logEntity.F_Account = currentuser.UserCode;
				logEntity.F_NickName = currentuser.UserName;
			}
			return logEntity;
		}
	}
}