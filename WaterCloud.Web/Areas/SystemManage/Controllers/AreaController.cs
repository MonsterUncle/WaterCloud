/*******************************************************************************
 * Copyright © 2018 WaterCloud 版权所有
 * Author: WaterCloud
 * Description: WaterCloud
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Service.SystemManage;
using WaterCloud.Service.SystemSecurity;
using WaterCloud.Service;
using WaterCloud.Domain.SystemSecurity;
using System.Threading.Tasks;
using Serenity;

namespace WaterCloud.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class AreaController : ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public AreaService _areaService { get; set; }
        public LogService _logService { get; set; }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeSelectJson()
        {
            var data =await _areaService.GetList();
            //默认三级区域
            data = data.Where(a => a.F_Layers < 3).ToList();
            var treeList = new List<TreeSelectModel>();
            foreach (AreaEntity item in data)
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
        public async Task<ActionResult> GetSelectJson(string keyValue)
        {
            var data = await _areaService.GetList();
            data = data.Where(a => a.F_ParentId== keyValue).ToList();
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeGridJson(string keyword)
        {
            var data =await _areaService.GetLookList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_FullName.Contains(keyword));
            }
            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetListJson(string keyValue, string keyword)
        {
            var data = await _areaService.GetLookList();
            var result = new List<AreaEntity>();
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = "0";
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                result = data.TreeWhere(t => t.F_FullName.Contains(keyword) || t.F_EnCode.Contains(keyword));
            }
            else
            {
                result = data;
            }
            result = result.Where(t => t.F_ParentId == keyValue).ToList();
            if (result.Count==0)
            {
                result= data.Where(t => t.F_ParentId == keyValue).ToList();
            }
            foreach (var item in result)
            {
                item.haveChild = data.Where(a => a.F_ParentId == item.F_Id).Count() > 0 ? true : false;
            }
            return Success(data.Count, result);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data =await _areaService.GetLookForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(AreaEntity areaEntity, string keyValue)
        {
            LogEntity logEntity;
            if (string.IsNullOrEmpty(keyValue))
            {
                areaEntity.F_DeleteMark = false;
                logEntity = await _logService.CreateLog(className, DbLogType.Create.ToString());
                logEntity.F_Description += DbLogType.Create.ToDescription();
            }
            else
            {
                logEntity = await _logService.CreateLog(className, DbLogType.Update.ToString());
                logEntity.F_Description += DbLogType.Update.ToDescription();
                logEntity.F_KeyValue = keyValue;
            }
            if (areaEntity.F_ParentId=="0")
            {
                areaEntity.F_Layers = 1;
            }
            else
            {
                areaEntity.F_Layers =(await _areaService.GetForm(areaEntity.F_ParentId)).F_Layers + 1;
            }
            try
            {
                logEntity.F_Account = _areaService.currentuser.UserCode;
                logEntity.F_NickName = _areaService.currentuser.UserName;
                await _areaService.SubmitForm(areaEntity, keyValue);
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
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            LogEntity logEntity = await _logService.CreateLog(className, DbLogType.Delete.ToString());
            logEntity.F_Description += DbLogType.Delete.ToDescription();
            try
            {
                logEntity.F_Account = _areaService.currentuser.UserCode;
                logEntity.F_NickName = _areaService.currentuser.UserName;
                await _areaService.DeleteForm(keyValue);
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
