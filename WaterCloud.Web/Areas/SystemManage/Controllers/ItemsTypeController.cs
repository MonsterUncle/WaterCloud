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
    public class ItemsTypeController : ControllerBase
    {
        private string moduleName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace.Split('.')[3];
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        private readonly ItemsService _itemsService;
        private readonly LogService _logService;
        private readonly ModuleService _moduleService;
        public ItemsTypeController(ItemsService itemsService, LogService logService, ModuleService moduleService)
        {
            _itemsService = itemsService;
            _logService = logService;
            _moduleService = moduleService;
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = _itemsService.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (ItemsEntity item in data)
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
        public ActionResult GetTreeJson()
        {
            var data = _itemsService.GetList();
            var treeList = new List<TreeViewModel>();
            foreach (ItemsEntity item in data)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                tree.id = item.F_Id;
                tree.text = item.F_FullName;
                tree.value = item.F_EnCode;
                tree.parentId = item.F_ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = _itemsService.GetList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_FullName.Contains(keyword));
            }
            var treeList = new List<TreeGridModel>();
            foreach (ItemsEntity item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                treeModel.id = item.F_Id;
                treeModel.title = item.F_FullName;
                treeModel.parentId = item.F_ParentId;
                //treeModel.self = item;
                treeList.Add(treeModel);
            }
            return ResultDTree(treeList.TreeList());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = _itemsService.GetList();
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
            var data = _itemsService.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ItemsEntity itemsEntity, string keyValue)
        {
            var module = _moduleService.GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();            
            LogEntity logEntity;
            if (string.IsNullOrEmpty(keyValue))
            {
                itemsEntity.F_DeleteMark = false;
                itemsEntity.F_IsTree = false;
                logEntity = new LogEntity(module.F_FullName,"字典分类", DbLogType.Create.ToString());
                logEntity.F_Description += DbLogType.Create.ToDescription();
            }
            else
            {
                logEntity = new LogEntity(module.F_FullName, "字典分类", DbLogType.Update.ToString());
                logEntity.F_Description += DbLogType.Update.ToDescription();
                logEntity.F_KeyValue = keyValue;
            }
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                if (string.IsNullOrEmpty(keyValue))
                {

                }
                if (itemsEntity.F_ParentId == "0")
                {
                    itemsEntity.F_Layers = 1;
                }
                else
                {
                    itemsEntity.F_Layers = _itemsService.GetForm(itemsEntity.F_ParentId).F_Layers + 1;
                }
                _itemsService.SubmitForm(itemsEntity, keyValue);
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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            var module = _moduleService.GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();
            LogEntity logEntity = new LogEntity(module.F_FullName, "字典分类", DbLogType.Delete.ToString());
            logEntity.F_Description += DbLogType.Delete.ToDescription();
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                _itemsService.DeleteForm(keyValue);
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
