/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Application.SystemManage;
using WaterCloud.Code;
using WaterCloud.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WaterCloud.Entity.SystemSecurity;
using WaterCloud.Application;
using WaterCloud.Application.SystemSecurity;
using System;

namespace WaterCloud.Web.Areas.SystemManage.Controllers
{
    public class UserController : ControllerBase
    {
        private string moduleName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace.Split('.')[3];
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        private UserApp userApp = new UserApp();
        private UserLogOnApp userLogOnApp = new UserLogOnApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            pagination.order = "asc";
            pagination.sort = "F_DepartmentId";
            var data = userApp.GetList(pagination, keyword);
            return ResultLayUiTable(pagination.records, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = userApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetUserFormJson()
        {
            var data = userApp.GetForm(OperatorProvider.Provider.GetCurrent().UserId);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult SubmitUserForm(UserEntity userEntity)
        {
            var module = new ModuleApp().GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();
            var moduleitem = new ModuleApp().GetList().Where(a => a.F_Layers > 1 && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
            LogEntity logEntity= new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Update.ToString());
            logEntity.F_Description += DbLogType.Update.ToDescription();
            logEntity.F_KeyValue = userEntity.F_Id;
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                userEntity.F_Id = OperatorProvider.Provider.GetCurrent().UserId;
                userApp.SubmitUserForm(userEntity);
                logEntity.F_Description += "操作成功";
                new LogApp().WriteDbLog(logEntity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "操作失败，" + ex.Message;
                new LogApp().WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
            var module = new ModuleApp().GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();
            var moduleitem = new ModuleApp().GetList().Where(a => a.F_Layers > 1 && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
            LogEntity logEntity;
            if (string.IsNullOrEmpty(keyValue))
            {
                userEntity.F_DeleteMark = false;
                userEntity.F_IsBoss = false;
                logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Create.ToString());
                logEntity.F_Description += DbLogType.Create.ToDescription();
            }
            else
            {
                logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Update.ToString());
                logEntity.F_Description += DbLogType.Update.ToDescription();
                logEntity.F_KeyValue = keyValue;
                if (OperatorProvider.Provider.GetCurrent().UserId == keyValue)
                {
                    logEntity.F_Result = false;
                    logEntity.F_Description += "操作失败，不能修改用户自身";
                    new LogApp().WriteDbLog(logEntity);
                    return Error(logEntity.F_Description);
                }
            }
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                userApp.SubmitForm(userEntity, userLogOnEntity, keyValue);
                logEntity.F_Description += "操作成功";
                new LogApp().WriteDbLog(logEntity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "操作失败，" + ex.Message;
                new LogApp().WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            var module = new ModuleApp().GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();
            var moduleitem = new ModuleApp().GetList().Where(a => a.F_Layers > 1 && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
            LogEntity logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Delete.ToString());
            logEntity.F_Description += DbLogType.Delete.ToDescription();
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                if (OperatorProvider.Provider.GetCurrent().UserId == keyValue)
                {
                    logEntity.F_Result = false;
                    logEntity.F_Description += "操作失败，不能删除用户自身";
                    new LogApp().WriteDbLog(logEntity);
                    return Error(logEntity.F_Description);
                }
                userApp.DeleteForm(keyValue);
                logEntity.F_Description += "操作成功";
                new LogApp().WriteDbLog(logEntity);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "操作失败，" + ex.Message;
                new LogApp().WriteDbLog(logEntity);
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
        public ActionResult SubmitRevisePassword(string F_UserPassword, string keyValue)
        {
            var module = new ModuleApp().GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();
            var moduleitem = new ModuleApp().GetList().Where(a => a.F_Layers > 1 && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
            LogEntity logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Update.ToString());
            logEntity.F_Description += DbLogType.Update.ToDescription();
            logEntity.F_KeyValue = keyValue;
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                userLogOnApp.RevisePassword(F_UserPassword, keyValue);
                logEntity.F_Description += "重置密码成功";
                new LogApp().WriteDbLog(logEntity);
                return Success("重置密码成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "重置密码失败，" + ex.Message;
                new LogApp().WriteDbLog(logEntity);
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
        public ActionResult SubmitReviseSelfPassword(string F_UserPassword)
        {
            var module = new ModuleApp().GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();
            var moduleitem = new ModuleApp().GetList().Where(a => a.F_Layers > 1 && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
            LogEntity logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Update.ToString());
            logEntity.F_Description += DbLogType.Update.ToDescription();
            logEntity.F_KeyValue = OperatorProvider.Provider.GetCurrent().UserId;
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                userLogOnApp.ReviseSelfPassword(F_UserPassword, logEntity.F_KeyValue);
                logEntity.F_Description += "重置密码成功";
                new LogApp().WriteDbLog(logEntity);
                return Success("重置密码成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "重置密码失败，" + ex.Message;
                new LogApp().WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledAccount(string keyValue)
        {
            var module = new ModuleApp().GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();
            var moduleitem = new ModuleApp().GetList().Where(a => a.F_Layers > 1 && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
            LogEntity logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Update.ToString());
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
                    new LogApp().WriteDbLog(logEntity);
                    return Error(logEntity.F_Description);
                }
                userApp.UpdateForm(userEntity);
                logEntity.F_Description += "账户禁用成功";
                new LogApp().WriteDbLog(logEntity);
                return Success("账户禁用成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "账户禁用失败，" + ex.Message;
                new LogApp().WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EnabledAccount(string keyValue)
        {
            var module = new ModuleApp().GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();
            var moduleitem = new ModuleApp().GetList().Where(a => a.F_Layers > 1 && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
            LogEntity logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Update.ToString());
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
                    new LogApp().WriteDbLog(logEntity);
                    return Error(logEntity.F_Description);
                }
                userApp.UpdateForm(userEntity);
                logEntity.F_Description += "账户启用成功";
                new LogApp().WriteDbLog(logEntity);
                return Success("账户启用成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "账户启用失败，" + ex.Message;
                new LogApp().WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
    }
}
