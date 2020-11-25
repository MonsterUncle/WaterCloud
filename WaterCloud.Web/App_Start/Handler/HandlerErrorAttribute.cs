using WaterCloud.Code;
using System.Web.Mvc;
using System.Web;

namespace WaterCloud.Web
{
    public class HandlerErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            WriteLog(context);
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode = 200;
                context.Result = new ContentResult { Content = new AjaxResult { state = ResultType.error.ToString(), message = context.Exception.Message }.ToJson() };
            }
            else
            {
                string errorMessage = context.Exception.Message;
                context.Result = new RedirectResult("/Content/page/error.html?message=" + HttpUtility.UrlEncode(errorMessage));
                context.ExceptionHandled = true;
            }

        }
        private void WriteLog(ExceptionContext context)
        {
            if (context == null)
                return;
            var log = LogFactory.GetLogger(context.Controller.ToString());
            log.Error(context.Exception);
        }
    }
}