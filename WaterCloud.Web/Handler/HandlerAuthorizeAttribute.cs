﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using WaterCloud.Code;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Service.SystemOrganize;
using System.Reflection;
/// <summary>
/// 权限验证
/// </summary>
namespace WaterCloud.Web
{
	public class HandlerAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly RoleAuthorizeService _service;
		private readonly bool _needAuth;

		private string _authorize { get; set; }
        /// <summary>
        /// 权限特性
        /// </summary>
        /// <param name="authorize">权限参数</param>
        /// <param name="needAuth">是否鉴权</param>
        public HandlerAuthorizeAttribute(string authorize = "", bool needAuth = true)
        {
            _service = GlobalContext.ScopeServiceProvider.GetRequiredService<RoleAuthorizeService>();
            _authorize = authorize.ToLower();
			_needAuth = needAuth;
		}
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (OperatorProvider.Provider.GetCurrent() != null && OperatorProvider.Provider.GetCurrent().IsSuperAdmin)
            {
                return;
            }
            if (!_needAuth)
            {
                return;
            }
            if (!string.IsNullOrEmpty(_authorize)&&AuthorizeCheck(filterContext))
			{
                return;
            }
            if (!ActionAuthorize(filterContext))
            {
                OperatorProvider.Provider.EmptyCurrent("pc_").GetAwaiter().GetResult();
                //filterContext.HttpContext.Response.WriteAsync("<script>top.location.href ='" + filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=403" + "';if(document.all) window.event.returnValue = false;</script>");
                filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=403");
                return;
            }
        }
        private bool ActionAuthorize(ActionExecutingContext filterContext)
        {
            try
            {
                OperatorResult result=OperatorProvider.Provider.IsOnLine("pc_").GetAwaiter().GetResult();
                if (result.stateCode<=0)
                {

                    return false;
                }
                var action = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>().HttpContext.Request.Path;
                return _service.ActionValidate(action).GetAwaiter().GetResult();
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteWithTime(ex);
                return false;
            }

        }
        private bool AuthorizeCheck(ActionExecutingContext filterContext)
        {
            try
            {
                OperatorResult result = OperatorProvider.Provider.IsOnLine("pc_").GetAwaiter().GetResult();
                if (result.stateCode <= 0)
                {

                    return false;
                }
                return _service.ActionValidate(_authorize,true).GetAwaiter().GetResult();
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteWithTime(ex);
                return false;
            }

        }
    }
}