using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace WaterCloud.Code.SysConfig
{
   /// <summary>
   /// 微信平台配置读写操作
   /// </summary>
    public partial class WeixinConfigApp
    {
        private static object lockHelper = new object();
        /// <summary>
        ///  读取配置文件（MVC模式下已弃用）
        /// </summary>
        //public WeixinConfig LoadConfig()
        //{
        //    WeixinConfig model = CacheHelper.Get<WeixinConfig>(SysKeys.CACHE_WEIXINMP_CONFIG);
        //    if (model == null)
        //    {
        //        string path = Utils.GetXmlMapPath(SysKeys.FILE_WEIXINMP_XML_CONFING);
        //        CacheHelper.Insert(SysKeys.CACHE_SYS_CONFIG, LoadConfig(path),
        //            Utils.GetXmlMapPath(SysKeys.FILE_WEIXINMP_XML_CONFING));
        //        model = CacheHelper.Get<WeixinConfig>(SysKeys.CACHE_SYS_CONFIG);
        //    }
        //    return model;
        //}

        /// <summary>
        ///  读取配置文件
        /// </summary>
        public WeixinConfig LoadConfig()
        {
            WeixinConfig model = CacheFactory.CaChe().Read<WeixinConfig>(SysKeys.CACHE_WEIXINMP_CONFIG, CacheId.siteConfig);
            if (model == null)
            {
                string path = Utils.GetXmlMapPath(SysKeys.FILE_WEIXINMP_XML_CONFING);
                model = LoadConfig(path);
                CacheFactory.CaChe().Write<WeixinConfig>(SysKeys.CACHE_WEIXINMP_CONFIG, model, CacheId.siteConfig);
            }
            return model;
        }

        /// <summary>
        ///  保存配置文件
        /// </summary>
        public WeixinConfig SaveConifg(WeixinConfig model)
        {
            return SaveConifg(model, Utils.GetXmlMapPath(SysKeys.FILE_WEIXINMP_XML_CONFING));
        }
        
        /// <summary>
        ///  读取站点配置文件
        /// </summary>
        private WeixinConfig LoadConfig(string configFilePath)
        {
            return (WeixinConfig)SerializationHelper.Load(typeof(WeixinConfig), configFilePath);
        }

        /// <summary>
        /// 写入站点配置文件
        /// </summary>
        private WeixinConfig SaveConifg(WeixinConfig model, string configFilePath)
        {
            lock (lockHelper)
            {
                SerializationHelper.Save(model, configFilePath);
            }
            return model;
        }
    }
}
