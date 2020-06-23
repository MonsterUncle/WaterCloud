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
using Chloe;
using Autofac;
using System.Reflection;
using System.Linq;
using WaterCloud.Service;

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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //区分缓存
            switch (Configuration.GetSection("SystemConfig:CacheProvider").Value)
            {
                case Define.CACHEPROVIDER_REDIS:
                    //redis 注入服务
                    string redisConnectiong = Configuration.GetSection("SystemConfig:RedisConnectionString").Value;
                    // 多客户端
                    var redisDB = new CSRedisClient(redisConnectiong + ",defaultDatabase=" + 0);
                    RedisHelper.Initialization(redisDB);
                    //注册服务
                    services.AddSingleton(redisDB);
                    break;
                case Define.CACHEPROVIDER_MEMORY:
                    services.AddMemoryCache();
                    break;
                default:
                    services.AddMemoryCache();
                    break;
            }
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.AddSession();
            //代替HttpContext.Current
            services.AddHttpContextAccessor();
            //注册服务
            services.AddDataService();

            //注册特性
            services.AddScoped<HandlerLoginAttribute>();
            services.AddScoped<HandlerAuthorizeAttribute>();
            //ajax不能使用注入
            //services.AddScoped<HandlerAjaxOnlyAttribute>();
            services.AddScoped<HandlerAdminAttribute>();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
            }).AddNewtonsoftJson(options =>
            {
                // 返回数据首字母不小写，CamelCasePropertyNamesContractResolver是小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            //注入数据库连接
            services.AddScoped<Chloe.IDbContext>((serviceProvider) =>
            {
                return DBContexHelper.Contex();
            });


            ////定时任务（已废除）
            //services.AddBackgroundServices();
            //调试前端可更新
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddOptions();
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + "DataProtection"));
            GlobalContext.SystemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
            GlobalContext.Services = services;
            GlobalContext.Configuration = Configuration;
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            //虚拟目录 
            //如需使用，所有URL修改，例："/Home/Index"改成'@Url.Content("~/Home/Index")'，部署访问首页必须带虚拟目录;
            //if (!string.IsNullOrEmpty(GlobalContext.SystemConfig.VirtualDirectory))
            //{
            //    app.UsePathBase(new PathString(GlobalContext.SystemConfig.VirtualDirectory)); // 让 Pathbase 中间件成为第一个处理请求的中间件， 才能正确的模拟虚拟路径
            //}
            if (WebHostEnvironment.IsDevelopment())
            {
                GlobalContext.SystemConfig.Debug = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //文件地址Resource
            //静态资源wwwroot
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = new CustomerFileExtensionContentTypeProvider(),
                OnPrepareResponse = GlobalContext.SetCacheControl
            });
            //session
            app.UseSession();
            //路径
            app.UseRouting();
            //MVC路由
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Login}/{action=Index}/{id?}");
            });
            GlobalContext.ServiceProvider = app.ApplicationServices;
            new JobCenter().Start(); // 使用Quartz定时任务
        }
    }
}
