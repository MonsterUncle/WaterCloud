using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddDefaultSwaggerGen(Assembly.GetExecutingAssembly().GetName().Name);
            services.AddSqlSugar()
                    .AddRabbitMq();
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
            app.AddDefaultSwaggerGen();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "api/{controller=ApiHome}/{action=Index}/{id?}");
            });
        }
    }
}
