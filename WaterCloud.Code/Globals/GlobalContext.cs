using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using WaterCloud.Code.Model;

namespace WaterCloud.Code
{
	public static class GlobalContext
	{

        /// <summary>
        /// 构造函数
        /// </summary>
        static GlobalContext()
        {
            // 未托管的对象
            UnmanagedObjects = new ConcurrentBag<IDisposable>();
        }
        /// <summary>
        /// 服务集合
        /// </summary>
        public static IServiceCollection Services { get; set; }

		/// <summary>
		/// 根服务
		/// </summary>
		public static IServiceProvider RootServices { get; set; }

		public static IConfiguration Configuration { get; set; }

		public static IWebHostEnvironment HostingEnvironment { get; set; }

        /// <summary>
        /// 未托管的对象集合
        /// </summary>
        public static readonly ConcurrentBag<IDisposable> UnmanagedObjects;

        public static HttpContext HttpContext => RootServices?.GetService<IHttpContextAccessor>()?.HttpContext;

		public static SystemConfig SystemConfig { get; set; }

        /// <summary>
        /// 获取请求生存周期的服务(未注册返回null)
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static TService GetService<TService>(IServiceProvider serviceProvider = null) where TService : class
		{
			return GetService(typeof(TService), serviceProvider) as TService;
		}

		/// <summary>
		/// 获取请求生存周期的服务(未注册返回null)
		/// </summary>
		/// <param name="type"></param>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
		public static object GetService(Type type, IServiceProvider serviceProvider = null)
		{
			return (serviceProvider ?? GetServiceProvider(type)).GetService(type);
		}

		/// <summary>
		/// 获取请求生存周期的服务(未注册异常)
		/// </summary>
		/// <typeparam name="TService"></typeparam>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
		public static TService GetRequiredService<TService>(IServiceProvider serviceProvider = null) where TService : class
		{
			return GetRequiredService(typeof(TService), serviceProvider) as TService;
		}

		/// <summary>
		/// 获取请求生存周期的服务(未注册异常)
		/// </summary>
		/// <typeparam name="type"></typeparam>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
		public static object GetRequiredService(Type type, IServiceProvider serviceProvider = null)
		{
			return (serviceProvider ?? GetServiceProvider(type)).GetRequiredService(type);
		}

		/// <summary>
		/// 获取服务注册器
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		public static IServiceProvider GetServiceProvider(Type serviceType)
		{
			if (HostingEnvironment == null)
			{
				return RootServices;
			}
			if (RootServices != null && Services.Where((ServiceDescriptor u) => u.ServiceType == (serviceType.IsGenericType ? serviceType.GetGenericTypeDefinition() : serviceType)).Any((ServiceDescriptor u) => u.Lifetime == ServiceLifetime.Singleton))
			{
				return RootServices;
			}
            // 第二选择是获取 HttpContext 对象的 RequestServices
            var httpContext = HttpContext;
            if (httpContext?.RequestServices != null) return httpContext.RequestServices;
            // 第三选择，创建新的作用域并返回服务提供器
            else if (RootServices != null)
            {
                var scoped = RootServices.CreateScope();
                UnmanagedObjects.Add(scoped);
                return scoped.ServiceProvider;
            }
            // 第四选择，构建新的服务对象（性能最差）
            else
            {
                var serviceProvider = Services.BuildServiceProvider();
                UnmanagedObjects.Add(serviceProvider);
                return serviceProvider;
            }
        }


        /// <summary>
        /// GC 回收默认间隔
        /// </summary>
        private const int GC_COLLECT_INTERVAL_SECONDS = 5;

        /// <summary>
        /// 记录最近 GC 回收时间
        /// </summary>
        private static DateTime? LastGCCollectTime { get; set; }

        /// <summary>
        /// 释放所有未托管的对象
        /// </summary>
        public static void DisposeUnmanagedObjects()
        {
            foreach (var dsp in UnmanagedObjects)
            {
                try
                {
                    dsp?.Dispose();
                }
                finally { }
            }

            // 强制手动回收 GC 内存
            if (UnmanagedObjects.Any())
            {
                var nowTime = DateTime.UtcNow;
                if ((LastGCCollectTime == null || (nowTime - LastGCCollectTime.Value).TotalSeconds > GC_COLLECT_INTERVAL_SECONDS))
                {
                    LastGCCollectTime = nowTime;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }

            UnmanagedObjects.Clear();
        }
        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
		{
			Version version = Assembly.GetEntryAssembly().GetName().Version;
			return version.ToString();
		}
        /// <summary>
        /// 获取请求跟踪 Id
        /// </summary>
        /// <returns></returns>
        public static string GetTraceId()
        {
            return Activity.Current?.Id ?? (RootServices == null ? default : HttpContext?.TraceIdentifier);
        }
        /// <summary>
        /// 程序启动时，记录目录
        /// </summary>
        /// <param name="env"></param>
        public static void LogWhenStart(IWebHostEnvironment env)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("程序启动");
			sb.AppendLine("ContentRootPath:" + env.ContentRootPath);
			sb.AppendLine("WebRootPath:" + env.WebRootPath);
			sb.AppendLine("IsDevelopment:" + env.IsDevelopment());
			LogHelper.WriteWithTime(sb.ToString());
		}

		/// <summary>
		/// 设置cache control
		/// </summary>
		/// <param name="context"></param>
		public static void SetCacheControl(StaticFileResponseContext context)
		{
			int second = 365 * 24 * 60 * 60;
			context.Context.Response.Headers.Add("Cache-Control", new[] { "public,max-age=" + second });
			context.Context.Response.Headers.Add("Expires", new[] { DateTime.UtcNow.AddYears(1).ToString("R") }); // Format RFC1123
		}
    }
}