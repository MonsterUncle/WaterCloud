/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemSecurity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaterCloud.Service.SystemManage;
using System.Linq;
using Chloe;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.Service.SystemSecurity
{
    public class LogService : DataFilterService<LogEntity>, IDenpendency
    {
        //登录信息保存方式
        private string LoginProvider = GlobalContext.SystemConfig.LoginProvider;
        private ModuleService moduleservice;
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public LogService(IDbContext context) : base(context)
        {
            moduleservice = new ModuleService(context);
        }
        public async Task<List<LogEntity>> GetLookList(Pagination pagination, int timetype, string keyword="")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(u => u.F_Account.Contains(keyword) || u.F_Description.Contains(keyword) || u.F_ModuleName.Contains(keyword));
            }
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
            list = list.Where(t => t.F_Date >= startTime && t.F_Date <= endTime);
            return GetFieldsFilterData(await repository.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }
        public async Task<List<LogEntity>> GetList()
        {           
            return repository.IQueryable().ToList();
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
            expression = expression.And(t => t.F_Date <= operateTime);
            await repository.Delete(expression);
        }
        public async Task WriteDbLog(bool result, string resultLog)
        {
            LogEntity logEntity = new LogEntity();
            logEntity.F_Id = Utils.GuId();
            logEntity.F_Date = DateTime.Now;
            logEntity.F_Account = currentuser.UserCode;
            logEntity.F_NickName = currentuser.UserName;
            logEntity.F_IPAddress = currentuser.LoginIPAddress;
            logEntity.F_IPAddressName = currentuser.LoginIPAddressName;
            logEntity.F_CompanyId = currentuser.CompanyId;
            logEntity.F_Result = result;
            logEntity.F_Description = resultLog;
            logEntity.Create();
            await repository.Insert(logEntity);
        }
        public async Task WriteDbLog(LogEntity logEntity)
        {
            logEntity.F_Id = Utils.GuId();
            logEntity.F_Date = DateTime.Now;
            try
            {
                if (currentuser == null || string.IsNullOrEmpty(currentuser.UserId))
                {
                    logEntity.F_IPAddress = LoginProvider=="WebApi"? "未连接未知": WebHelper.Ip;
                    logEntity.F_IPAddressName = "本地局域网";
                    logEntity.F_CompanyId = Define.SYSTEM_MASTERPROJECT;
                }
                else
                {
                    logEntity.F_IPAddress = currentuser.LoginIPAddress;
                    logEntity.F_IPAddressName = currentuser.LoginIPAddressName;
                    logEntity.F_CompanyId = currentuser.CompanyId;
                }
                logEntity.Create();
                await repository.Insert(logEntity);
            }
            catch (Exception)
            {
                logEntity.F_IPAddress = LoginProvider == "WebApi" ? "未连接未知" : WebHelper.Ip;
                logEntity.F_IPAddressName = "本地局域网";
                logEntity.F_CompanyId = Define.SYSTEM_MASTERPROJECT;
                logEntity.Create();
                await repository.Insert(logEntity);
            }
        }

        public async Task<LogEntity> CreateLog(string className, DbLogType type)
        {
            try
            {
                var moduleitem = (await moduleservice.GetList()).Where(a => a.F_IsExpand == false && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
                var module = (await moduleservice.GetList()).Where(a => a.F_Id == moduleitem.F_ParentId).FirstOrDefault();
                return new LogEntity(await CreateModule(module), moduleitem == null ? "" : moduleitem.F_FullName, type.ToString());
            }
            catch (Exception)
            {
                return new LogEntity(className, "" , type.ToString());
            }
        }
        public async Task<LogEntity> CreateLog(string className, string type)
        {
            try
            {
                var moduleitem = (await moduleservice.GetList()).Where(a => a.F_IsExpand == false && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
                var module = (await moduleservice.GetList()).Where(a => a.F_Id == moduleitem.F_ParentId).FirstOrDefault();
                return new LogEntity(await CreateModule(module), moduleitem == null ? "" : moduleitem.F_FullName, type);

            }
            catch (Exception)
            {
                return new LogEntity(className, "", type.ToString());
            }
        }
        public async Task<string> CreateModule(ModuleEntity module, string str="")
        {
            if (module==null)
            {
                return str;
            }
            str = module.F_FullName + "-" + str;
            if (module.F_ParentId=="0")
            {
                return str;
            }
            else
            {
                var temp= (await moduleservice.GetList()).Where(a =>a.F_Id==module.F_ParentId).FirstOrDefault();
                return await CreateModule(temp ,str);
            }
        }
        public async Task WriteLog(string message, string className, string keyValue = "", DbLogType? logType = null, bool isError = false)
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
            await WriteDbLog(logEntity);
        }
    }
}
