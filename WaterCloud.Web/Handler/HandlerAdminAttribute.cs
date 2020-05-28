using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Serenity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCloud.Code;

namespace WaterCloud.Web
{
    public class HandlerAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (OperatorProvider.Provider.GetCurrent() != null && OperatorProvider.Provider.GetCurrent().IsSystem)
            {
                return;
            }
            else
            {
                filterContext.HttpContext.Response.WriteAsync("<script>top.location.href = '"+ filterContext .HttpContext.Request.PathBase + "/Home/Error?msg=" + "很抱歉！您的权限不足，请使用管理员登录！" + "';window.event.returnValue = false;</script>");
                return;
            }
        }
    }
}