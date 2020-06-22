using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using WaterCloud.Service.SystemManage;
using WaterCloud.Code;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Security.Policy;
using Serenity.Web;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Service.SystemOrganize;
using Chloe;
using WaterCloud.DataBase;
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
            if (OperatorProvider.Provider.GetCurrent() != null&& OperatorProvider.Provider.GetCurrent().IsSystem)
            {
                return;
            }
            if (Ignore == false)
            {
                return;
            }
            if (!ActionAuthorize(filterContext))
            {
                OperatorProvider.Provider.EmptyCurrent("pc_");
                //filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=" + HttpUtility.UrlEncode("很抱歉！您的权限不足，访问被拒绝！"));
                filterContext.HttpContext.Response.WriteAsync("<script>top.location.href ='" + filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=" + HttpUtility.UrlEncode("很抱歉！您的权限不足，访问被拒绝！") + "';if(document.all) window.event.returnValue = false;</script>");
                return;
            }
        }
        private bool ActionAuthorize(ActionExecutingContext filterContext)
        {
            try
            {
                OperatorResult result=OperatorProvider.Provider.IsOnLine("pc_").Result;
                if (result.stateCode<=0)
                {

                    return false;
                }
                var roleId = result.userInfo.RoleId;
                var action = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>().HttpContext.Request.Path;
                var current = OperatorProvider.Provider.GetCurrent();
                return new RoleAuthorizeService(DBContexHelper.Contex(current.DbString, current.DBProvider)).ActionValidate(roleId, action).Result;
            }
            catch (System.Exception)
            {

                return false;
            }

        }
    }
}