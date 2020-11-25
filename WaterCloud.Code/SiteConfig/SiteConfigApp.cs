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
   /// 系统站点配置读写操作
   /// </summary>
    public partial class SiteConfigApp
    {
        private static object lockHelper = new object();
        /// <summary>
        ///  读取配置文件（MVC模式下已弃用）
        /// </summary>
        //public SiteConfig LoadConfig()
        //{
        //    SiteConfig model = CacheHelper.Get<SiteConfig>(SysKeys.CACHE_SYS_CONFIG);
        //    if (model == null)
        //    {
        //        string path = Utils.GetXmlMapPath(SysKeys.FILE_SYS_XML_CONFING);
        //        CacheHelper.Insert(SysKeys.CACHE_SYS_CONFIG, LoadConfig(path),
        //            Utils.GetXmlMapPath(SysKeys.FILE_SYS_XML_CONFING));
        //        model = CacheHelper.Get<SiteConfig>(SysKeys.CACHE_SYS_CONFIG);
        //    }
        //    return model;
        //}

        /// <summary>
        ///  读取配置文件
        /// </summary>
        public SiteConfig LoadConfig()
        {
            SiteConfig model = CacheFactory.CaChe().Read<SiteConfig>(SysKeys.CACHE_SYS_CONFIG, CacheId.siteConfig);
            if (model == null)
            {
                string path = Utils.GetXmlMapPath(SysKeys.FILE_SYS_XML_CONFING);
                model = LoadConfig(path);
                CacheFactory.CaChe().Write(SysKeys.CACHE_SYS_CONFIG,model, CacheId.siteConfig);
            }
            return model;
        }

        /// <summary>
        ///  保存配置文件
        /// </summary>
        public SiteConfig SaveConifg(SiteConfig model)
        {
            return SaveConifg(model, Utils.GetXmlMapPath(SysKeys.FILE_SYS_XML_CONFING));
        }
        
        /// <summary>
        ///  读取站点配置文件
        /// </summary>
        private SiteConfig LoadConfig(string configFilePath)
        {
            return (SiteConfig)SerializationHelper.Load(typeof(SiteConfig), configFilePath);
        }

        /// <summary>
        /// 写入站点配置文件
        /// </summary>
        private SiteConfig SaveConifg(SiteConfig model, string configFilePath)
        {
            lock (lockHelper)
            {
                SerializationHelper.Save(model, configFilePath);
            }
            return model;
        }
    }
}
