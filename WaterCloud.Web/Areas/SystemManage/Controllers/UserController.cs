/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Service.SystemManage;
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Service;
using WaterCloud.Service.SystemSecurity;
using System;
using Senparc.CO2NET.Extensions;
using System.Threading.Tasks;

namespace WaterCloud.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class UserController : ControllerBase
    {
        private string moduleName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace.Split('.')[3];
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        private readonly UserService _userService;
        private readonly UserLogOnService _userLogOnService;
        private readonly ModuleService _moduleService;
        private readonly LogService _logService;
        public UserController(LogService logService, UserService userService, UserLogOnService userLogOnService, ModuleService moduleService)
        {
            _moduleService = moduleService;
            _userService = userService;
            _logService = logService;
            _userLogOnService = userLogOnService;
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(Pagination pagination, string keyword)
        {
            pagination.order = "asc";
            pagination.sort = "F_DepartmentId";
            var data =await _userService.GetLookList(pagination, keyword);
            return Success(pagination.records, data);
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult AddForm()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetListJson(string keyword)
        {
            var data = await _userService.GetList(keyword);
            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data =await _userService.GetLookForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetUserFormJson()
        {
            var data =await _userService.GetForm(OperatorProvider.Provider.GetCurrent().UserId);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> SubmitUserForm(UserEntity userEntity)
        {
            LogEntity logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Update.ToString());
            logEntity.F_Description += DbLogType.Update.ToDescription();
            logEntity.F_KeyValue = userEntity.F_Id;
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                userEntity.F_Id = OperatorProvider.Provider.GetCurrent().UserId;
                await _userService.SubmitUserForm(userEntity);
                logEntity.F_Description += "操作成功";
                await _logService.WriteDbLog(logEntity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "操作失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
            LogEntity logEntity;
            if (string.IsNullOrEmpty(keyValue))
            {
                userEntity.F_DeleteMark = false;
                userEntity.F_IsBoss = false;
                logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Create.ToString());
                logEntity.F_Description += DbLogType.Create.ToDescription();
            }
            else
            {
                logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Update.ToString());
                logEntity.F_Description += DbLogType.Update.ToDescription();
                logEntity.F_KeyValue = keyValue;
                if (OperatorProvider.Provider.GetCurrent().UserId == keyValue)
                {
                    logEntity.F_Result = false;
                    logEntity.F_Description += "操作失败，不能修改用户自身";
                    await _logService.WriteDbLog(logEntity);
                    return Error(logEntity.F_Description);
                }
            }
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                await _userService.SubmitForm(userEntity, userLogOnEntity, keyValue);
                logEntity.F_Description += "操作成功";
                await _logService.WriteDbLog(logEntity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "操作失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            LogEntity logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Delete.ToString());
            logEntity.F_Description += DbLogType.Delete.ToDescription();
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                if (OperatorProvider.Provider.GetCurrent().UserId == keyValue)
                {
                    logEntity.F_Result = false;
                    logEntity.F_Description += "操作失败，不能删除用户自身";
                    await _logService.WriteDbLog(logEntity);
                    return Error(logEntity.F_Description);
                }
                await _userService.DeleteForm(keyValue);
                logEntity.F_Description += "操作成功";
                await _logService.WriteDbLog(logEntity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "操作失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult RevisePassword()
        {
            return View();
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitRevisePassword(string F_UserPassword, string keyValue)
        {
            LogEntity logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Update.ToString());
            logEntity.F_Description += DbLogType.Update.ToDescription();
            logEntity.F_KeyValue = keyValue;
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                await _userLogOnService.RevisePassword(F_UserPassword, keyValue);
                logEntity.F_Description += "重置密码成功";
                await _logService.WriteDbLog(logEntity);
                return Success("重置密码成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "重置密码失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult ReviseSelfPassword()
        {
            return View();
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitReviseSelfPassword(string F_UserPassword)
        {
            LogEntity logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Update.ToString());
            logEntity.F_Description += DbLogType.Update.ToDescription();
            logEntity.F_KeyValue = OperatorProvider.Provider.GetCurrent().UserId;
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                await _userLogOnService.ReviseSelfPassword(F_UserPassword, logEntity.F_KeyValue);
                logEntity.F_Description += "重置密码成功";
                await _logService.WriteDbLog(logEntity);
                return Success("重置密码成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "重置密码失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisabledAccount(string keyValue)
        {
            LogEntity logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Update.ToString());
            logEntity.F_Description += DbLogType.Update.ToDescription();
            logEntity.F_KeyValue = keyValue;
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                UserEntity userEntity = new UserEntity();
                userEntity.F_Id = keyValue;
                userEntity.F_EnabledMark = false;
                if (OperatorProvider.Provider.GetCurrent().UserId == keyValue)
                {
                    logEntity.F_Result = false;
                    logEntity.F_Description += "操作失败，不能修改用户自身";
                    await _logService.WriteDbLog(logEntity);
                    return Error(logEntity.F_Description);
                }
                await _userService.UpdateForm(userEntity);
                logEntity.F_Description += "账户禁用成功";
                await _logService.WriteDbLog(logEntity);
                return Success("账户禁用成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "账户禁用失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnabledAccount(string keyValue)
        {
            LogEntity logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Update.ToString());
            logEntity.F_Description += DbLogType.Update.ToDescription();
            logEntity.F_KeyValue = keyValue;
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                UserEntity userEntity = new UserEntity();
                userEntity.F_Id = keyValue;
                userEntity.F_EnabledMark = true;
                if (OperatorProvider.Provider.GetCurrent().UserId == keyValue)
                {
                    logEntity.F_Result = false;
                    logEntity.F_Description += "操作失败，不能修改用户自身";
                    await _logService.WriteDbLog(logEntity);
                    return Error(logEntity.F_Description);
                }
                await _userService.UpdateForm(userEntity);
                logEntity.F_Description += "账户启用成功";
                await _logService.WriteDbLog(logEntity);
                return Success("账户启用成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "账户启用失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
    }
}
