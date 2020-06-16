/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/

using Serenity;
using System;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Service;
using WaterCloud.Service.SystemSecurity;
using WaterCloud.Code;
using WaterCloud.Domain.SystemSecurity;
using System.Threading.Tasks;
using WaterCloud.Service.SystemOrganize;
using WaterCloud.Domain.SystemOrganize;
using Microsoft.AspNetCore.Http;

namespace WaterCloud.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly FilterIPService _filterIPService;
        private readonly UserService _userService;
        private readonly LogService _logService;
        private readonly SystemSetService _setService;
        public LoginController(FilterIPService filterIPService, UserService userService, LogService logService, SystemSetService setService)
        {
            _filterIPService = filterIPService;
            _userService = userService;
            _logService = logService;
            _setService = setService;
        }
        [HttpGet]
        public virtual ActionResult Index()
        {
            //根据域名判断租户
            var host= HttpContext.Request.Host.ToString();
            if (GlobalContext.SystemConfig.Debug)
            {
                host = "";
            }
            var systemset = _setService.GetFormByHost(host).Result;
            if (GlobalContext.SystemConfig.Demo)
            {
                ViewBag.UserName = Define.SYSTEM_USERNAME;
                ViewBag.Password = Define.SYSTEM_USERPWD;
            }
            ViewBag.ProjectName = systemset.F_ProjectName;
            ViewBag.LogoIcon ="../icon/"+ systemset.F_Logo;
            return View();
        }
        [HttpGet]
        public ActionResult GetAuthCode()
        {
            return File(new VerifyCodeHelper().GetVerifyCode(), @"image/Gif");
        }
        [HttpGet]
        public async Task<ActionResult> OutLogin()
        {
            await new LogService().WriteDbLog(new LogEntity
            {
                F_ModuleName = "系统登录",
                F_Type = DbLogType.Exit.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserCode,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "安全退出系统",
            });
            await OperatorProvider.Provider.EmptyCurrent("pc_");
            return Redirect("/Login/Index");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> CheckLoginState()
        {
            try
            {
                var operatorProvider = OperatorProvider.Provider.GetCurrent();
                if (operatorProvider==null)
                {
                    return Content(new AjaxResult { state = ResultType.error.ToString() }.ToJson());
                }
                //登录检测      
                if ((await OperatorProvider.Provider.IsOnLine("pc_")).stateCode<=0)
                {
                    await OperatorProvider.Provider.EmptyCurrent("pc_");
                    return Content(new AjaxResult { state = ResultType.error.ToString() }.ToJson());
                }
                else
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString() }.ToJson());
                }
            }
            catch (Exception)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString() }.ToJson());
            }

        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> CheckLogin(string username, string password,string localurl)
        {
            //根据域名判断租户
            LogEntity logEntity = new LogEntity();
            logEntity.F_ModuleName ="系统登录";
            logEntity.F_Type = DbLogType.Login.ToString();
            if (GlobalContext.SystemConfig.Debug)
            {
                localurl = "";
            }
            try
            {
                if (!await CheckIP())
                {
                    throw new Exception("IP受限");
                }
                UserEntity userEntity =await _userService.CheckLogin(username, password, localurl);
                OperatorModel operatorModel = new OperatorModel();
                operatorModel.UserId = userEntity.F_Id;
                operatorModel.UserCode = userEntity.F_Account;
                operatorModel.UserName = userEntity.F_RealName;
                operatorModel.CompanyId = userEntity.F_OrganizeId;
                operatorModel.DepartmentId = userEntity.F_DepartmentId;
                operatorModel.RoleId = userEntity.F_RoleId;
                operatorModel.LoginIPAddress = WebHelper.Ip;
                operatorModel.LoginIPAddressName = "本地局域网";//Net.GetLocation(operatorModel.LoginIPAddress);
                operatorModel.LoginTime = DateTime.Now;
                operatorModel.DdUserId = userEntity.F_DingTalkUserId;
                operatorModel.WxOpenId = userEntity.F_WxOpenId;
                operatorModel.IsAdmin = userEntity.F_IsAdmin.Value;
                operatorModel.IsBoss = userEntity.F_IsBoss.Value;
                operatorModel.IsLeaderInDepts = userEntity.F_IsLeaderInDepts.Value;
                operatorModel.IsSenior = userEntity.F_IsSenior.Value;
                SystemSetEntity setEntity = await _setService.GetForm(userEntity.F_OrganizeId);
                operatorModel.DBProvider = setEntity.F_DBProvider;
                operatorModel.DbString = setEntity.F_DbString;
                if (userEntity.F_Account == "admin")
                {
                    operatorModel.IsSystem = true;
                }
                else
                {
                    operatorModel.IsSystem = false;
                }
                await OperatorProvider.Provider.AddLoginUser(operatorModel, "","pc_");
                logEntity.F_Account = userEntity.F_Account;
                logEntity.F_NickName = userEntity.F_RealName;
                logEntity.F_Result = true;
                logEntity.F_Description = "登录成功";
                await _logService.WriteDbLog(logEntity);
                return Content(new AjaxResult { state = ResultType.success.ToString(), message = "登录成功。"}.ToJson());
            }
            catch (Exception ex)
            {
                logEntity.F_Account = username;
                logEntity.F_NickName = username;
                logEntity.F_Result = false;
                logEntity.F_Description = "登录失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        private async Task<bool> CheckIP()
        {
            string ip = WebHelper.Ip;
            return await _filterIPService.CheckIP(ip);
        }
    }
}
