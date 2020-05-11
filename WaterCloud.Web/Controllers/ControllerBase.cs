using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serenity;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Code.Model;

namespace WaterCloud.Web
{
    [HandlerLogin]
    public abstract class ControllerBase : Controller
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string action = context.RouteData.Values["Action"].ParseToString();
            OperatorModel user =  OperatorProvider.Provider.GetCurrent();

            if (GlobalContext.SystemConfig.Demo)
            {
                if (context.HttpContext.Request.Method.ToUpper() == "POST")
                {
                    string[] allowAction = new string[] { "LoginJson", "ExportUserJson", "CodePreviewJson" };
                    if (!allowAction.Select(p => p.ToUpper()).Contains(action.ToUpper()))
                    {

                        string Message = "演示模式，不允许操作";
                        context.Result = new JsonResult(new AjaxResult{
                            state = ResultType.error.ToString(),
                            message = Message

                        });
                        return;
                    }
                }
            }
            var resultContext = await next();
            sw.Stop();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Form()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Details()
        {
            return View();
        }
        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message }.ToJson());
        }
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message, data = data }.ToJson());
        }
        protected virtual ActionResult SuccessByEasyUI(bool state, string message)
        {
            return Content(new AjaxResultByEasyUI { success = state, message = message }.ToJson());
        }
        protected virtual ActionResult ResultDataGrid(int total, object data)
        {
            return Content(new AjaxResultDataGrid { total = total, rows = data }.ToJson());
        }
        protected virtual ActionResult ResultLayUiTable(int total, object data)
        {
            return Content(new AjaxResultLayUiTable {code=0,msg="", count = total, data = data }.ToJson());
        }
        protected virtual ActionResult ResultDTree(object data)
        {
            return Content(new AjaxResultDTree { status =new StatusInfo {code=200,message= "操作成功" }, data = data }.ToJson());
        }
        protected virtual ActionResult Error(string message)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = message }.ToJson());
        }
    }
}
