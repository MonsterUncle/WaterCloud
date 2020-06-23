using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using System;
/// <summary>
/// ajax验证
/// </summary>
namespace WaterCloud.Web
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HandlerAjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public bool Ignore { get; set; }
        public HandlerAjaxOnlyAttribute(bool ignore = false)
        {
            Ignore = ignore;
        }
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            if (Ignore)
                return true;
            bool result = false;
            var xreq = routeContext.HttpContext.Request.Headers.ContainsKey("x-requested-with");
            if (xreq)
            {
                result = routeContext.HttpContext.Request.Headers.ContainsKey("x-requested-with");
            }
            return result;
        }
    }
}
