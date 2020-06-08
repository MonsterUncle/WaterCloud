/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Service;
using WaterCloud.Service.SystemManage;
using WaterCloud.Service.SystemSecurity;
using System.Threading.Tasks;

namespace WaterCloud.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class ItemsDataController : ControllerBase
    {
        private string moduleName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace.Split('.')[3];
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        private readonly ItemsDataService _itemsDetailService;
        private readonly LogService _logService;
        public ItemsDataController(ItemsDataService itemsDetailService, LogService logService)
        {
            _itemsDetailService = itemsDetailService;
            _logService = logService;
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(string itemId, string keyword)
        {
            var data =await _itemsDetailService.GetLookList(itemId, keyword);
            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetSelectJson(string enCode)
        {
            var data =await _itemsDetailService.GetItemList(enCode);
            List<object> list = new List<object>();
            foreach (ItemsDetailEntity item in data)
            {
                list.Add(new { id = item.F_ItemCode, text = item.F_ItemName });
            }
            return Content(list.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data =await _itemsDetailService.GetLookForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(ItemsDetailEntity itemsDetailEntity, string keyValue)
        {
            LogEntity logEntity;
            if (string.IsNullOrEmpty(keyValue))
            {
                itemsDetailEntity.F_DeleteMark = false;
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
                await _itemsDetailService.SubmitForm(itemsDetailEntity, keyValue);
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
                await _itemsDetailService.DeleteForm(keyValue);
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
