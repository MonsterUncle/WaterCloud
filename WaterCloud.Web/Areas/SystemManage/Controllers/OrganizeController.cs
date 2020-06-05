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
    public class OrganizeController : ControllerBase
    {
        private string moduleName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace.Split('.')[3];
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        private readonly OrganizeService _organizeService;
        private readonly LogService _logService;
        public OrganizeController(OrganizeService organizeService, LogService logService)
        {
            _organizeService = organizeService;
            _logService = logService;
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult AddForm()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeSelectJson()
        {
            var data =await _organizeService.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (OrganizeEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.parentId = item.F_ParentId;
                treeModel.data = item;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeJson()
        {
            var data =await _organizeService.GetList();
            var treeList = new List<TreeViewModel>();
            foreach (OrganizeEntity item in data)
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
        public async Task<ActionResult> GetTreeGridJson(string keyword)
        {
            var data =await _organizeService.GetLookList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_FullName.Contains(keyword));
            }
            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data =await _organizeService.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(OrganizeEntity organizeEntity, string keyValue)
        {
            LogEntity logEntity;
            if (string.IsNullOrEmpty(keyValue))
            {
                organizeEntity.F_AllowDelete = false;
                organizeEntity.F_AllowEdit = false;
                organizeEntity.F_DeleteMark = false;
                logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Create.ToString());
                logEntity.F_Description += DbLogType.Create.ToDescription();
            }
            else
            {
                logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Update.ToString());
                logEntity.F_Description += DbLogType.Update.ToDescription();
                logEntity.F_KeyValue = keyValue;
            }
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                if (organizeEntity.F_ParentId=="0")
                {
                    organizeEntity.F_Layers = 1;
                }
                else
                {
                    organizeEntity.F_Layers =(await _organizeService.GetForm(organizeEntity.F_ParentId)).F_Layers + 1;
                }
                await _organizeService.SubmitForm(organizeEntity, keyValue);
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
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            LogEntity logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Delete.ToString());
            logEntity.F_Description += DbLogType.Delete.ToDescription();
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                await _organizeService.DeleteForm(keyValue);
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
    }
}
