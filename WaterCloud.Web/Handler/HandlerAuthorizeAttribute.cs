using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using WaterCloud.Service.SystemManage;
using WaterCloud.Code;
using Microsoft.Extensions.DependencyInjection;
/// <summary>
/// 权限验证
/// </summary>
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
                filterContext.HttpContext.Response.WriteAsync("<script>top.location.href = '/page/error.html?msg=" + "很抱歉！您的权限不足，访问被拒绝！" + "';</script>");
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
                var action = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>().HttpContext.Request.Headers["SCRIPT_NAME"].ToString();
                return new RoleAuthorizeService().ActionValidate(roleId, action);
            }
            catch (System.Exception)
            {

                return false;
            }

        }
    }
}