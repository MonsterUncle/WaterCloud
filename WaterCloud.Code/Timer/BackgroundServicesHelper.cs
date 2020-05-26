using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WaterCloud.Code
{
    /// <summary>
    /// 已废除
    /// </summary>
    public static class BackgroundServicesHelper
    {
        /// <summary>
        /// 反射取得所有的业务逻辑类
        /// </summary>
        private static Type[] GetAllChildClass(Type baseType)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
            //取得实现了某个接口的类
            //.SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ISecurity))))  .ToArray();
            //取得继承了某个类的所有子类
            .SelectMany(a => a.GetTypes().Where(t => t.BaseType == baseType))
            .ToArray();

            return types;
        }


        public static Type[] GetAllBackgroundService()
        {
            return GetAllChildClass(typeof(BackgroundService));
        }

        /// <summary>
        /// 自动增加后台任务.所有继承自BackgroundService的类都会自动运行
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            //services.AddHostedService<TimedService>(); //asp.net core 应该是这个.
            //或者  单为方便循环自动创建, 所以改成使用AddTransient 也一样可以使用.
            //services.AddTransient<IHostedService, TimedService>();  
            //services.AddTransient(typeof(Microsoft.Extensions.Hosting.IHostedService),backtype);
            //var backtypes = BackgroundServicesHelper.GetAllBackgroundService();
            //foreach (var backtype in backtypes)
            //{
            //    services.AddTransient(typeof(Microsoft.Extensions.Hosting.IHostedService),backtype);
            //}

            var backtypes = GetAllBackgroundService();
            foreach (var backtype in backtypes)
            {
                services.AddTransient(typeof(Microsoft.Extensions.Hosting.IHostedService), backtype);
            }
            return services;
        }


    }
}

