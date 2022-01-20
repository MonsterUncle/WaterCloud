using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serenity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WaterCloud.Code;

namespace WaterCloud.Web
{
    public class HandlerAdminAttribute : ActionFilterAttribute
    {
        private readonly bool _isSuper;
        public HandlerAdminAttribute(bool isSuper=true)
		{
            _isSuper = isSuper;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (OperatorProvider.Provider.GetCurrent() != null && _isSuper==true?OperatorProvider.Provider.GetCurrent().IsSuperAdmin: OperatorProvider.Provider.GetCurrent().IsAdmin)
            {
                return;
            }
            else
            {
                //filterContext.HttpContext.Response.WriteAsync("<script>top.location.href ='" + filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=403" + "';if(document.all) window.event.returnValue = false;</script>");
                filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=403");
                return;
            }
        }
    }
}