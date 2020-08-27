/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Service.SystemOrganize;
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Service;
using WaterCloud.Service.SystemSecurity;
using System;
using Serenity;
using System.Threading.Tasks;
using WaterCloud.Service.SystemManage;

namespace WaterCloud.Web.Areas.SystemOrganize.Controllers
{
    [Area("SystemOrganize")]
    public class UserController : ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public UserService _service { get; set; }
        public UserLogOnService _userLogOnService { get; set; }
        public ModuleService _moduleService { get; set; }
        public LogService _logService { get; set; }
        public RoleService _roleService { get; set; }
        public OrganizeService _orgService { get; set; }

        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(Pagination pagination, string keyword)
        {
            pagination.order = "asc";
            pagination.sort = "F_DepartmentId asc";
            //导出全部页使用
            if (pagination.rows == 0 && pagination.page == 0)
            {
                pagination.rows = 99999999;
                pagination.page = 1;
            }
            var data =await _service.GetLookList(pagination, keyword);
            return Success(pagination.records, data);
        }
        [HttpGet]
        public virtual ActionResult AddForm()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetListJson(string keyword,string ids)
        {
            var data = await _service.GetList(keyword);
            data = data.Where(a => a.F_EnabledMark == true).ToList();
            if (!string.IsNullOrEmpty(ids))
            {
                foreach (var item in ids.Split(','))
                {
                    var temp = data.Find(a => a.F_Id == item);
                    if (temp != null)
                    {
                        temp.LAY_CHECKED = true;
                    }
                }
            }
            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data =await _service.GetLookForm(keyValue);
            if (!string.IsNullOrEmpty(data.F_DepartmentId))
            {
                List<string> str = new List<string>();
                foreach (var item in data.F_DepartmentId.Split(','))
                {
                    var temp = await _orgService.GetForm(item);
                    if (temp != null)
                    {
                        str.Add(temp.F_FullName);
                    }
                }
                data.F_DepartmentName = string.Join("  ", str.ToArray());
            }
            if (!string.IsNullOrEmpty(data.F_RoleId))
            {
                List<string> str = new List<string>();
                foreach (var item in data.F_RoleId.Split(','))
                {
                    var temp = await _roleService.GetForm(item);
                    if (temp != null)
                    {
                        str.Add(temp.F_FullName);
                    }
                }
                data.F_RoleName = string.Join("  ", str.ToArray());
            }
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetUserFormJson()
        {
            var data =await _service.GetForm(_service.currentuser.UserId);
            if (!string.IsNullOrEmpty(data.F_DepartmentId))
            {
                List<string> str = new List<string>();
                foreach (var item in data.F_DepartmentId.Split(','))
                {
                    var temp = await _orgService.GetForm(item);
                    if (temp != null)
                    {
                        str.Add(temp.F_FullName);
                    }
                }
                data.F_DepartmentName = string.Join("  ", str.ToArray());
            }
            if (!string.IsNullOrEmpty(data.F_RoleId))
            {
                List<string> str = new List<string>();
                foreach (var item in data.F_RoleId.Split(','))
                {
                    var temp = await _roleService.GetForm(item);
                    if (temp != null)
                    {
                        str.Add(temp.F_FullName);
                    }
                }
                data.F_RoleName = string.Join("  ", str.ToArray());
            }
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> SubmitUserForm(UserEntity userEntity)
        {
            LogEntity logEntity = await _logService.CreateLog(className, DbLogType.Update.ToString());
            logEntity.F_Description += DbLogType.Update.ToDescription();
            logEntity.F_KeyValue = userEntity.F_Id;
            try
            {
                logEntity.F_Account = _service.currentuser.UserCode;
                logEntity.F_NickName = _service.currentuser.UserName;
                userEntity.F_Id = _service.currentuser.UserId;
                await _service.SubmitUserForm(userEntity);
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
                userEntity.F_IsAdmin = false;
                userEntity.F_DeleteMark = false;
                userEntity.F_IsBoss = false;
                userEntity.F_OrganizeId = _service.currentuser.CompanyId;
                logEntity = await _logService.CreateLog(className, DbLogType.Create.ToString());
                logEntity.F_Description += DbLogType.Create.ToDescription();
            }
            else
            {
                logEntity = await _logService.CreateLog(className, DbLogType.Update.ToString());
                logEntity.F_Description += DbLogType.Update.ToDescription();
                logEntity.F_KeyValue = keyValue;
                if (_service.currentuser.UserId == keyValue)
                {
                    logEntity.F_Result = false;
                    logEntity.F_Description += "操作失败，不能修改用户自身";
                    await _logService.WriteDbLog(logEntity);
                    return Error(logEntity.F_Description);
                }
            }
            try
            {
                logEntity.F_Account = _service.currentuser.UserCode;
                logEntity.F_NickName = _service.currentuser.UserName;
                await _service.SubmitForm(userEntity, userLogOnEntity, keyValue);
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
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            LogEntity logEntity = await _logService.CreateLog(className, DbLogType.Delete.ToString());
            logEntity.F_Description += DbLogType.Delete.ToDescription();
            try
            {
                logEntity.F_Account = _service.currentuser.UserCode;
                logEntity.F_NickName = _service.currentuser.UserName;
                if (_service.currentuser.UserId == keyValue)
                {
                    logEntity.F_Result = false;
                    logEntity.F_Description += "操作失败，不能删除用户自身";
                    await _logService.WriteDbLog(logEntity);
                    return Error(logEntity.F_Description);
                }
                await _service.DeleteForm(keyValue);
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
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitRevisePassword(string F_UserPassword, string keyValue)
        {
            LogEntity logEntity = await _logService.CreateLog(className, DbLogType.Update.ToString());
            logEntity.F_Description += DbLogType.Update.ToDescription();
            logEntity.F_KeyValue = keyValue;
            try
            {
                logEntity.F_Account = _service.currentuser.UserCode;
                logEntity.F_NickName = _service.currentuser.UserName;
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
            LogEntity logEntity = await _logService.CreateLog(className, DbLogType.Update.ToString());
            logEntity.F_Description += DbLogType.Update.ToDescription();
            logEntity.F_KeyValue = _service.currentuser.UserId;
            try
            {
                logEntity.F_Account = _service.currentuser.UserCode;
                logEntity.F_NickName = _service.currentuser.UserName;
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
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisabledAccount(string keyValue)
        {
            LogEntity logEntity = await _logService.CreateLog(className, DbLogType.Update.ToString());
            logEntity.F_Description += DbLogType.Update.ToDescription();
            logEntity.F_KeyValue = keyValue;
            try
            {
                logEntity.F_Account = _service.currentuser.UserCode;
                logEntity.F_NickName = _service.currentuser.UserName;
                UserEntity userEntity = new UserEntity();
                userEntity.F_Id = keyValue;
                userEntity.F_EnabledMark = false;
                if (_service.currentuser.UserId == keyValue)
                {
                    logEntity.F_Result = false;
                    logEntity.F_Description += "操作失败，不能修改用户自身";
                    await _logService.WriteDbLog(logEntity);
                    return Error(logEntity.F_Description);
                }
                await _service.UpdateForm(userEntity);
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
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnabledAccount(string keyValue)
        {
            LogEntity logEntity = await _logService.CreateLog(className, DbLogType.Update.ToString());
            logEntity.F_Description += DbLogType.Update.ToDescription();
            logEntity.F_KeyValue = keyValue;
            try
            {
                logEntity.F_Account = _service.currentuser.UserCode;
                logEntity.F_NickName = _service.currentuser.UserName;
                UserEntity userEntity = new UserEntity();
                userEntity.F_Id = keyValue;
                userEntity.F_EnabledMark = true;
                if (_service.currentuser.UserId == keyValue)
                {
                    logEntity.F_Result = false;
                    logEntity.F_Description += "操作失败，不能修改用户自身";
                    await _logService.WriteDbLog(logEntity);
                    return Error(logEntity.F_Description);
                }
                await _service.UpdateForm(userEntity);
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
