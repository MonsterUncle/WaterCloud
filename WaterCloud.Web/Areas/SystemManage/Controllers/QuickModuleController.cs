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
using Senparc.CO2NET.Extensions;

namespace WaterCloud.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class QuickModuleController : Controller
    {
        private QuickModuleService moduleApp = new QuickModuleService();
        [HttpGet]
        public virtual ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTransferJson()
        {
            var userId = OperatorProvider.Provider.GetCurrent().UserId;
            var data = moduleApp.GetTransferList(userId);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult SubmitForm(string permissionIds)
        {
            moduleApp.SubmitForm(permissionIds.Split(','));
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "操作成功" }.ToJson());
        }
    }
}
