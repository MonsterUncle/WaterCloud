﻿using System;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
using WaterCloud.Code.Model;
namespace WaterCloud.Code
{
	public class GlobalContext
    {
        /// <summary>
        /// 服务集合
        /// </summary>
        public static IServiceCollection Services { get; set; }

        /// <summary>
        /// Rootservice
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }
        /// <summary>
        /// ScopeService
        /// </summary>
        public static IServiceProvider ScopeServiceProvider => ServiceProvider.CreateScope().ServiceProvider;

        public static IConfiguration Configuration { get; set; }

        public static IWebHostEnvironment HostingEnvironment { get; set; }

        public static SystemConfig SystemConfig { get; set; }

        public static string GetVersion()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            return version.ToString();
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
