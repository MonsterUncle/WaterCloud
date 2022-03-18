using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;
using WaterCloud.Code;
using WaterCloud.Service.SystemOrganize;

namespace WaterCloud.WebApi
{
	/// <summary>
	/// 权限验证
	/// </summary>
	public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        private readonly RoleAuthorizeService _service;
        private string _authorize { get; set; }
        public AuthorizeFilterAttribute(RoleAuthorizeService service)
        {
            _service = service;
            _authorize = string.Empty;
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            OperatorModel user = OperatorProvider.Provider.GetCurrent();
            var description =
            (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;
            var methodanonymous = description.MethodInfo.GetCustomAttribute<Authorize>(false)!;
            if (user == null || methodanonymous == null)
            {
                AlwaysResult obj = new AlwaysResult();
                obj.message = "抱歉，没有操作权限";
                obj.state = ResultType.error.ToString();
                context.Result = new JsonResult(obj);
                return;
            }
            _authorize = methodanonymous._authorize;
            if (!AuthorizeCheck())
            {
                AlwaysResult obj = new AlwaysResult();
                obj.message = "抱歉，没有操作权限";
                obj.state = ResultType.error.ToString();
                context.Result = new JsonResult(obj);
                return;
            }
            var resultContext = await next();

            sw.Stop();

        }
        private bool AuthorizeCheck()
        {
            try
            {
                return _service.ActionValidate(_authorize, true).GetAwaiter().GetResult();
            }
            catch (System.Exception)
            {

                return false;
            }

        }
    }
}
