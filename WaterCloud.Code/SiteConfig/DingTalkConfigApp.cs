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
   /// 钉钉平台配置读写操作
   /// </summary>
    public partial class DingTalkConfigApp
    {
        private static object lockHelper = new object();
        ///// <summary>
        /////  读取配置文件（MVC模式下已弃用）
        ///// </summary>
        //public DingTalkConfig LoadConfig()
        //{
        //    DingTalkConfig model = CacheFactory.CaChe().Read<DingTalkConfig>(SysKeys.CACHE_DINGTALK_CONFIG, CacheId.SiteConfig);
        //    if (model == null)
        //    {
        //        string path = Utils.GetXmlMapPath(SysKeys.FILE_DINGTALK_XML_CONFING);
        //        CacheHelper.Insert(SysKeys.CACHE_SYS_CONFIG, LoadConfig(path),
        //            Utils.GetXmlMapPath(SysKeys.FILE_DINGTALK_XML_CONFING));
        //        model= CacheFactory.CaChe().Read<DingTalkConfig>(SysKeys.CACHE_SYS_CONFIG, CacheId.SiteConfig);
        //    }
        //    return model;
        //}

        /// <summary>
        ///  读取配置文件
        /// </summary>
        public DingTalkConfig LoadConfig()
        {
            DingTalkConfig model = CacheFactory.CaChe().Read<DingTalkConfig>(SysKeys.CACHE_DINGTALK_CONFIG, CacheId.siteConfig);
            if (model == null)
            {
                string path = Utils.GetXmlMapPath(SysKeys.FILE_DINGTALK_XML_CONFING);
                model = LoadConfig(path);
                CacheFactory.CaChe().Write(SysKeys.CACHE_DINGTALK_CONFIG, model, CacheId.siteConfig);
            }
            return model;
        }

        /// <summary>
        ///  保存配置文件
        /// </summary>
        public DingTalkConfig SaveConifg(DingTalkConfig model)
        {
            return SaveConifg(model, Utils.GetXmlMapPath(SysKeys.FILE_DINGTALK_XML_CONFING));
        }
        
        /// <summary>
        ///  读取站点配置文件
        /// </summary>
        private DingTalkConfig LoadConfig(string configFilePath)
        {
            return (DingTalkConfig)SerializationHelper.Load(typeof(DingTalkConfig), configFilePath);
        }

        /// <summary>
        /// 写入站点配置文件
        /// </summary>
        private DingTalkConfig SaveConifg(DingTalkConfig model, string configFilePath)
        {
            lock (lockHelper)
            {
                SerializationHelper.Save(model, configFilePath);
            }
            return model;
        }
    }
}
