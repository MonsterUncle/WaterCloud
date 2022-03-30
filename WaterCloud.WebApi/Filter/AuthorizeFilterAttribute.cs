using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;
using WaterCloud.Code;
using WaterCloud.Service.SystemOrganize;
using Microsoft.Extensions.DependencyInjection;

namespace WaterCloud.WebApi
{
	/// <summary>
	/// 权限验证
	/// </summary>
	public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        private readonly RoleAuthorizeService _service;
        private readonly string _authorize;
		private readonly bool _needAuth;
        /// <summary>
        /// 权限特性
        /// </summary>
        /// <param name="authorize">权限参数</param>
        /// <param name="needAuth">是否鉴权</param>
        public AuthorizeFilterAttribute(string authorize = "", bool needAuth = true)
        {
            _service = GlobalContext.ScopeServiceProvider.GetRequiredService<RoleAuthorizeService>();
            _authorize = authorize.ToLower();
            _needAuth = needAuth;
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
			if (_needAuth)
			{
                if (user == null || string.IsNullOrEmpty(_authorize))
                {
                    AlwaysResult obj = new AlwaysResult();
                    obj.message = "抱歉，没有操作权限";
                    obj.state = ResultType.error.ToString();
                    context.Result = new JsonResult(obj);
                    return;
                }
                if (!AuthorizeCheck())
                {
                    AlwaysResult obj = new AlwaysResult();
                    obj.message = "抱歉，没有操作权限";
                    obj.state = ResultType.error.ToString();
                    context.Result = new JsonResult(obj);
                    return;
                }
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
