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
    public class ModuleController : ControllerBase
    {
        private string moduleName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace.Split('.')[3];
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        private ModuleApp moduleApp = new ModuleApp();

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult FontIcons()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = moduleApp.GetList();
            data = data.Where(a => a.F_Target == "expand" && a.F_IsExpand == true).ToList();
            var treeList = new List<TreeSelectModel>();
            foreach (ModuleEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = moduleApp.GetList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_FullName.Contains(keyword));
            }
            //var treeList = new List<TreeGridModel2>();
            //foreach (ModuleEntity item in data)
            //{
            //    TreeGridModel2 treeModel = new TreeGridModel2();
            //    treeModel.id = item.F_Id;
            //    treeModel.parentId = item.F_ParentId;
            //    treeModel.self = item;
            //    treeList.Add(treeModel);
            //}
            return ResultLayUiTable(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = moduleApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ModuleEntity moduleEntity, string keyValue)
        {
            var module = new ModuleApp().GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();
            var moduleitem = new ModuleApp().GetList().Where(a => a.F_Layers > 1 && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
            LogEntity logEntity;
            if (string.IsNullOrEmpty(keyValue))
            {
                moduleEntity.F_DeleteMark = false;
                moduleEntity.F_AllowEdit = false;
                moduleEntity.F_AllowDelete = false;
                logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Create.ToString());
                logEntity.F_Description += DbLogType.Create.ToDescription();
            }
            else
            {
                //前端传值为null，更新的时候null不更新
                if (moduleEntity.F_Icon==null)
                {
                    moduleEntity.F_Icon = "";
                }
                logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Update.ToString());
                logEntity.F_Description += DbLogType.Update.ToDescription();
                logEntity.F_KeyValue = keyValue;
            }
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                if (moduleEntity.F_ParentId == "0")
                {
                    moduleEntity.F_Layers = 1;
                }
                else
                {
                    moduleEntity.F_Layers = moduleApp.GetForm(moduleEntity.F_ParentId).F_Layers + 1;
                }
                if (!string.IsNullOrEmpty(moduleEntity.F_UrlAddress))
                {
                    var templist = moduleApp.GetList();
                    if (!string.IsNullOrEmpty(keyValue))
                    {
                        templist = templist.Where(a => a.F_Id != keyValue).ToList();
                    }
                    if (templist.Find(a => a.F_UrlAddress == moduleEntity.F_UrlAddress) != null)
                        throw new Exception("菜单地址不能重复！");
                }
                else
                {
                    moduleEntity.F_UrlAddress = null;
                }
                moduleApp.SubmitForm(moduleEntity, keyValue);
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
        [HandlerAuthorize]
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
                moduleApp.DeleteForm(keyValue);
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
    }
}
