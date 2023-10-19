using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WaterCloud.Code;
using WaterCloud.Service;

namespace WaterCloud.Web
{
	public class Startup : DefaultStartUp
	{
		private List<string> _plugins = new List<string> { "WaterCloud.Service" };
		public Startup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env)
		{
		}

		public override void ConfigureServices(IServiceCollection services)
		{
            base.ConfigureServices(services);
            services.AddDefaultSwaggerGen(Assembly.GetExecutingAssembly().GetName().Name)
				.AddSqlSugar()
				.AddQuartz()
				.ReviseSuperSysem()
				.AddEventBus()
				.AddRabbitMq()
				.AddWorkerService()
				.AddSignalR(options =>
				{
					//客户端发保持连接请求到服务端最长间隔，默认30秒，改成4分钟，网页需跟着设置connection.keepAliveIntervalInMilliseconds = 12e4;即2分钟
					options.ClientTimeoutInterval = TimeSpan.FromMinutes(4);
					//服务端发保持连接请求到客户端间隔，默认15秒，改成2分钟，网页需跟着设置connection.serverTimeoutInMilliseconds = 24e4;即4分钟
					options.KeepAliveInterval = TimeSpan.FromMinutes(2);
				});
			services.AddDefaultAPI();
			services.AddDefaultMVC()
			.ConfigureApplicationPartManager(apm => {
                var plugDir = Directory.GetCurrentDirectory() + "/Plugins";
                if (!Directory.Exists(plugDir))
                    Directory.CreateDirectory(plugDir);
                var paths = Directory.GetDirectories(plugDir);
				var hostAssembly= Assembly.GetExecutingAssembly();
				foreach (var path in paths)
				{
					var name=Path.GetFileName(path);
					var fn = path + "/" + name + ".dll";
					if (File.Exists(fn))
					{
                        var addInAssembly = Assembly.LoadFrom(fn);
                        var controllerAssemblyPart = new AssemblyPart(addInAssembly);
                        apm.ApplicationParts.Add(controllerAssemblyPart);
                        //开始判断是不是有razor页面
                        fn = path + "/" + name + ".Views.dll";
                        if (File.Exists(fn))
                        {
                            var addInAssemblyView = Assembly.LoadFrom(fn);
                            var viewAssemblyPart = new CompiledRazorAssemblyPart(addInAssemblyView);
                            apm.ApplicationParts.Add(viewAssemblyPart);
                        }
						else
						{
                            var viewAssemblyPart = new CompiledRazorAssemblyPart(addInAssembly);
                            apm.ApplicationParts.Add(viewAssemblyPart);
                        }
                        _plugins.Add(path);
                    }
                }
				_plugins = _plugins.Distinct().ToList();
            })
			.AddNewtonsoftJson(options =>
			{
				// 返回数据首字母不小写，CamelCasePropertyNamesContractResolver是小写
				options.SerializerSettings.ContractResolver = new DefaultContractResolver();
			});
			//调试前端可更新
			services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            //清理缓存
            //CacheHelper.FlushAllAsync().GetAwaiter().GetResult();
        }

		//AutoFac注入
		public void ConfigureContainer(ContainerBuilder builder)
		{
			AutofacConfigureContainer(builder, _plugins, typeof(Controller), typeof(IDenpendency), typeof(Program));
			AutofacConfigureContainer(builder, _plugins, typeof(ControllerBase), typeof(IDenpendency), typeof(Program));
		}

		public override void Configure(IApplicationBuilder app)
		{
			base.Configure(app);
			//MVC路由
			app.UseMiddleware(typeof(GlobalExceptionMiddleware))
			   .AddDefaultSwaggerGen()
			   .UseEndpoints(endpoints =>
				{
					endpoints.MapHub<MessageHub>("/chatHub");
					endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
					endpoints.MapControllerRoute("default", "{controller=Login}/{action=Index}/{id?}");
					endpoints.MapControllerRoute("api", "api/{controller=ApiHome}/{action=Index}/{id?}");
					endpoints.MapRazorPages();
				});
		}
	}
}