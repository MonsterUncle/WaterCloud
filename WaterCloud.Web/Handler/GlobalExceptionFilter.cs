using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using WaterCloud.Code;
using Serenity.Web;

namespace WaterCloud.Web
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            LogHelper.WriteWithTime(context.Exception);
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                AjaxResult obj = new AjaxResult();
                obj.state = ResultType.error.ToString();
                obj.message = context.Exception.GetOriginalException().Message;
                if (string.IsNullOrEmpty(obj.message))
                {
                    obj.message = "抱歉，系统错误，请联系管理员！";
                }
                context.Result = new JsonResult(obj);
                context.ExceptionHandled = true;
            }
            else
            {
                string errorMessage = context.Exception.GetOriginalException().Message;
                context.Result = new RedirectResult(context.HttpContext.Request.GetBaseUri() + "Home/Error?message=" + HttpUtility.UrlEncode(errorMessage));
                context.ExceptionHandled = true;
            }
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }
    }
}