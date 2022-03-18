using System;
using System.IO;
using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using WaterCloud.Code;
using WaterCloud.Service;

namespace WaterCloud.WebApi
{
	public class Startup:DefaultStartUp
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env):base(configuration, env)
        {
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "WaterCloud Api", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath, true); //添加控制器层注释（true表示显示控制器注释）                
            });
            services.AddSqlSugar();
            services.AddDefaultAPI().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            AutofacConfigureContainer(builder, default, typeof(ControllerBase), typeof(IDenpendency), typeof(Program));
            //注册特性
            builder.RegisterType(typeof(AuthorizeFilterAttribute)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(LoginFilterAttribute)).InstancePerLifetimeScope();
        }
        public override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);
            //api全局异常
            app.UseMiddleware(typeof(GlobalExceptionMiddleware));
            app.Use(next => context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            });
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api-doc/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api-doc";
                c.SwaggerEndpoint("v1/swagger.json", "WaterCloud Api v1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "api/{controller=ApiHome}/{action=Index}/{id?}");
            });
        }
    }
}
