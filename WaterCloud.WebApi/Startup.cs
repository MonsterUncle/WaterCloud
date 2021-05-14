using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using CSRedis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using WaterCloud.Code;
using WaterCloud.Code.Model;
using WaterCloud.Service;
using SqlSugar;
using WaterCloud.DataBase;
using System.Collections.Generic;

namespace WaterCloud.WebApi
{
	public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            GlobalContext.LogWhenStart(env);
            GlobalContext.HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "WaterCloud Api", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath, true); //添加控制器层注释（true表示显示控制器注释）                
            });
            //缓存选择
            if (Configuration.GetSection("SystemConfig:CacheProvider").Value != Define.CACHEPROVIDER_REDIS)
            {
                services.AddMemoryCache();
            }
            else
            {
                //redis 注入服务
                string redisConnectiong = Configuration.GetSection("SystemConfig:RedisConnectionString").Value;
                // 多客户端 1、基础 2、操作日志
                var redisDB1 = new CSRedisClient(redisConnectiong + ",defaultDatabase=" + 0);
                BaseHelper.Initialization(redisDB1);
                var redisDB2 = new CSRedisClient(redisConnectiong + ",defaultDatabase=" + 1);
                HandleLogHelper.Initialization(redisDB2);
                services.AddSingleton(redisDB1);
                services.AddSingleton(redisDB2);
            }
            //连续guid初始化,示例IDGenerator.NextId()
            services.AddSingleton<IDistributedIDGenerator, SequentialGuidIDGenerator>();

            #region 数据库模式
            List<ConnectionConfig> list = new List<ConnectionConfig>();
            var defaultConfig = DBContexHelper.Contex(Configuration.GetSection("SystemConfig:DBConnectionString").Value, Configuration.GetSection("SystemConfig:DBProvider").Value);
            defaultConfig.ConfigId = "0";
            list.Add(defaultConfig);
            if (Configuration.GetSection("SystemConfig:SqlMode").Value == "TenantSql")
            {
                try
                {
                    using (var context = new UnitOfWork(new SqlSugarClient(defaultConfig)))
                    {
                        var _setService = new Service.SystemOrganize.SystemSetService(context);
                        var sqls = _setService.GetList().GetAwaiter().GetResult();
                        foreach (var item in sqls.Where(a => a.F_EnabledMark == true && a.F_EndTime > DateTime.Now.Date && a.F_DbNumber != "0"))
                        {
                            var config = DBContexHelper.Contex(item.F_DbString, item.F_DBProvider);
                            config.ConfigId = item.F_DbNumber;
                            list.Add(config);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Write(ex);
                }
            }
            else
            {
                try
                {
                    var configs = Configuration.GetSection("SystemConfig:SqlConfig");
                    for (int i = 1; i < 9999; i++)
                    {
                        if (!configs.GetSection(i.ToString()).Exists())
                        {
                            break;
                        }
                        var config = DBContexHelper.Contex(configs.GetSection(i.ToString()).GetSection("DBConnectionString").Value, configs.GetSection(i.ToString()).GetSection("DBProvider").Value);
                        config.ConfigId = i.ToString();
                        list.Add(config);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Write(ex);
                }
            }
            #endregion

            //注入数据库连接
            // 注册 SqlSugar 客户端
            services.AddScoped<ISqlSugarClient>(u =>
            {
                return new SqlSugarClient(list);
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //代替HttpContext.Current
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddOptions();
            services.AddDirectoryBrowser();
            //跨域
            services.AddCors();
            services.AddControllers(options =>
            {
                options.Filters.Add<ModelActionFilter>();
                options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddControllers().AddControllersAsServices();
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + "DataProtection"));
            GlobalContext.SystemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
            GlobalContext.Services = services;
            GlobalContext.Configuration = Configuration;
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assemblys = Assembly.Load("WaterCloud.Service");//Service是继承接口的实现方法类库名称
            var baseType = typeof(IDenpendency);//IDenpendency 是一个接口（所有要实现依赖注入的借口都要继承该接口）
            builder.RegisterAssemblyTypes(assemblys).Where(m => baseType.IsAssignableFrom(m) && m != baseType)
              .InstancePerLifetimeScope()//生命周期
              .PropertiesAutowired();//属性注入
            //ControllerBase中使用属性注入
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
            .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
            .PropertiesAutowired();
            //注册特性
            builder.RegisterType(typeof(AuthorizeFilterAttribute)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(LoginFilterAttribute)).InstancePerLifetimeScope();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                GlobalContext.SystemConfig.Debug = true;
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = GlobalContext.SetCacheControl
            });
            app.UseMiddleware(typeof(GlobalExceptionMiddleware));
            //跨域设置
            app.UseCors(builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            //app.UseCors(builder =>
            //{
            //    builder.WithOrigins(GlobalContext.SystemConfig.AllowCorsSite.Split(',')).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            //});
            //允许body重用
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
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "api/{controller=ApiHome}/{action=Index}/{id?}");
            });
            GlobalContext.ServiceProvider = app.ApplicationServices;
        }
    }
}
