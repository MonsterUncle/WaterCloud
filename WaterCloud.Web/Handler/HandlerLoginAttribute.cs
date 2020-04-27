using WaterCloud.Service.SystemManage;
using WaterCloud.Code;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
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
                filterContext.HttpContext.Response.WriteAsync("<script>top.location.href = '/page/error.html?msg=" + "系统登录已超时，请重新登录！" + "';</script>");
                return;
            }
            //登录检测
            if (!this.LoginAuthorize(filterContext))
            {
                OperatorProvider.Provider.EmptyCurrent();
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
                OperatorProvider.Provider.EmptyCurrent();
                filterContext.HttpContext.Response.WriteAsync("<script>top.location.href = '/page/error.html?msg=" + "很抱歉！您的权限不足，访问被拒绝！" + "';</script>");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
        private bool LoginAuthorize(ActionExecutingContext filterContext)
        {
            try
            {

                OperatorResult result = OperatorProvider.Provider.IsOnLine("pc_");
                switch (result.stateCode)
                {
                    case 1:
                        return true;
                    case 0:
                        filterContext.HttpContext.Response.WriteAsync("<script>top.location.href = '/page/error.html?msg=" + "系统登录已超时,请重新登录！" + "';</script>");
                        return false;
                    case -1:
                        filterContext.HttpContext.Response.WriteAsync("<script>top.location.href = '/page/error.html?msg=" + "账号未登录，请登录！" + "';</script>");
                        return false;
                    case -2:
                        filterContext.HttpContext.Response.WriteAsync("<script>top.location.href = '/page/error.html?msg=" + "您的帐号已在其它地方登录,请重新登录！" + "';</script>");
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
                return new RoleAuthorizeService().RoleValidate(userId, roleId);
            }
            catch (System.Exception)
            {

                return false;
            }
        }
    }
}