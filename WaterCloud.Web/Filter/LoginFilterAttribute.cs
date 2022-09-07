using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;
using WaterCloud.Code;
using Microsoft.AspNetCore.Authorization;
using WaterCloud.Service.SystemOrganize;

namespace WaterCloud.Web
{
    /// <summary>
    /// 验证token
    /// </summary>
    public class LoginFilterAttribute : ActionFilterAttribute
    {
        private readonly bool _needLogin;
        /// <summary>
        /// 登录特性
        /// </summary>
        /// <param name="needLogin">是否验证</param>
		public LoginFilterAttribute(bool needLogin = true)
        {
            _needLogin = needLogin;
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
            string token = context.HttpContext.Request.Headers[GlobalContext.SystemConfig.TokenName].ParseToString();
            OperatorModel user = OperatorProvider.Provider.GetCurrent();
            var description =
            (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;

            //添加有允许匿名的Action，可以不用登录访问，如Login/Index
            //控制器整体忽略或者单独方法忽略
            var anonymous = description.ControllerTypeInfo.GetCustomAttribute(typeof(AllowAnonymousAttribute));
            var methodanonymous = description.MethodInfo.GetCustomAttribute(typeof(AllowAnonymousAttribute));
            if (user != null && RoleAuthorize())
            {
                //延长过期时间
                int LoginExpire = GlobalContext.SystemConfig.LoginExpire;
                string cacheKeyOperator = GlobalContext.SystemConfig.ProjectPrefix + "_operator_";// +登录者token
                await CacheHelper.ExpireAsync(cacheKeyOperator + token, LoginExpire);
                await CacheHelper.ExpireAsync(cacheKeyOperator + "api_" + user.UserId, LoginExpire);
                // 根据传入的Token，添加token和客户参数
                if (context.ActionArguments != null && context.ActionArguments.Count > 0)
                {
                    PropertyInfo property = context.ActionArguments.First().Value.GetType().GetProperty("Token");
                    if (property != null)
                    {
                        property.SetValue(context.ActionArguments.First().Value, token, null);
                    }
                    switch (context.HttpContext.Request.Method.ToUpper())
                    {
                        case "GET":
                            break;

                        case "POST":
                            property = context.ActionArguments.First().Value.GetType().GetProperty("CustomerId");
                            if (property != null)
                            {
                                property.SetValue(context.ActionArguments.First().Value, user.UserId, null);
                            }
                            break;
                    }
                }
            }
            else if (anonymous == null && methodanonymous == null && _needLogin)
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
        private bool RoleAuthorize()
        {
            try
            {
                return GlobalContext.GetRequiredService<RoleAuthorizeService>().RoleValidate().GetAwaiter().GetResult();
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteWithTime(ex);
                return false;
            }
        }
    }
}
