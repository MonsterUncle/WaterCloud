using WaterCloud.Code;
using System.Web.Mvc;
using System.Web;
using System.Text;
using WaterCloud.Application.SystemManage;
using WaterCloud.Application;
using WaterCloud.Domain;

namespace WaterCloud.Web
{
    public class HandlerLoginAttribute : AuthorizeAttribute
    {
        public bool Ignore = true;
        public HandlerLoginAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Ignore == false)
            {
                return;
            }
            if (OperatorProvider.Provider.GetCurrent() == null)
            {
                WebHelper.WriteCookie("WaterCloud_login_error", "overdue");
                filterContext.HttpContext.Response.Write("<script>top.location.href = '/Content/page/error.html?msg=" + "系统登录已超时，请重新登录！" + "';if(document.all) window.event.returnValue = false;</script>");
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
                filterContext.HttpContext.Response.Write("<script>top.location.href = '/Content/page/error.html?msg=" + "很抱歉！您的权限不足，访问被拒绝！" + "';if(document.all) window.event.returnValue = false;</script>");
                return;
            }
        }
        private bool LoginAuthorize(AuthorizationContext filterContext)
        {
            try
            {

                OperatorResult result = OperatorProvider.Provider.IsOnLine("pc_");
                switch (result.stateCode)
                {
                    case 1:
                        return true;
                    case 0:
                        filterContext.HttpContext.Response.Write("<script>top.location.href = '/Content/page/error.html?msg=" + "系统登录已超时,请重新登录！" + "';if(document.all) window.event.returnValue = false;</script>");
                        return false;
                    case -1:
                        filterContext.HttpContext.Response.Write("<script>top.location.href = '/Content/page/error.html?msg=" + "账号未登录，请登录！" + "';if(document.all) window.event.returnValue = false;</script>");
                        return false;
                    case -2:
                        filterContext.HttpContext.Response.Write("<script>top.location.href = '/Content/page/error.html?msg=" + "您的帐号已在其它地方登录,请重新登录！" + "';if(document.all) window.event.returnValue = false;</script>");
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
                return new RoleAuthorizeApp().RoleValidate(userId, roleId);
            }
            catch (System.Exception)
            {

                return false;
            }
        }
    }
}