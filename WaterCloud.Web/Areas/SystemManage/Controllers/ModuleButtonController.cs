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
using System.Threading.Tasks;
using Serenity;

namespace WaterCloud.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class ModuleButtonController : ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        private readonly ModuleService _moduleService;
        private readonly ModuleButtonService _moduleButtonService;       
        private readonly LogService _logService;
        public ModuleButtonController(ModuleButtonService moduleButtonService, LogService logService, ModuleService moduleService)
        {
            _moduleButtonService = moduleButtonService;
            _logService = logService;
            _moduleService = moduleService;
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeSelectJson(string moduleId)
        {
            var data =await _moduleButtonService.GetList(moduleId);
            var treeList = new List<TreeSelectModel>();
            foreach (ModuleButtonEntity item in data)
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
        public async Task<ActionResult> GetTreeGridJson(string moduleId)
        {
            var data =await _moduleButtonService.GetLookList(moduleId);
            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data =await _moduleButtonService.GetLookForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ServiceFilter(typeof(HandlerAdminAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(ModuleButtonEntity moduleButtonEntity, string keyValue)
        {
            LogEntity logEntity;
            if (string.IsNullOrEmpty(keyValue))
            {
                moduleButtonEntity.F_DeleteMark = false;
                moduleButtonEntity.F_AllowEdit = false;
                moduleButtonEntity.F_AllowDelete = false;
                logEntity = await _logService.CreateLog(className, DbLogType.Create.ToString());
                logEntity.F_Description += DbLogType.Create.ToDescription();
            }
            else
            {
                logEntity = await _logService.CreateLog(className, DbLogType.Update.ToString());
                logEntity.F_Description += DbLogType.Update.ToDescription();
                logEntity.F_KeyValue = keyValue;
            }
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                if (moduleButtonEntity.F_ParentId == "0")
                {
                    moduleButtonEntity.F_Layers = 1;
                }
                else
                {
                    moduleButtonEntity.F_Layers =(await _moduleButtonService.GetForm(moduleButtonEntity.F_ParentId)).F_Layers + 1;
                }
                await _moduleButtonService.SubmitForm(moduleButtonEntity, keyValue);
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
        [ServiceFilter(typeof(HandlerAdminAttribute))]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            LogEntity logEntity = await _logService.CreateLog(className, DbLogType.Delete.ToString()); 
            logEntity.F_Description += DbLogType.Delete.ToDescription();
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                await _moduleButtonService.DeleteForm(keyValue);
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
        public ActionResult CloneButton()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetCloneButtonTreeJson()
        {
            var moduledata =await _moduleService.GetList();
            var buttondata =await _moduleButtonService.GetList();
            var treeList = new List<TreeGridModel>();
            foreach (ModuleEntity item in moduledata)
            {
                TreeGridModel treeModel = new TreeGridModel();
                treeModel.id = item.F_Id;
                treeModel.title = item.F_FullName;
                treeModel.parentId = item.F_ParentId;
                treeModel.checkArr = "0";
                treeModel.disabled = true;
                //treeModel.self = item;
                treeList.Add(treeModel);
            }
            foreach (ModuleButtonEntity item in buttondata)
            {
                TreeGridModel treeModel = new TreeGridModel();
                treeModel.id = item.F_Id;
                treeModel.title = item.F_FullName;
                treeModel.parentId = item.F_ParentId == "0" ? item.F_ModuleId : item.F_ParentId;
                treeModel.checkArr = "0";
                //treeModel.self = item;
                treeList.Add(treeModel);
            }
            return ResultDTree(treeList.TreeList());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> SubmitCloneButton(string moduleId, string Ids)
        {
            LogEntity logEntity = await _logService.CreateLog(className, DbLogType.Create.ToString());
            logEntity.F_Description += DbLogType.Create.ToDescription();
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                await _moduleButtonService.SubmitCloneButton(moduleId, Ids);
                logEntity.F_Description += "克隆成功";
                await _logService.WriteDbLog(logEntity);
                return Success("克隆成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "克隆失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
    }
}
