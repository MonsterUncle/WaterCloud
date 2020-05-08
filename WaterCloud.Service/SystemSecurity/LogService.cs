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

namespace WaterCloud.Service.SystemSecurity
{
    public class LogService: IDenpendency
    {
        private ILogRepository service = new LogRepository();

        public List<LogEntity> GetList(Pagination pagination, int timetype, string keyword="")
        {
            var expression = ExtLinq.True<LogEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_Account.Contains(keyword));
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
            expression = expression.And(t => t.F_Date >= startTime && t.F_Date <= endTime);
            return service.FindList(expression, pagination);
        }
        public List<LogEntity> GetList()
        {           
            return service.IQueryable().ToList();
        }
        public void RemoveLog(string keepTime)
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
            service.Delete(expression);
        }
        public void WriteDbLog(bool result, string resultLog)
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
            service.Insert(logEntity);
        }
        public void WriteDbLog(LogEntity logEntity)
        {
            logEntity.F_Id = Utils.GuId();
            logEntity.F_Date = DateTime.Now;
            try
            {

                var operatorModel = OperatorProvider.Provider.GetCurrent();
                if (operatorModel==null)
                {
                    logEntity.F_IPAddress = NetHelper.Ip;
                    logEntity.F_IPAddressName = "本地局域网";
                }
                else
                {
                    logEntity.F_IPAddress = OperatorProvider.Provider.GetCurrent().LoginIPAddress;
                    logEntity.F_IPAddressName = OperatorProvider.Provider.GetCurrent().LoginIPAddressName;
                }
                logEntity.Create();
                service.Insert(logEntity);
            }
            catch (Exception)
            {
                logEntity.F_IPAddress = NetHelper.Ip;
                logEntity.F_IPAddressName = "本地局域网";
                logEntity.Create();
                service.Insert(logEntity);
            }
        }
    }
}
