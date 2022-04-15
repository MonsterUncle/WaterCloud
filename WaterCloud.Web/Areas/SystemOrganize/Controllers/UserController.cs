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
using WaterCloud.Service;
using System;
using System.Threading.Tasks;
using WaterCloud.Service.SystemManage;

namespace WaterCloud.Web.Areas.SystemOrganize.Controllers
{
    [Area("SystemOrganize")]
    public class UserController : ControllerBase
    {

        public UserService _service { get; set; }
        public UserLogOnService _userLogOnService { get; set; }
        public ModuleService _moduleService { get; set; }
        public RoleService _roleService { get; set; }
        public OrganizeService _orgService { get; set; }
        [HandlerAjaxOnly]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> GetGridJson(SoulPage<UserExtend> pagination, string keyword)
        {
            if (string.IsNullOrEmpty(pagination.field))
            {
                pagination.field = "DepartmentId";
                pagination.order = "asc";
            }
            var data = await _service.GetLookList(pagination, keyword);
            return Content(pagination.setData(data).ToJson());
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
            data = data.Where(a => a.EnabledMark == true).ToList();
            if (!string.IsNullOrEmpty(ids))
            {
                foreach (var item in ids.Split(','))
                {
                    var temp = data.Find(a => a.Id == item);
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
            if (!string.IsNullOrEmpty(data.DepartmentId))
            {
                List<string> str = new List<string>();
                foreach (var item in data.DepartmentId.Split(','))
                {
                    var temp = await _orgService.GetForm(item);
                    if (temp != null)
                    {
                        str.Add(temp.FullName);
                    }
                }
                data.DepartmentName = string.Join("  ", str.ToArray());
            }
            if (!string.IsNullOrEmpty(data.RoleId))
            {
                List<string> str = new List<string>();
                foreach (var item in data.RoleId.Split(','))
                {
                    var temp = await _roleService.GetForm(item);
                    if (temp != null)
                    {
                        str.Add(temp.FullName);
                    }
                }
                data.RoleName = string.Join("  ", str.ToArray());
            }
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetUserFormJson()
        {
            var data =await _service.GetFormExtend(_service.currentuser.UserId);
            if (!string.IsNullOrEmpty(data.DepartmentId))
            {
                List<string> str = new List<string>();
                foreach (var item in data.DepartmentId.Split(','))
                {
                    var temp = await _orgService.GetForm(item);
                    if (temp != null)
                    {
                        str.Add(temp.FullName);
                    }
                }
                data.DepartmentName = string.Join("  ", str.ToArray());
            }
            if (!string.IsNullOrEmpty(data.RoleId))
            {
                List<string> str = new List<string>();
                foreach (var item in data.RoleId.Split(','))
                {
                    var temp = await _roleService.GetForm(item);
                    if (temp != null)
                    {
                        str.Add(temp.FullName);
                    }
                }
                data.RoleName = string.Join("  ", str.ToArray());
            }
            return Content(data.ToJson("yyyy-MM-dd"));
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerLock]
        public async Task<ActionResult> SubmitUserForm(string Account,string RealName, bool Gender,DateTime Birthday,string MobilePhone,string Email,string Description)
        {
            try
            {
                var userEntity = new UserEntity();
                userEntity.Account = Account;
                userEntity.RealName = RealName;
                userEntity.Gender = Gender;
                userEntity.Birthday = Birthday;
                userEntity.MobilePhone = MobilePhone;
                userEntity.Email = Email;
                userEntity.Description = Description;
                userEntity.Id = _service.currentuser.UserId;
                await _service.SubmitUserForm(userEntity);
                return await Success("操作成功。", "", userEntity.Id);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, "", _service.currentuser.UserId);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                userEntity.IsAdmin = false;
                userEntity.DeleteMark = false;
                userEntity.IsBoss = false;
                userEntity.OrganizeId = _service.currentuser.CompanyId;
            }
            else
            {
                if (_service.currentuser.UserId == keyValue)
                {
                    return Error("操作失败，不能修改用户自身");
                }
            }
            try
            {
                await _service.SubmitForm(userEntity, userLogOnEntity, keyValue);
                return await Success("操作成功。", "", keyValue);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, "", keyValue);
            }
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            try
            {
                if (_service.currentuser.UserId == keyValue)
                {
                    return Error("操作失败，不能删除用户自身");
                }
                await _service.DeleteForm(keyValue);
                return await Success("操作成功。", "", keyValue, DbLogType.Delete);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, "", keyValue, DbLogType.Delete);
            }
        }
        [HttpGet]
        public ActionResult RevisePassword()
        {
            return View();
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> SubmitRevisePassword(string UserPassword, string keyValue)
        {
            try
            {
                await _userLogOnService.RevisePassword(UserPassword, keyValue);
                return await Success("重置密码成功。", "", keyValue);
            }
            catch (Exception ex)
            {
                return await Error("重置密码失败," + ex.Message, "", keyValue);
            }
        }
        [HttpGet]
        public ActionResult ReviseSelfPassword()
        {
            return View();
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> SubmitReviseSelfPassword(string UserPassword)
        {
            try
            {
                await _userLogOnService.ReviseSelfPassword(UserPassword, _service.currentuser.UserId);
                return await Success("重置密码成功。", "", _service.currentuser.UserId);
            }
            catch (Exception ex)
            {
                return await Error("重置密码失败," + ex.Message, "", _service.currentuser.UserId);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        public async Task<ActionResult> DisabledAccount(string keyValue)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.Id = keyValue;
                userEntity.EnabledMark = false;
                if (_service.currentuser.UserId == keyValue)
                {
                    return Error("操作失败，不能修改用户自身");
                }
                await _service.UpdateForm(userEntity);
                return await Success("账户禁用成功。", "", keyValue);
            }
            catch (Exception ex)
            {
                return await Error("账户禁用失败," + ex.Message, "", keyValue);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        public async Task<ActionResult> EnabledAccount(string keyValue)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.Id = keyValue;
                userEntity.EnabledMark = true;
                if (_service.currentuser.UserId == keyValue)
                {
                    return Error("操作失败，不能修改用户自身");
                }
                await _service.UpdateForm(userEntity);
                return await Success("账户启用成功。", "", keyValue);
            }
            catch (Exception ex)
            {
                return await Error("账户启用失败,"+ex.Message, "", keyValue);
            }
        }
    }
}
