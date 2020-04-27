/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Service.SystemManage;
using WaterCloud.Code;
using WaterCloud.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Entity.SystemSecurity;
using WaterCloud.Service;
using WaterCloud.Service.SystemSecurity;
using System;
using Senparc.CO2NET.Extensions;

namespace WaterCloud.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class RoleController : ControllerBase
    {
        private string moduleName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace.Split('.')[3];
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        private readonly RoleService _roleService;
        private readonly ModuleService _moduleService;
        private readonly LogService _logService;
        public RoleController(LogService logService,RoleService roleService, ModuleService moduleService)
        {
            _moduleService = moduleService;
            _roleService = roleService;
            _logService = logService;
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetListJson(string keyword)
        {
            var data = _roleService.GetList( keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            pagination.order = "asc";
            pagination.sort = "F_EnCode";
            var data = _roleService.GetList(pagination,keyword);
            return ResultLayUiTable(pagination.records,data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = _roleService.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(RoleEntity roleEntity, string permissionIds, string keyValue)
        {
            var module = _moduleService.GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();
            var moduleitem = _moduleService.GetList().Where(a => a.F_Layers > 1 && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
            LogEntity logEntity;
            if (string.IsNullOrEmpty(keyValue))
            {
                roleEntity.F_DeleteMark = false;
                roleEntity.F_AllowEdit = false;
                roleEntity.F_AllowDelete = false;
                logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Create.ToString());
                logEntity.F_Description += DbLogType.Create.ToDescription();
            }
            else
            {
                logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Update.ToString());
                logEntity.F_Description += DbLogType.Update.ToDescription();
                logEntity.F_KeyValue = keyValue;
                if (OperatorProvider.Provider.GetCurrent().RoleId == keyValue)
                {
                    logEntity.F_Result = false;
                    logEntity.F_Description += "操作失败，不能修改用户当前角色" ;
                    _logService.WriteDbLog(logEntity);
                    return Error(logEntity.F_Description);
                }
            }
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                _roleService.SubmitForm(roleEntity, permissionIds.Split(','), keyValue);
                logEntity.F_Description += "操作成功";
                _logService.WriteDbLog(logEntity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "操作失败，" + ex.Message;
                _logService.WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            var module = _moduleService.GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();
            var moduleitem = _moduleService.GetList().Where(a => a.F_Layers > 1 && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
            LogEntity logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Delete.ToString());
            logEntity.F_Description += DbLogType.Delete.ToDescription();
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                if (OperatorProvider.Provider.GetCurrent().RoleId == keyValue)
                {
                    logEntity.F_Result = false;
                    logEntity.F_Description += "操作失败，不能删除用户当前角色";
                    _logService.WriteDbLog(logEntity);
                    return Error(logEntity.F_Description);
                }
                _roleService.DeleteForm(keyValue);
                logEntity.F_Description += "操作成功";
                _logService.WriteDbLog(logEntity);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "操作失败，" + ex.Message;
                _logService.WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
    }
}
