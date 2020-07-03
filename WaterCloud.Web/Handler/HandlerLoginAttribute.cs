using WaterCloud.Service.SystemManage;
using WaterCloud.Code;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Serenity.Web;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using WaterCloud.Service.SystemOrganize;
using Chloe;
using WaterCloud.DataBase;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
/// <summary>
/// 登录验证
/// </summary>
namespace WaterCloud.Web
{
    public class HandlerLoginAttribute : ActionFilterAttribute
    {
        private readonly RoleAuthorizeService _service;
        public HandlerLoginAttribute(RoleAuthorizeService service)
        {
            _service = service;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var description =
                (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)filterContext.ActionDescriptor;

            //添加有允许匿名的Action，可以不用登录访问，如Login/Index
            //控制器整体忽略或者单独方法忽略
            var anonymous = description.ControllerTypeInfo.GetCustomAttribute(typeof(AllowAnonymousAttribute));
            var methodanonymous = description.MethodInfo.GetCustomAttribute(typeof(AllowAnonymousAttribute));
            if (anonymous != null|| methodanonymous!=null)
            {
                return;
            }
            if (OperatorProvider.Provider.GetCurrent() == null)
            {
                WebHelper.WriteCookie("WaterCloud_login_error", "overdue");
                //filterContext.HttpContext.Response.WriteAsync("<script>top.location.href = '" + filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=" + HttpUtility.UrlEncode("系统登录已超时，请重新登录！") + "';if(document.all) window.event.returnValue = false;</script>");
                filterContext.HttpContext.Response.WriteAsync("<script>top.location.href ='" + filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=408" + "';if(document.all) window.event.returnValue = false;</script>");
                OperatorProvider.Provider.EmptyCurrent("pc_");
                return;
            }
            //登录检测
            if (!this.LoginAuthorize(filterContext))
            {
                OperatorProvider.Provider.EmptyCurrent("pc_");
                return;
            }
            //管理员跳过检测
            if (OperatorProvider.Provider.GetCurrent().IsSystem)
            {
                return;
            }
            //用户和角色检测
            if (!this.RoleAuthorize())
            {
                OperatorProvider.Provider.EmptyCurrent("pc_");
                //filterContext.HttpContext.Response.WriteAsync("<script>top.location.href ='" + filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=" + HttpUtility.UrlEncode("很抱歉！您的权限不足，访问被拒绝！") + "';if(document.all) window.event.returnValue = false;</script>");
                filterContext.HttpContext.Response.WriteAsync("<script>top.location.href ='" + filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=403" + "';if(document.all) window.event.returnValue = false;</script>");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
        private bool LoginAuthorize(ActionExecutingContext filterContext)
        {
            try
            {

                OperatorResult result = OperatorProvider.Provider.IsOnLine("pc_").Result;
                switch (result.stateCode)
                {
                    case 1:
                        return true;
                    case 0:
                        //filterContext.HttpContext.Response.WriteAsync("<script>top.location.href = '" + filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=" + HttpUtility.UrlEncode("系统登录已超时,请重新登录！") + "';if(document.all) window.event.returnValue = false;</script>");
                        filterContext.HttpContext.Response.WriteAsync("<script>top.location.href ='" + filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=408" + "';if(document.all) window.event.returnValue = false;</script>");
                        return false;
                    case -1:
                        //filterContext.HttpContext.Response.WriteAsync("<script>top.location.href = '" + filterContext.HttpContext.Request.PathBase + "Home/Error?msg=" + HttpUtility.UrlEncode("账号未登录，请登录！") + "';if(document.all) window.event.returnValue = false;</script>");
                        filterContext.HttpContext.Response.WriteAsync("<script>top.location.href ='" + filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=408" + "';if(document.all) window.event.returnValue = false;</script>");
                        return false;
                    case -2:
                        //filterContext.HttpContext.Response.WriteAsync("<script>top.location.href = '" + filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=" + HttpUtility.UrlEncode("您的帐号已在其它地方登录,请重新登录！") + "';if(document.all) window.event.returnValue = false;</script>");
                        filterContext.HttpContext.Response.WriteAsync("<script>top.location.href ='" + filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=401" + "';if(document.all) window.event.returnValue = false;</script>");
                        return false;
                    default:
                        return false;
                }
            }
            catch (System.Exception)
            {

                return false;
            }

        }
        private bool RoleAuthorize()
        {
            try
            {
                var current = OperatorProvider.Provider.GetCurrent();
                var roleId = current.RoleId;
                var userId = current.UserId;
                return _service.RoleValidate(userId, roleId).Result;
            }
            catch (System.Exception)
            {

                return false;
            }
        }
    }
}