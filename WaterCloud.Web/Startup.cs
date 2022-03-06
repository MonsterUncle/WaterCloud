using CSRedis;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WaterCloud.Code;
using WaterCloud.Code.Model;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Newtonsoft.Json.Serialization;
using WaterCloud.Service.AutoJob;
using WaterCloud.DataBase;
using System.Reflection;
using System.Linq;
using WaterCloud.Service;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using SqlSugar;
using System.Collections.Generic;
using WaterCloud.Domain.SystemOrganize;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Quartz.Impl.AdoJobStore.Common;
using System.Collections.Specialized;
using MySql.Data.MySqlClient;

namespace WaterCloud.Web
{
	public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;
            GlobalContext.LogWhenStart(env);
            GlobalContext.HostingEnvironment = env;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            GlobalContext.SystemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //解决session问题
            services.AddSession(options =>
            {
                // Make the session cookie essential
                options.Cookie.IsEssential = true;//是否强制存储cookie
                options.Cookie.SameSite = SameSiteMode.None;
            });
            //代替HttpContext.Current
            services.AddHttpContextAccessor();
            //缓存缓存选择
            if (GlobalContext.SystemConfig.CacheProvider!= Define.CACHEPROVIDER_REDIS)
            {
                services.AddMemoryCache();
            }
            else
            {
                //redis 注入服务
                string redisConnectiong = GlobalContext.SystemConfig.RedisConnectionString;
                // 多客户端 1、基础 2、操作日志
                var redisDB1 = new CSRedisClient(redisConnectiong + ",defaultDatabase=" + 0);
                BaseHelper.Initialization(redisDB1);
                var redisDB2 = new CSRedisClient(redisConnectiong + ",defaultDatabase=" + 1);
                HandleLogHelper.Initialization(redisDB2);
                services.AddSingleton(redisDB1);
                services.AddSingleton(redisDB2);
            }
            //连续guid初始化,示例IDGen.NextId()
            services.AddSingleton<IDistributedIDGenerator, SequentialGuidIDGenerator>();
			#region 依赖注入
			//注入数据库连接
			// 注册 SqlSugar
			services.AddScoped<ISqlSugarClient>(u =>
            {
                var db = new SqlSugarClient(DBInitialize.GetConnectionConfigs());
                DBInitialize.GetConnectionConfigs().ForEach(config => {
                    db.GetConnection(config.ConfigId);
                    db.Ado.CommandTimeOut = GlobalContext.SystemConfig.CommandTimeout;
                    db.CurrentConnectionConfig.ConfigureExternalServices = new ConfigureExternalServices()
                    {
                        DataInfoCacheService = new SqlSugarCache() //配置我们创建的缓存类
                    };
                    db.Aop.OnLogExecuted = (sql, pars) => //SQL执行完
                    {
                        if (sql.StartsWith("SELECT"))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("[SELECT]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        }
                        if (sql.StartsWith("INSERT"))
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("[INSERT]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        }
                        if (sql.StartsWith("UPDATE"))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("[UPDATE]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                        }
                        if (sql.StartsWith("DELETE"))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[DELETE]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        }
                        Console.WriteLine("NeedTime-" + db.Ado.SqlExecutionTime.ToString());
                        //App.PrintToMiniProfiler("SqlSugar", "Info", sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                        Console.WriteLine("Content:" + SqlProfiler.ParameterFormat(sql, pars));
                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("");
                    };
                });
                return db;
            });
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            #region 注入 Quartz调度类
            services.AddSingleton<JobExecute>();
            //注册ISchedulerFactory的实例。
            services.AddSingleton<IJobFactory, IOCJobFactory>();
            if (Configuration.GetSection("SystemConfig:IsCluster").Value != "True")
            {
                services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            }
			else
			{
				//开启集群模式,具体数据库从官方github下载
				//https://github.com/quartznet/quartznet/blob/main/database/tables/
				services.AddSingleton<ISchedulerFactory>(u =>
				{
                    //当前是mysql的例子，其他数据库做相应更改
					DbProvider.RegisterDbMetadata("mysql-custom", new DbMetadata()
					{
						AssemblyName = typeof(MySqlConnection).Assembly.GetName().Name,
						ConnectionType = typeof(MySqlConnection),
						CommandType = typeof(MySqlCommand),
						ParameterType = typeof(MySqlParameter),
						ParameterDbType = typeof(DbType),
						ParameterDbTypePropertyName = "DbType",
						ParameterNamePrefix = "@",
						ExceptionType = typeof(MySqlException),
						BindByName = true
					});
					var properties = new NameValueCollection
					{
						["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz", // 配置Quartz以使用JobStoreTx
																								 //["quartz.jobStore.useProperties"] = "true", // 配置AdoJobStore以将字符串用作JobDataMap值
						["quartz.jobStore.dataSource"] = "myDS", // 配置数据源名称
						["quartz.jobStore.tablePrefix"] = "QRTZ_", // quartz所使用的表，在当前数据库中的表前缀
						["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.MySQLDelegate, Quartz",  // 配置AdoJobStore使用的DriverDelegate
						["quartz.dataSource.myDS.connectionString"] = Configuration.GetSection("SystemConfig:DBConnectionString").Value, // 配置数据库连接字符串，自己处理好连接字符串，我这里就直接这么写了
						["quartz.dataSource.myDS.provider"] = "mysql-custom", // 配置数据库提供程序（这里是自定义的，定义的代码在上面）
						["quartz.jobStore.lockHandler.type"] = "Quartz.Impl.AdoJobStore.UpdateLockRowSemaphore, Quartz",
						["quartz.serializer.type"] = "json",
						["quartz.jobStore.clustered"] = "true",    //  指示Quartz.net的JobStore是应对 集群模式
						["quartz.scheduler.instanceId"] = "AUTO"
					};
					return new StdSchedulerFactory(properties);
				});
			}
            //是否开启后台任务
            if (Configuration.GetSection("SystemConfig:OpenQuarz").Value == "True")
            {
                services.AddHostedService<JobCenter>();
            }
            #endregion
            //注入SignalR实时通讯，默认用json传输
            services.AddSignalR(options =>
            {
                //客户端发保持连接请求到服务端最长间隔，默认30秒，改成4分钟，网页需跟着设置connection.keepAliveIntervalInMilliseconds = 12e4;即2分钟
                options.ClientTimeoutInterval = TimeSpan.FromMinutes(4);
                //服务端发保持连接请求到客户端间隔，默认15秒，改成2分钟，网页需跟着设置connection.serverTimeoutInMilliseconds = 24e4;即4分钟
                options.KeepAliveInterval = TimeSpan.FromMinutes(2);
            });
            ////注册html解析
            //services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            ////注册特性
            //services.AddScoped<HandlerLoginAttribute>();
            //services.AddScoped<HandlerAuthorizeAttribute>();
            ////ajax不能使用注入
            ////services.AddScoped<HandlerAjaxOnlyAttribute>();
            //services.AddScoped<HandlerAdminAttribute>();
            //////定时任务（已废除）
            ////services.AddBackgroundServices();
            #endregion
            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                       .WithOrigins(Configuration.GetSection("SystemConfig:AllowCorsSite").Value.Split(","))
                       .AllowCredentials();
            }));
            services.AddHttpClient();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<ModelActionFilter>();
                options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).AddNewtonsoftJson(options =>
            {
                // 返回数据首字母不小写，CamelCasePropertyNamesContractResolver是小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");
            services.AddControllersWithViews().AddControllersAsServices();
            //调试前端可更新
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            //启用 Gzip 和 Brotil 压缩功能
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes =
                    ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { "image/svg+xml" });
            });

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = (CompressionLevel)4; // 4 or 5 is OK
            });
            services.AddOptions();
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + "DataProtection"));
            GlobalContext.Services = services;
            GlobalContext.Configuration = Configuration;
            DBInitialize.ReviseSuperSysem();
            //清理缓存
            //CacheHelper.FlushAll().GetAwaiter().GetResult();
        }
        //AutoFac注入
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assemblys = Assembly.Load("WaterCloud.Service");//Service是继承接口的实现方法类库名称
            var baseType = typeof(IDenpendency);//IDenpendency 是一个接口（所有要实现依赖注入的借口都要继承该接口）
            builder.RegisterAssemblyTypes(assemblys).Where(m => baseType.IsAssignableFrom(m) && m != baseType)
              .InstancePerLifetimeScope()//生命周期，这里没有使用接口方式
              .PropertiesAutowired() ;//属性注入
            //Controller中使用属性注入
            var controllerBaseType = typeof(Controller);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
            .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
            .PropertiesAutowired();

            ////注入redis
            //if (Configuration.GetSection("SystemConfig:CacheProvider").Value== Define.CACHEPROVIDER_REDIS)
            //{
            //    //redis 注入服务
            //    string redisConnectiong = Configuration.GetSection("SystemConfig:RedisConnectionString").Value;
            //    // 多客户端
            //    var redisDB = new CSRedisClient(redisConnectiong + ",defaultDatabase=" + 0);
            //    RedisHelper.Initialization(redisDB);
            //    builder.RegisterInstance(redisDB).SingleInstance();//生命周期只能单例
            //}
            //注册html解析
            builder.RegisterInstance(HtmlEncoder.Create(UnicodeRanges.All)).SingleInstance();
            //注册特性
            builder.RegisterType(typeof(HandlerLoginAttribute)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(HandlerAuthorizeAttribute)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(HandlerLockAttribute)).InstancePerLifetimeScope();
            ////注册ue编辑器
            //Config.ConfigFile = "config.json";
            //Config.noCache = true;
            //var actions = new UEditorActionCollection();
            //builder.RegisterInstance(actions).SingleInstance();
            //builder.RegisterInstance(typeof(UEditorService)).SingleInstance();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            //由于.Net Core默认只会从wwwroot目录加载静态文件，其他文件夹的静态文件无法正常访问。
            //而我们希望将图片上传到网站根目录的upload文件夹下，所以需要额外在Startup.cs类的Configure方法中
            //string resource = Path.Combine(Directory.GetCurrentDirectory(), "upload");
            //if (!FileHelper.IsExistDirectory(resource))
            //{
            //    FileHelper.CreateFolder(resource);
            //}
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(resource),
            //    RequestPath = "/upload",
            //    OnPrepareResponse = ctx =>
            //    {
            //        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
            //    }
            //});
            //虚拟目录 
            //如需使用，所有URL修改，例："/Home/Index"改成'@Url.Content("~/Home/Index")'，部署访问首页必须带虚拟目录;
            //if (!string.IsNullOrEmpty(GlobalContext.SystemConfig.VirtualDirectory))
            //{
            //    app.UsePathBase(new PathString(GlobalContext.SystemConfig.VirtualDirectory)); // 让 Pathbase 中间件成为第一个处理请求的中间件， 才能正确的模拟虚拟路径
            //}
            //实时通讯跨域
            app.UseCors("CorsPolicy");
            if (WebHostEnvironment.IsDevelopment())
            {
                GlobalContext.SystemConfig.Debug = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error?msg=404");
            }
            //文件地址Resource
            //静态资源wwwroot
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = new CustomerFileExtensionContentTypeProvider(),
                OnPrepareResponse = GlobalContext.SetCacheControl
            });
            //启用 Gzip 和 Brotil 压缩功能
            app.UseResponseCompression();
            //session
            app.UseSession();
            //路径
            app.UseRouting();
            //MVC路由
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/chatHub");
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Login}/{action=Index}/{id?}");
            });
            GlobalContext.ServiceProvider = app.ApplicationServices;
        }
    }
}
