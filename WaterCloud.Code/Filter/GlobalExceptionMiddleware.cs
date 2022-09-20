using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace WaterCloud.Code
{
	/// <summary>
	/// 全局异常中间件，api使用
	/// </summary>
	public class GlobalExceptionMiddleware
	{
		private readonly RequestDelegate next;

		public GlobalExceptionMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context /* other dependencies */)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var code = HttpStatusCode.OK;
			LogHelper.WriteWithTime(exception);
			var result = JsonConvert.SerializeObject(new AlwaysResult
			{
				state = ResultType.error.ToString(),
				message = exception.Message
			});
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)code;
			return context.Response.WriteAsync(result);
		}
	}
}