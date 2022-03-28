using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WaterCloud.Code;
using WaterCloud.Service;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using System;
using Newtonsoft.Json.Serialization;

namespace WaterCloud.Web
{
	public class Startup:DefaultStartUp
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env)
        {
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSqlSugar()
                .AddQuartz()
                .ReviseSuperSysem()
                .AddRabbitMq()
                .AddSignalR(options =>
                {
                    //客户端发保持连接请求到服务端最长间隔，默认30秒，改成4分钟，网页需跟着设置connection.keepAliveIntervalInMilliseconds = 12e4;即2分钟
                    options.ClientTimeoutInterval = TimeSpan.FromMinutes(4);
                    //服务端发保持连接请求到客户端间隔，默认15秒，改成2分钟，网页需跟着设置connection.serverTimeoutInMilliseconds = 24e4;即4分钟
                    options.KeepAliveInterval = TimeSpan.FromMinutes(2);
                });
            services.AddDefaultMVC().AddNewtonsoftJson(options =>
            {
                // 返回数据首字母不小写，CamelCasePropertyNamesContractResolver是小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            //调试前端可更新
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            //清理缓存
            //CacheHelper.FlushAll().GetAwaiter().GetResult();
        }
        //AutoFac注入
        public void ConfigureContainer(ContainerBuilder builder)
        {
            AutofacConfigureContainer(builder, default, typeof(Controller), typeof(IDenpendency), typeof(Program));
            //注册特性
            builder.RegisterType(typeof(HandlerLoginAttribute)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(HandlerAuthorizeAttribute)).InstancePerLifetimeScope();
        }
        public override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);
            //MVC路由
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/chatHub");
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
