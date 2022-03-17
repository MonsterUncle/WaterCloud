using CSRedis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
                options.Level = (CompressionLevel)4; // 4 or 5 is OK
            });
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + "DataProtection"));

        }
        public virtual void Configure(IApplicationBuilder app)
        {
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
            //session
            app.UseSession();
            //路径
            app.UseRouting();
            GlobalContext.ServiceProvider = app.ApplicationServices;
        }
    }

}
