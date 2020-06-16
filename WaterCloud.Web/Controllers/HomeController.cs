/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Code;
using WaterCloud.Service.SystemManage;
using WaterCloud.Service.SystemOrganize;

namespace WaterCloud.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly SystemSetService _setService;
        public HomeController(SystemSetService setService)
        {
            _setService = setService;
        }
        [HttpGet]
        [HandlerLogin]
        public ActionResult Index()
        {
            var currentuser = OperatorProvider.Provider.GetCurrent();
            if (currentuser==null)
            {
                return View();
            }
            var systemset = _setService.GetForm(currentuser.CompanyId).Result;
            ViewBag.ProjectName = systemset.F_ProjectName;
            ViewBag.LogoIcon = "../icon/" + systemset.F_Logo;
            return View();
        }
        [HttpGet]
        [HandlerLogin]
        public ActionResult Default()
        {
            return View();
        }
        [HttpGet]
        [HandlerLogin]
        public ActionResult UserSetting()
        {
            return View();
        }
        [HttpGet]
        [HandlerLogin(false)]
        public ActionResult Error()
        {
            return View();
        }
        [HttpGet]
        [HandlerLogin]
        public ActionResult Message()
        {
            return View();
        }
    }
}
