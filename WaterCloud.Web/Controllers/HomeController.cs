/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using Microsoft.AspNetCore.Mvc;

namespace WaterCloud.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [HandlerLogin]
        public ActionResult Index()
        {

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
