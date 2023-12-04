using Autofac;
using CSRedis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WaterCloud.Code.Model;

namespace WaterCloud.Code
{
	public class DefaultStartUp
	{
		protected IConfiguration Configuration { get; }
		protected IWebHostEnvironment WebHostEnvironment { get; set; }

		public DefaultStartUp(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			WebHostEnvironment = env;
			GlobalContext.LogWhenStart(env);
			GlobalContext.HostingEnvironment = env;
		}

		public virtual void ConfigureServices(IServiceCollection services)
		{
			GlobalContext.SystemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
			GlobalContext.Services = services;
			GlobalContext.Configuration = Configuration;
			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});
			//解决session问题
			services.AddSession(options =>
			{
				options.Cookie.IsEssential = true;//是否强制存储cookie
				options.Cookie.SameSite = SameSiteMode.None;
			});
			//代替HttpContext.Current
			services.AddHttpContextAccessor();
			//缓存缓存选择
			if (GlobalContext.SystemConfig.CacheProvider != Define.CACHEPROVIDER_REDIS)
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
			services.AddCors(options => options.AddPolicy("CorsPolicy",
			builder =>
			{
				builder.AllowAnyMethod().AllowAnyHeader()
					   .WithOrigins(GlobalContext.SystemConfig.AllowCorsSite.Split(","))
					   .AllowCredentials();
			}));
			services.AddOptions();
			services.AddHttpClient();
			services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");
			services.AddControllersWithViews().AddControllersAsServices();
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
				options.Level = CompressionLevel.SmallestSize; 
			});
			services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + "DataProtection"));
		}

		/// <summary>
		/// Autofac配置
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="projects">扫描程序</param>
		/// <param name="controller">控制器</param>
		/// <param name="IService">服务接口</param>
		/// <param name="program">程序</param>
		public void AutofacConfigureContainer(ContainerBuilder builder, List<string> projects, Type controller, Type IService, Type program)
		{
			if (projects == null)
			{
				projects = new List<string>();
			}
            var baseType = IService;//IDenpendency 是一个接口（所有要实现依赖注入的借口都要继承该接口）
            var controllerBaseType = controller;
            foreach (var item in projects)
			{
				var assemblys = Assembly.Load(item);//Service是继承接口的实现方法类库名称
				builder.RegisterAssemblyTypes(assemblys).Where(m => baseType.IsAssignableFrom(m) && m != baseType)
				  .InstancePerLifetimeScope()//生命周期，这里没有使用接口方式
				  .PropertiesAutowired();//属性注入
                //插件Controller中使用属性注入
                builder.RegisterAssemblyTypes(assemblys)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();
            }
			//Controller中使用属性注入
			builder.RegisterAssemblyTypes(program.Assembly)
			.Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
			.PropertiesAutowired();
			//注册html解析
			builder.RegisterInstance(HtmlEncoder.Create(UnicodeRanges.All)).SingleInstance();
		}

		public virtual void Configure(IApplicationBuilder app)
		{
            GlobalContext.RootServices = app.ApplicationServices;
            //实时通讯跨域
            app.UseCors("CorsPolicy");
			if (WebHostEnvironment.IsDevelopment())
			{
				GlobalContext.SystemConfig.Debug = true;
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseDeveloperExceptionPage();
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
			app.Use(async (context, next) =>
			{
				context.Request.EnableBuffering();
                // 执行下一个中间件
                await next.Invoke();
                // 释放所有未托管的服务提供器
                GlobalContext.DisposeUnmanagedObjects();
			});
			//session
			app.UseSession();
			//路径
			app.UseRouting();
		}
	}

	/// <summary>
	/// StartUp扩展
	/// </summary>
	public static class StartUpExtends
	{
		/// <summary>
		/// 默认MVC配置
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IMvcBuilder AddDefaultMVC(this IServiceCollection services)
		{
			return services.AddControllersWithViews(options =>
			{
				options.Filters.Add<GlobalExceptionFilter>();
				options.Filters.Add<ModelActionFilter>();
				options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
				//options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
			});
		}

		/// <summary>
		/// 默认API配置
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IMvcBuilder AddDefaultAPI(this IServiceCollection services)
		{
			services.AddDirectoryBrowser();
			return services.AddControllers(options =>
			{
				options.Filters.Add<ModelActionFilter>();
				options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
			}).ConfigureApiBehaviorOptions(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});
		}

		/// <summary>
		/// 默认API配置
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddDefaultSwaggerGen(this IServiceCollection services, string name)
		{
			services.AddSwaggerGen(config =>
			{
				foreach (var item in GlobalContext.SystemConfig.DocumentSettings.GroupOpenApiInfos)
				{
					config.SwaggerDoc($"{item.Group}", new OpenApiInfo { Title = item.Title, Version = item.Version, Description = item.Description });
				}
				var xmlFile = $"{name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				config.IncludeXmlComments(xmlPath, true); //添加控制器层注释（true表示显示控制器注释）
				config.AddSecurityDefinition(GlobalContext.SystemConfig.TokenName, new OpenApiSecurityScheme
				{
					Description = "header token",
					Name = GlobalContext.SystemConfig.TokenName,
					In = ParameterLocation.Header,
					Scheme = "",
					Type = SecuritySchemeType.ApiKey,//设置类型
					BearerFormat = ""
				});
				config.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = GlobalContext.SystemConfig.TokenName }
						},
						new List<string>()
					}
				});
			});
			return services;
		}

		/// <summary>
		/// 默认API配置
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IApplicationBuilder AddDefaultSwaggerGen(this IApplicationBuilder app)
		{
			app.UseSwagger(c =>
			{
				c.RouteTemplate = "api-doc/{documentName}/swagger.json";
			});
			app.UseSwaggerUI(c =>
			{
				c.RoutePrefix = "api-doc";
				foreach (var item in GlobalContext.SystemConfig.DocumentSettings.GroupOpenApiInfos)
				{
					c.SwaggerEndpoint($"{item.Group}/swagger.json", $"{item.Title}");
				}
			});
			return app;
		}

		/// <summary>
		/// 注入RabbitMq
		/// </summary>
		/// <param name="this"></param>
		/// <param name="configuration">json配置</param>
		/// <param name="lifeTime">生命周期，默认：单例模式</param>
		/// <returns></returns>
		public static IServiceCollection AddRabbitMq(
			this IServiceCollection @this,
			ServiceLifetime lifeTime = ServiceLifetime.Singleton)
		{
			if (GlobalContext.SystemConfig.RabbitMq.Enabled == false)
				return @this;

			switch (lifeTime)
			{
				case ServiceLifetime.Singleton:
					@this.AddSingleton(x => new RabbitMqHelper(GlobalContext.SystemConfig.RabbitMq));
					break;

				case ServiceLifetime.Scoped:
					@this.AddScoped(x => new RabbitMqHelper(GlobalContext.SystemConfig.RabbitMq));
					break;

				case ServiceLifetime.Transient:
					@this.AddTransient(x => new RabbitMqHelper(GlobalContext.SystemConfig.RabbitMq));
					break;

				default:
					break;
			}
			return @this;
		}

		#region AddWorkerService

		/// <summary>
		/// 自动注入 继承 BackgroundService 的后台服务
		/// </summary>
		/// <param name="this"></param>
		/// <returns></returns>
		public static IServiceCollection AddWorkerService(
			this IServiceCollection @this)
		{
			var ret = new List<Type>();
			var assemblies = Directory.GetFiles(AppContext.BaseDirectory, "*.dll")
				.Select(x => x.Substring(@"\").Substring(@"/").Replace(".dll", ""))
				.Select(x => Assembly.Load(x)).ToArray();
			//排除列表
			var ignoreList= new List<string>{ "EventBusHostedService" };
			foreach (var item in assemblies)
			{
				ret.AddRange(item.GetTypes() //获取当前类库下所有类型
				 .Where(t => typeof(BackgroundService).IsAssignableFrom(t)) //获取间接或直接继承t的所有类型
				 .Where(t => !t.IsAbstract && t.IsClass && !ignoreList.Contains(t.Name)));//获取非抽象类 排除接口继承
			}
			foreach (var item in ret)
			{
				@this.AddTransient(typeof(IHostedService), item);
			}
			return @this;
		}

		#endregion AddWorkerService

		#region AddIf

		/// <summary>
		/// 根据条件注入服务
		/// </summary>
		/// <param name="this"></param>
		/// <param name="condition"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static IServiceCollection AddIf(
			this IServiceCollection @this,
			bool condition,
			Action<IServiceCollection> action)
		{
			if (condition && action != null)
				action(@this);

			return @this;
		}

		#endregion AddIf
	}
}