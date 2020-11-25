using WaterCloud.Application.SystemManage;
using WaterCloud.Code;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WaterCloud.Web
{
    public class HandlerAuthorizeAttribute : ActionFilterAttribute
    {
        public bool Ignore { get; set; }
        public HandlerAuthorizeAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (OperatorProvider.Provider.GetCurrent() == null)
            {
                WebHelper.WriteCookie("WaterCloud_login_error", "overdue");
                filterContext.HttpContext.Response.Write("<script>top.location.href = '/Content/page/error.html?msg=" + "系统登录已超时，请重新登录！" + "';if(document.all) window.event.returnValue = false;</script>");
                return;
            }
            if (OperatorProvider.Provider.GetCurrent().IsSystem)
            {
                return;
            }
            if (Ignore == false)
            {
                return;
            }
            if (!this.ActionAuthorize(filterContext))
            {
                OperatorProvider.Provider.EmptyCurrent();
                filterContext.HttpContext.Response.Write("<script>top.location.href = '/Content/page/error.html?msg=" + "很抱歉！您的权限不足，访问被拒绝！" + "';if(document.all) window.event.returnValue = false;</script>");
                return;
            }
        }
        private bool ActionAuthorize(ActionExecutingContext filterContext)
        {
            try
            {
                OperatorResult result= OperatorProvider.Provider.IsOnLine("pc_");
                if (result.stateCode<=0)
                {

                    return false;
                }
                var roleId = result.userInfo.RoleId;
                var action = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
                return new RoleAuthorizeApp().ActionValidate(roleId, action);
            }
            catch (System.Exception)
            {

                return false;
            }

        }
    }
}