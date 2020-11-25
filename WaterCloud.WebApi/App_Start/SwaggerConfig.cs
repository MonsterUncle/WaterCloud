using System.Web.Http;
using WebActivatorEx;
using Swashbuckle.Application;
using System.Linq;
using System.Reflection;
using WaterCloud.WebAPI;
using System;
using Swashbuckle.Swagger;
using System.Web.Http.Description;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WaterCloud.WebAPI
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var xmlFile = string.Format("{0}/bin/WaterCloud.WebAPI.XML", System.AppDomain.CurrentDomain.BaseDirectory);
            GlobalConfiguration.Configuration.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "WaterCloudWebAPI");
                c.IncludeXmlComments(xmlFile);
                c.DocumentFilter<HiddenApiFilter>(); //隐藏Swagger 自带API及隐藏具体Api
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.CustomProvider((defaultProvider) => new SwaggerControllerDescProvider(defaultProvider, xmlFile));
            })
           //修改访问地址为doc/index
           //注入自定义的js文件
           .EnableSwaggerUi("doc/{*assetPath}", b => b.InjectJavaScript(Assembly.GetExecutingAssembly(), "WaterCloud.WebAPI.Scripts.Swagger-Custom.js"));
        }
    }
}
/// <summary>
/// 隐藏Swagger默认接口
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public partial class HiddenApiAttribute : Attribute { }
/// <summary>
/// 隐藏Swagger默认接口
/// </summary>
public class HiddenApiFilter : IDocumentFilter
{
    public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
    {
        foreach (ApiDescription apiDescription in apiExplorer.ApiDescriptions)
        {
            var _key = "/" + apiDescription.RelativePath.TrimEnd('/');
            // 过滤 swagger 自带的接口
            if (_key.Contains("/api/Swagger") && swaggerDoc.paths.ContainsKey(_key))
                swaggerDoc.paths.Remove(_key);

            //隐藏具体Api接口 需要在想隐藏的api 上面添加特性[HiddenApi]
            if (Enumerable.OfType<HiddenApiAttribute>(apiDescription.GetControllerAndActionAttributes<HiddenApiAttribute>()).Any())
            {
                string key = "/" + apiDescription.RelativePath;
                if (key.Contains("?"))
                {
                    int idx = key.IndexOf("?", System.StringComparison.Ordinal);
                    key = key.Substring(0, idx);
                }
                swaggerDoc.paths.Remove(key);
            }
        }
    }
}