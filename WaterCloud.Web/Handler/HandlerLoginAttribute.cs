using WaterCloud.Service.SystemManage;
using WaterCloud.Code;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Serenity.Web;
using Microsoft.AspNetCore.Mvc;
using System.Web;
/// <summary>
/// 登录验证
/// </summary>
namespace WaterCloud.Web
{
    public class HandlerLoginAttribute : ActionFilterAttribute
    {
        public bool Ignore = true;
        public HandlerLoginAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Ignore == false)
            {
                return;
            }
            if (OperatorProvider.Provider.GetCurrent() == null)
            {
                WebHelper.WriteCookie("WaterCloud_login_error", "overdue");
                filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=" + HttpUtility.UrlEncode("系统登录已超时，请重新登录！"));
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
                filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=" + HttpUtility.UrlEncode("很抱歉！您的权限不足，访问被拒绝！"));
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
                        filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=" + HttpUtility.UrlEncode("系统登录已超时,请重新登录！"));
                        return false;
                    case -1:
                        filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=" + HttpUtility.UrlEncode("账号未登录，请登录！"));
                        return false;
                    case -2:
                        filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=" + HttpUtility.UrlEncode("您的帐号已在其它地方登录,请重新登录！"));
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
                var roleId = OperatorProvider.Provider.GetCurrent().RoleId;
                var userId = OperatorProvider.Provider.GetCurrent().UserId;
                return new RoleAuthorizeService().RoleValidate(userId, roleId).Result;
            }
            catch (System.Exception)
            {

                return false;
            }
        }
    }
}