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

namespace WaterCloud.Service.SystemSecurity
{
    public class LogService : DataFilterService<LogEntity>, IDenpendency
    {
        //登录信息保存方式
        private string LoginProvider = GlobalContext.SystemConfig.LoginProvider;
        private ILogRepository service = new LogRepository();
        private ModuleService moduleservice = new ModuleService();
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
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
            return await service.OrderList(list, pagination);
        }
        public async Task<List<LogEntity>> GetList()
        {           
            return service.IQueryable().ToList();
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
            await service.Delete(expression);
        }
        public async Task WriteDbLog(bool result, string resultLog)
        {
            LogEntity logEntity = new LogEntity();
            logEntity.F_Id = Utils.GuId();
            logEntity.F_Date = DateTime.Now;
            logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
            logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
            logEntity.F_IPAddress = OperatorProvider.Provider.GetCurrent().LoginIPAddress;
            logEntity.F_IPAddressName = OperatorProvider.Provider.GetCurrent().LoginIPAddressName;
            logEntity.F_Result = result;
            logEntity.F_Description = resultLog;
            logEntity.Create();
            await service.Insert(logEntity);
        }
        public async Task WriteDbLog(LogEntity logEntity)
        {
            logEntity.F_Id = Utils.GuId();
            logEntity.F_Date = DateTime.Now;
            try
            {

                var operatorModel = OperatorProvider.Provider.GetCurrent();
                if (operatorModel==null)
                {
                    logEntity.F_IPAddress = LoginProvider=="WebApi"? "未连接未知": WebHelper.Ip;
                    logEntity.F_IPAddressName = "本地局域网";
                }
                else
                {
                    logEntity.F_IPAddress = OperatorProvider.Provider.GetCurrent().LoginIPAddress;
                    logEntity.F_IPAddressName = OperatorProvider.Provider.GetCurrent().LoginIPAddressName;
                }
                logEntity.Create();
                await service.Insert(logEntity);
            }
            catch (Exception)
            {
                logEntity.F_IPAddress = LoginProvider == "WebApi" ? "未连接未知" : WebHelper.Ip;
                logEntity.F_IPAddressName = "本地局域网";
                logEntity.Create();
                await service.Insert(logEntity);
            }
        }

        public async Task<LogEntity> CreateLog(string moduleName, string className, string type)
        {
            var module = (await moduleservice.GetList()).Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();
            var moduleitem = (await moduleservice.GetList()).Where(a => a.F_Layers > 1 && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
            return new LogEntity(module.F_FullName, moduleitem.F_FullName, type);
        }
    }
}
