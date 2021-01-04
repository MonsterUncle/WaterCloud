using WaterCloud.Service.SystemManage;
using WaterCloud.Code;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Serenity.Web;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using WaterCloud.Service.SystemOrganize;
using Chloe;
using WaterCloud.DataBase;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
/// <summary>
/// 防重复锁
/// </summary>
namespace WaterCloud.Web
{
    public class HandlerLockAttribute : ActionFilterAttribute
    {
        public HandlerLockAttribute()
        {
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (OperatorProvider.Provider.GetCurrent() == null)
            {
                WebHelper.WriteCookie("WaterCloud_login_error", "overdue");
                //filterContext.HttpContext.Response.WriteAsync("<script>top.location.href ='" + filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=408" + "';if(document.all) window.event.returnValue = false;</script>");
                OperatorProvider.Provider.EmptyCurrent("pc_").GetAwaiter().GetResult();
                filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.PathBase + "/Home/Error?msg=408");
                return;
            }
			else
			{
                string token = filterContext.HttpContext.Request.Cookies[GlobalContext.SystemConfig.TokenName];
				if (string.IsNullOrWhiteSpace(token))
				{
                    filterContext.Result = new JsonResult(new AlwaysResult { state = ResultType.error.ToString(), message = "toekn不能空" });
                    return;
                }
                bool result= CacheHelper.SetNx(token, token).GetAwaiter().GetResult();
                if (!result)
				{
                    filterContext.Result = new JsonResult(new AlwaysResult { state = ResultType.error.ToString(), message = "请求太频繁，请稍后" });
                    return;
                }
            }
            //随机值
            //token刷新（去除的话，变成唯一锁）
            string cacheToken = Utils.GuId();
            filterContext.HttpContext.Response.Cookies.Append(GlobalContext.SystemConfig.TokenName, cacheToken);
            base.OnActionExecuting(filterContext);
        }
        //失败才刷新
		//public override void OnActionExecuted(ActionExecutedContext context)
		//{
  //          var data = context.Result.ToJson().ToJObject();
		//	if (data.ContainsKey("Content"))
		//	{
  //              var result = data["Content"].ToString().ToJObject();
  //              if (result["state"].ToString() == "error")
  //              {
  //                  //token刷新
  //                  string cacheToken = Utils.GuId();
  //                  context.HttpContext.Response.Cookies.Append(GlobalContext.SystemConfig.TokenName, cacheToken);
  //              }
  //          }

  //          base.OnActionExecuted(context);
		//}
	}
}