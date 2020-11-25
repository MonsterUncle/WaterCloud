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

namespace WaterCloud.Web.Areas.SystemManage.Controllers
{
    public class QuickModuleController : Controller
    {
        private QuickModuleApp moduleApp = new QuickModuleApp();
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
