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
using SqlSugar;
using WaterCloud.Domain.SystemManage;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Service.SystemSecurity
{
    public class LogService : DataFilterService<LogEntity>, IDenpendency
    {
        //登录信息保存方式
        private string HandleLogProvider = GlobalContext.SystemConfig.HandleLogProvider;
        public ModuleService moduleservice { get; set; }
        //获取类名

        public LogService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<List<LogEntity>> GetList(Pagination pagination, int timetype, string keyword="")
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
            if (HandleLogProvider != Define.CACHEPROVIDER_REDIS)
            {
                var query = repository.IQueryable();
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(a => a.Account.Contains(keyword) || a.Description.Contains(keyword) || a.ModuleName.Contains(keyword));
                }

                query = query.Where(a => a.Date >= startTime && a.Date <= endTime);
                result = await query.ToPageListAsync(pagination);
            }
            else
            {
                result = HandleLogHelper.HGetAll<LogEntity>(currentuser.CompanyId).Values.ToList();
                if (!string.IsNullOrEmpty(keyword))
                {
                    result = result.Where(a => a.Account.Contains(keyword) || a.Description.Contains(keyword) || a.ModuleName.Contains(keyword)).Where(a => a.Date >= startTime && a.Date <= endTime).ToList();
                }
                else
                {
                    result = result.Where(a => a.Date >= startTime && a.Date <= endTime).ToList();
                }
                pagination.records = result.Count();
                result = result.OrderByDescending(a => a.CreatorTime).Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows).ToList();

            }
            return GetFieldsFilterData(result);
        }
        public async Task<List<LogEntity>> GetList()
        {
            return await Task.Run(() => {
                if (HandleLogProvider != Define.CACHEPROVIDER_REDIS)
                {
                    return repository.IQueryable().ToList();
                }
                else
                {
                    return HandleLogHelper.HGetAll<LogEntity>(currentuser.CompanyId).Values.ToList(); ;
                }
            });           
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
            if (HandleLogProvider != Define.CACHEPROVIDER_REDIS)
            {
                var expression = ExtLinq.True<LogEntity>();
                expression = expression.AndAlso(a => a.Date <= operateTime);
                await repository.Delete(expression);
            }
            else
            {
                var list = HandleLogHelper.HGetAll<LogEntity>(currentuser.CompanyId).Values.ToList();
                var strList = list.Where(a => a.Date <= operateTime).Select(a => a.Id).ToList();
                await HandleLogHelper.HDelAsync(currentuser.CompanyId, strList.ToArray());
            }
        }
        public async Task WriteDbLog(bool result, string resultLog)
        {
            LogEntity logEntity = new LogEntity();
            logEntity.Id = Utils.GuId();
            logEntity.Date = DateTime.Now;
            logEntity.Account = currentuser.UserCode;
            logEntity.NickName = currentuser.UserName;
            logEntity.IPAddress = currentuser.LoginIPAddress;
            logEntity.IPAddressName = currentuser.LoginIPAddressName;
            logEntity.CompanyId = currentuser.CompanyId;
            logEntity.Result = result;
            logEntity.Description = resultLog;
            logEntity.Create();
            if (HandleLogProvider != Define.CACHEPROVIDER_REDIS)
            {
                await repository.Insert(logEntity);
            }
            else
            {
                await HandleLogHelper.HSetAsync(currentuser.CompanyId, logEntity.Id, logEntity);
            }
        }
        public async Task WriteDbLog(LogEntity logEntity)
        {
            logEntity.Id = Utils.GuId();
            logEntity.Date = DateTime.Now;
            var dbNumber = unitofwork.GetDbClient().CurrentConnectionConfig.ConfigId;
            unitOfWork.GetDbClient().ChangeDatabase(GlobalContext.SystemConfig.MainDbNumber);
            var systemSet = await unitOfWork.GetDbClient().Queryable<SystemSetEntity>().Where(a => a.DbNumber == "0").FirstAsync();
            unitOfWork.GetDbClient().ChangeDatabase(dbNumber);
            try
            {
                if (currentuser == null || string.IsNullOrEmpty(currentuser.UserId))
                {
                    logEntity.IPAddress = WebHelper.Ip;
					if (GlobalContext.SystemConfig.LocalLAN != false)
					{
                        logEntity.IPAddressName = "本地局域网";
                    }
					else
					{
                        logEntity.IPAddressName = WebHelper.GetIpLocation(logEntity.IPAddress);
                    }
                    logEntity.CompanyId = systemSet.Id;
                }
                else
                {
                    logEntity.IPAddress = currentuser.LoginIPAddress;
                    logEntity.IPAddressName = currentuser.LoginIPAddressName;
                    logEntity.CompanyId = currentuser.CompanyId;
                }
                logEntity.Create();
                if (HandleLogProvider != Define.CACHEPROVIDER_REDIS)
                {
                    unitofwork.Rollback();
                    unitofwork.CurrentRollback();
                    if (!string.IsNullOrEmpty(logEntity.KeyValue))
                    {
                        //批量删除时，循环拆分KeyValue，以免截断二进制错误
                        //方便以后根据KeyValue查询；
                        var keylist = logEntity.KeyValue.Split(",").ToList();
                        var loglist = new List<LogEntity>();
                        foreach (var key in keylist)
                        {
                            var log = new LogEntity();
                            log = logEntity.ToJson().ToObject<LogEntity>();
                            log.KeyValue = key;
                            log.Id = Utils.GuId();
                            loglist.Add(log);
                        }
                        await repository.Insert(loglist);
                    }
                    else
                    {
                        await repository.Insert(logEntity);
                    }
                }
                else
                {
                    await HandleLogHelper.HSetAsync(logEntity.CompanyId, logEntity.Id, logEntity);
                }
            }
            catch (Exception)
            {
                logEntity.IPAddress = WebHelper.Ip;
                if (GlobalContext.SystemConfig.LocalLAN != false)
                {
                    logEntity.IPAddressName = "本地局域网";
                }
                else
                {
                    logEntity.IPAddressName = WebHelper.GetIpLocation(logEntity.IPAddress);
                }
                logEntity.CompanyId = systemSet.Id;
                logEntity.Create();
                if (HandleLogProvider != Define.CACHEPROVIDER_REDIS)
                {
                    await repository.Insert(logEntity);
                }
                else
                {
                    await HandleLogHelper.HSetAsync(logEntity.CompanyId, logEntity.Id, logEntity);
                }
            }
        }

        public async Task<LogEntity> CreateLog(string className, DbLogType type)
        {
            try
            {
                var moduleitem = (await moduleservice.GetList()).Where(a => a.IsExpand == false && a.EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
                if (moduleitem==null)
                {
                    throw new Exception();
                }
                var module = (await moduleservice.GetList()).Where(a => a.Id == moduleitem.ParentId).First();
                return new LogEntity(await CreateModule(module), moduleitem == null ? "" : moduleitem.FullName, type.ToString());
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
                var moduleitem = (await moduleservice.GetList()).Where(a => a.IsExpand == false && a.EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
                if (moduleitem == null)
                {
                    throw new Exception();
                }
                var module = (await moduleservice.GetList()).Where(a => a.Id == moduleitem.ParentId).First();
                return new LogEntity(await CreateModule(module), moduleitem == null ? "" : moduleitem.FullName, type);

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
            str = module.FullName + "-" + str;
            if (module.ParentId=="0")
            {
                return str;
            }
            else
            {
                var temp= (await moduleservice.GetList()).Where(a =>a.Id==module.ParentId).First();
                return await CreateModule(temp ,str);
            }
        }
        public async Task WriteLog(string message, string className, string keyValue = "", DbLogType? logType = null, bool isError = false)
        {
            LogEntity logEntity;
            if (logType != null)
            {
                logEntity = await CreateLog(className, (DbLogType)logType);
                logEntity.Description += logType.ToDescription();
            }
            else
            {
                if (string.IsNullOrEmpty(keyValue))
                {
                    logEntity = await CreateLog(className, DbLogType.Create);
                    logEntity.Description += DbLogType.Create.ToDescription();
                }
                else
                {
                    logEntity = await CreateLog(className, DbLogType.Update);
                    logEntity.Description += DbLogType.Update.ToDescription();
                }
            }
            logEntity.KeyValue = keyValue;
            if (isError)
            {
                logEntity.Result = false;
                logEntity.Description += "操作失败，" + message;
            }
            else
            {
                logEntity.Description += message;
            }
            if (currentuser != null && currentuser.UserId != null)
            {
                logEntity.Account = currentuser.UserCode;
                logEntity.NickName = currentuser.UserName;
            }
            await WriteDbLog(logEntity);
        }
    }
}
