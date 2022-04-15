/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Code;
using WaterCloud.Service.SystemOrganize;

namespace WaterCloud.Web.Controllers
{
    public class HomeController : Controller
    {
        public SystemSetService _setService { get; set; }
        [HttpGet]
        [HandlerLogin]
        public ActionResult Index()
        {
            //主页信息获取
            if (_setService.currentuser.UserId == null)
            {
                return View();
            }
            var systemset = _setService.GetForm(_setService.currentuser.CompanyId).GetAwaiter().GetResult();
            ViewBag.ProjectName = systemset.ProjectName;
            ViewBag.LogoIcon = ".." + systemset.Logo;
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
