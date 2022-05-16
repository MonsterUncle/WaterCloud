/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/

using System;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Service;
using WaterCloud.Service.SystemSecurity;
using WaterCloud.Code;
using WaterCloud.Domain.SystemSecurity;
using System.Threading.Tasks;
using WaterCloud.Service.SystemOrganize;
using WaterCloud.Domain.SystemOrganize;
using SqlSugar;
using System.Linq;
using System.Text;

namespace WaterCloud.Web.Controllers
{
	public class LoginController : Controller
    {
        public UserService _userService { get; set; }
        public LogService _logService { get; set; }
        public SystemSetService _setService { get; set; }
        public RoleAuthorizeService _roleAuthServuce { get; set; }
        public ISqlSugarClient _context { get; set; }
        [HttpGet]
        public virtual async Task<ActionResult> Index()
        {
            //登录页获取logo和项目名称
            try
            {
                var systemset = await _setService.GetFormByHost("");
                if (GlobalContext.SystemConfig.Demo)
                {
                    ViewBag.UserName = systemset.AdminAccount;
                    ViewBag.Password = systemset.AdminPassword;
                }
                ViewBag.SqlMode = GlobalContext.SystemConfig.SqlMode;
                ViewBag.ProjectName = systemset.ProjectName;
                ViewBag.LogoIcon = ".." + systemset.Logo;
                return View();
            }
            catch (Exception)
            {
                ViewBag.ProjectName = "水之云信息系统";
                ViewBag.LogoIcon = "../icon/favicon.ico";
                return View();
            }

        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetListJsonByLogin(string keyword)
        {
            var data = await _setService.GetList(keyword);
            data = data.OrderBy(a=>a.DbNumber).ToList();
            foreach (var item in data)
            {
                item.AdminAccount = null;
                item.AdminPassword = null;
                item.DBProvider = null;
                item.DbString = null;
                item.PrincipalMan = null;
                item.MobilePhone = null;
                item.CompanyName = null;
                item.LogoCode= null;
            }
            return Content(data.ToJson());
        }
        /// <summary>
        /// 验证码获取（此接口已弃用）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAuthCode()
        {
            return File(new VerifyCodeHelper().GetVerifyCode(), @"image/Gif");
        }
        [HttpGet]
        public async Task<ActionResult> OutLogin()
        {
            await _logService.WriteDbLog(new LogEntity
            {
                ModuleName = "系统登录",
                Type = DbLogType.Exit.ToString(),
                Account = _setService.currentuser.UserCode,
                NickName = _setService.currentuser.UserName,
                Result = true,
                Description = "安全退出系统",
            });
            await OperatorProvider.Provider.EmptyCurrent("pc_");
            return Content(new AlwaysResult { state = ResultType.success.ToString() }.ToJson());
        }
        /// <summary>
        /// 验证登录状态请求接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> CheckLoginState()
        {
            try
            {
                if (_setService.currentuser.UserId == null)
                {
                    return Content(new AlwaysResult { state = ResultType.error.ToString() }.ToJson());
                }
                //登录检测      
                if ((await OperatorProvider.Provider.IsOnLine("pc_")).stateCode<=0)
                {
                    await OperatorProvider.Provider.EmptyCurrent("pc_");
                    return Content(new AlwaysResult { state = ResultType.error.ToString() }.ToJson());
                }
                else
                {
                    //验证回退路由是否有权限，没有就删除
                    await CheckReturnUrl(_setService.currentuser.UserId);
                    return Content(new AlwaysResult { state = ResultType.success.ToString() }.ToJson());
                }
            }
            catch (Exception)
            {
                return Content(new AlwaysResult { state = ResultType.error.ToString() }.ToJson());
            }

        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户</param>
        /// <param name="password">密码</param>
        /// <param name="localurl">域名</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> CheckLogin(string username, string password,string localurl)
        {
            //根据域名判断租户
            LogEntity logEntity = new LogEntity();
            logEntity.ModuleName ="系统登录";
            logEntity.Type = DbLogType.Login.ToString();
            if (GlobalContext.SystemConfig.SqlMode==Define.SQL_MORE)
            {
                localurl = "";
            }
            try
            {
                UserEntity userEntity =await _userService.CheckLogin(username, password, localurl);
                OperatorModel operatorModel = new OperatorModel();
                operatorModel.UserId = userEntity.Id;
                operatorModel.UserCode = userEntity.Account;
                operatorModel.UserName = userEntity.RealName;
                operatorModel.CompanyId = userEntity.OrganizeId;
                operatorModel.DepartmentId = userEntity.DepartmentId;
                operatorModel.RoleId = userEntity.RoleId;
                operatorModel.LoginIPAddress = WebHelper.Ip;
                if (GlobalContext.SystemConfig.LocalLAN != false)
                {
                    operatorModel.LoginIPAddressName = "本地局域网";
                }
                else
                {
                    operatorModel.LoginIPAddressName = WebHelper.GetIpLocation(operatorModel.LoginIPAddress);
                }
                operatorModel.LoginTime = DateTime.Now;
                operatorModel.DdUserId = userEntity.DingTalkUserId;
                operatorModel.WxOpenId = userEntity.WxOpenId;
                //各租户的管理员也是当前数据库的全部权限
                operatorModel.IsSuperAdmin = userEntity.IsAdmin.Value;
                operatorModel.IsAdmin = userEntity.IsAdmin.Value;
                operatorModel.IsBoss = userEntity.IsBoss.Value;
                operatorModel.IsLeaderInDepts = userEntity.IsLeaderInDepts.Value;
                operatorModel.IsSenior = userEntity.IsSenior.Value;
                SystemSetEntity setEntity = await _setService.GetForm(userEntity.OrganizeId);
                operatorModel.DbNumber = setEntity.DbNumber;
                if (operatorModel.IsAdmin && operatorModel.DbNumber == GlobalContext.SystemConfig.MainDbNumber)
                {
                    operatorModel.IsSuperAdmin = true;
                }
                else
                {
                    operatorModel.IsSuperAdmin = false;
                }
                //缓存保存用户信息
                await OperatorProvider.Provider.AddLoginUser(operatorModel, "","pc_");
                //防重复token
                string token = Utils.GuId();
                HttpContext.Response.Cookies.Append("pc_" + GlobalContext.SystemConfig.TokenName, token);
                await CacheHelper.SetAsync("pc_" + GlobalContext.SystemConfig.TokenName + "_" + operatorModel.UserId + "_" + operatorModel.LoginTime, token, GlobalContext.SystemConfig.LoginExpire, true);
                logEntity.Account = userEntity.Account;
                logEntity.NickName = userEntity.RealName;
                logEntity.Result = true;
                logEntity.Description = "登录成功";
                await _logService.WriteDbLog(logEntity);
                //验证回退路由是否有权限，没有就删除
                await CheckReturnUrl(operatorModel.UserId);
                return Content(new AlwaysResult { state = ResultType.success.ToString(), message = "登录成功。"}.ToJson());
            }
            catch (Exception ex)
            {
                logEntity.Account = username;
                logEntity.NickName = username;
                logEntity.Result = false;
                logEntity.Description = "登录失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Content(new AlwaysResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }

        private async Task CheckReturnUrl(string userId)
        {
            var realurl = WebHelper.GetCookie("wc_realreturnurl");
            var url = WebHelper.GetCookie("wc_returnurl");
            if (!string.IsNullOrEmpty(realurl) && await _roleAuthServuce.CheckReturnUrl(userId, realurl))
            {
                WebHelper.RemoveCookie("wc_realreturnurl");
            }
            if (!string.IsNullOrEmpty(url)&& !await _roleAuthServuce.CheckReturnUrl(userId,url))
            {
                WebHelper.RemoveCookie("wc_returnurl");
            }
        }
    }
}
