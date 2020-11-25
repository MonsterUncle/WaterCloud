#define 使用RegisterServices方式注册微信

using WaterCloud.Code;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;
using System.Web.Http;
using WaterCloud.SystemTask;
#if 使用RegisterServices方式注册微信
/* 
 * 调试方式：当前模式为“使用RegisterServices方式注册微信”（推荐），
 * 如需使用原始（底层）注册方式，修改下方 #if 后的条件文字（如加上一个"0"），使之不成立即可。
 */
using Senparc.CO2NET;
using Senparc.CO2NET.Cache;
using Senparc.CO2NET.RegisterServices;
using Senparc.CO2NET.Utilities;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Containers;
using WaterCloud.Application.WeixinManage;
#endif

namespace WaterCloud.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 启动应用程序
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // 启动的时候清除全部缓存
            ICache cache = CacheFactory.CaChe();
            //for (int i = 0; i <= 6; i++)
            //{
            //    cache.RemoveAll(i);

            //}
            cache.RemoveAll(6);

            // 开启Log4Net日志配置
            LogFactory.LogFactoryConfig();

            //注册定时作业任务
            SystemTask.PublicSystemTask t = new SystemTask.PublicSystemTask();
            t.StartUp();

#if 使用RegisterServices方式注册微信
            ////设置全局 Debug 状态
            //var isGLobalDebug = false;
            ////全局设置参数，将被储存到 Senparc.CO2NET.Config.SenparcSetting
            //var senparcSetting = SenparcSetting.BuildFromWebConfig(isGLobalDebug);
            ////也可以通过这种方法在程序任意位置设置全局 Debug 状态：
            ////Senparc.CO2NET.Config.IsDebug = isGLobalDebug;

            ////CO2NET 全局注册，必须！！
            //IRegisterService register = RegisterService.Start(senparcSetting).UseSenparcGlobal();


            ////--------注册公众号--------------
            ////设置微信 Debug 状态
            //var isWeixinDebug = true;
            ////全局设置参数，将被储存到 Senparc.Weixin.Config.SenparcWeixinSetting
            //var senparcWeixinSetting = SenparcWeixinSetting.BuildFromWebConfig(isWeixinDebug);
            ////微信全局注册，必须！！
            //register.UseSenparcWeixin(senparcWeixinSetting, senparcSetting).RegisterMpAccount(senparcWeixinSetting, "我的公众号名称");
            ////可以在程序任意地方注册公众号或小程序：
            ////AccessTokenContainer.Register(weixin.T_AppId, weixin.T_AppSecret, weixin.T_WxName);
            //WxBaseConfigApp app = new WxBaseConfigApp();
            //var weixin = app.GetDefaultConfig();
            //AccessTokenContainer.RegisterAsync(weixin.AppId, weixin.AppSecret, weixin.WxName);
#endif

        }
    }
}