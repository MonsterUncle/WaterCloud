using System;
using System.Collections.Generic;
using System.Text;

namespace WaterCloud.Code
{
    public class SysKeys
    {
        //系统版本
        /// <summary>
        /// 版本号全称
        /// </summary>
        public const string ASSEMBLY_VERSION = "1.0.0";
        /// <summary>
        /// 版本年号
        /// </summary>
        public const string ASSEMBLY_YEAR = "2014";
        //File======================================================
        /// <summary>
        /// 插件配制文件名
        /// </summary>
        public const string FILE_PLUGIN_XML_CONFING = "plugin.config";
        /// <summary>
        /// 站点配置文件名
        /// </summary>
        public const string FILE_SYS_XML_CONFING = "ConfigPath";
        /// <summary>
        /// 钉钉平台配置文件名
        /// </summary>
        public const string FILE_DINGTALK_XML_CONFING = "DingTalkPath";
        /// <summary>
        /// 微信平台配置文件名
        /// </summary>
        public const string FILE_WEIXINMP_XML_CONFING = "WeiXinPath";

        //Cache======================================================
        /// <summary>
        /// 站点配置
        /// </summary>
        public const string CACHE_SYS_CONFIG = "dt_cache_sys_config";
        /// <summary>
        /// 钉钉配置
        /// </summary>
        public const string CACHE_DINGTALK_CONFIG = "dt_cache_dingtalk_config";
        /// <summary>
        /// 微信配置
        /// </summary>
        public const string CACHE_WEIXINMP_CONFIG = "dt_cache_weixin_config";
        /// HttpModule映射类
        /// </summary>
        public const string CACHE_SITE_HTTP_MODULE = "WaterCloud_cache_http_module";
        /// <summary>
        /// 绑定域名
        /// </summary>
        public const string CACHE_SITE_HTTP_DOMAIN = "WaterCloud_cache_http_domain";
        /// URL重写映射表
        /// </summary>
        public const string CACHE_SITE_URLS = "WaterCloud_cache_site_urls";
        /// <summary>
        /// URL重写LIST列表
        /// </summary>
        public const string CACHE_SITE_URLS_LIST = "WaterCloud_cache_site_urls_list";
        /// <summary>
        /// 升级通知
        /// </summary>
        public const string CACHE_OFFICIAL_UPGRADE = "WaterCloud_official_upgrade";
        /// <summary>
        /// 官方消息
        /// </summary>
        public const string CACHE_OFFICIAL_NOTICE = "WaterCloud_official_notice";

        //Session=====================================================
        /// <summary>
        /// 网页验证码
        /// </summary>
        public const string SESSION_CODE = "WaterCloud_session_code";
        /// <summary>
        /// 短信验证码
        /// </summary>
        public const string SESSION_SMS_CODE = "WaterCloud_session_sms_code";
        /// <summary>
        /// 后台管理员
        /// </summary>
        public const string SESSION_ADMIN_INFO = "WaterCloud_session_admin_info";

        //Cookies=====================================================

        public const string COOKIE_CODE = "WaterCloud_cookie_code";
        /// <summary>
        /// 防重复顶踩KEY
        /// </summary>
        public const string COOKIE_DIGG_KEY = "WaterCloud_cookie_digg_key";
        /// <summary>
        /// 防重复评论KEY
        /// </summary>
        public const string COOKIE_COMMENT_KEY = "WaterCloud_cookie_comment_key";
        /// <summary>
        /// 返回上一页
        /// </summary>
        public const string COOKIE_URL_REFERRER = "WaterCloud_cookie_url_referrer";

        /// <summary>
        /// 用户微信信息
        /// </summary>
        public const string SESSION_WXUSER_INFO = "WaterCloud_session_wxuserinfo";
        /// <summary>
        /// 用户微信OpenId
        /// </summary>
        public const string SESSION_OPENID = "WaterCloud_session_openid";
        /// <summary>
        /// 记住微信用户信息
        /// </summary>
        public const string COOKIE_WXUSER_INFO = "WaterCloud_cookie_wxuserinfo";
        /// <summary>
        /// 用户微信OpenId
        /// </summary>
        public const string COOKIE_OPENID = "WaterCloud_cookie_openid";

    }
}
