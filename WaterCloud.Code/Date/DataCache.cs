using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.IO;

namespace WaterCloud.Code
{
    /// <summary>  
    /// 网页中的缓存类,使用示例：  
    /// object obj = DataCache.GetCache("file1",depfile);  
    ///if (obj == null)  
    ///{  
    ///   string txt = "缓存内容";//从数据库或文件读取到的内容  
    ///   DataCache.SetCacheDepFile("file1", txt, depfile);  
    /// }  
    /// else  
    /// {  
    ///     string txt=obj.ToString();  
    /// }  
    /// </summary>  
    public partial class DataCache
    {
        #region 文件路径web.config  
        private static string _webconfigfile = string.Empty;
        /// <summary>  
        /// 文件路径web.config  
        /// </summary>  
        public static string webconfigfile
        {
            get
            {
                if (string.IsNullOrEmpty(_webconfigfile)) _webconfigfile = HttpContext.Current.Server.MapPath("/web.config");
                return _webconfigfile;
            }
        }
        #endregion

        #region 文件路径App_Data/ShopConfig.config  
        private static string _shopconfigfile = string.Empty;
        /// <summary>  
        /// 文件路径App_Data/ShopConfig.config  
        /// </summary>  
        public static string shopconfigfile
        {
            get
            {
                if (string.IsNullOrEmpty(_shopconfigfile)) _shopconfigfile = HttpContext.Current.Server.MapPath("/App_Data/ShopConfig.config");
                return _shopconfigfile;
            }
        }
        #endregion

        #region 文件路径App_Data/SiteConfig.config  
        private static string _siteconfigfile = string.Empty;
        /// <summary>  
        /// 文件路径App_Data/SiteConfig.config  
        /// </summary>  
        public static string siteconfigfile
        {
            get
            {
                if (string.IsNullOrEmpty(_siteconfigfile)) _siteconfigfile = HttpContext.Current.Server.MapPath("/App_Data/SiteConfig.config");
                return _siteconfigfile;
            }
        }
        #endregion

        #region 文件路径App_Data/Template.config  
        private static string _templateconfigfile = string.Empty;
        /// <summary>  
        /// 文件路径App_Data/Template.config  
        /// </summary>  
        public static string templateconfigfile
        {
            get
            {
                if (string.IsNullOrEmpty(_templateconfigfile)) _templateconfigfile = HttpContext.Current.Server.MapPath("/App_Data/Template.config");
                return _templateconfigfile;
            }
        }
        #endregion

        #region 删除缓存  
        /// <summary>  
        /// 删除缓存  
        /// </summary>  
        /// <param name="CacheKey">键</param>  
        public static void DeleteCache(string CacheKey)
        {
            HttpRuntime.Cache.Remove(CacheKey);
        }
        #endregion

        #region 获取缓存，依赖时间  
        /// <summary>  
        /// 获取缓存，依赖时间  
        /// </summary>  
        /// <param name="CacheKey">键</param>  
        /// <returns></returns>  
        public static object GetCache(string CacheKey)
        {
            object obj_time = HttpRuntime.Cache[CacheKey + "_time"];
            object obj_cache = HttpRuntime.Cache[CacheKey];
            if (obj_time != null && obj_cache != null)
            {
                if (Convert.ToDateTime(obj_time) < DateTime.Now)
                {
                    DeleteCache(CacheKey);
                    DeleteCache(CacheKey + "_time");
                    return null;
                }
                else return obj_cache;
            }
            else
            {
                DeleteCache(CacheKey);
                DeleteCache(CacheKey + "_time");
                return null;
            }
        }
        #endregion

        #region 获取缓存，依赖文件  
        /// <summary>  
        /// 获取缓存，依赖文件  
        /// </summary>  
        /// <param name="CacheKey">键</param>  
        /// <param name="depFile">依赖的文件</param>  
        /// <returns></returns>  
        public static object GetCache(string CacheKey, string depFile)
        {
            object obj_time = HttpRuntime.Cache[CacheKey + "_time"];
            object obj_cache = HttpRuntime.Cache[CacheKey];
            if (File.Exists(depFile))
            {
                FileInfo fi = new FileInfo(depFile);

                if (obj_time != null && obj_cache != null)
                {
                    if (Convert.ToDateTime(obj_time) != fi.LastWriteTime)
                    {
                        DeleteCache(CacheKey);
                        DeleteCache(CacheKey + "_time");
                        return null;
                    }
                    else return obj_cache;
                }
                else
                {
                    DeleteCache(CacheKey);
                    DeleteCache(CacheKey + "_time");
                    return null;
                }
            }
            else
            {
                throw new Exception("文件(" + depFile + ")不存在！");
            }
        }
        #endregion

        #region 简单的插入缓存  
        /// <summary>  
        /// 简单的插入缓存  
        /// </summary>  
        /// <param name="CacheKey">键</param>  
        /// <param name="objObject">数据</param>  
        public static void SetCache(string CacheKey, object objObject)
        {
            HttpRuntime.Cache.Insert(CacheKey, objObject);
        }
        #endregion

        #region 有过期时间的插入缓存数据  
        /// <summary>  
        /// 有过期时间的插入缓存数据  
        /// </summary>  
        /// <param name="CacheKey">键</param>  
        /// <param name="objObject">数据</param>  
        /// <param name="absoluteExpiration">过期时间</param>  
        /// <param name="slidingExpiration">可调度参数，传null就是禁用可调度</param>  
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            if (slidingExpiration == null) slidingExpiration = System.Web.Caching.Cache.NoSlidingExpiration;
            HttpRuntime.Cache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
            HttpRuntime.Cache.Insert(CacheKey + "_time", absoluteExpiration, null, absoluteExpiration, slidingExpiration);//存储过期时间  
        }
        #endregion

        #region 插入缓存数据，指定缓存多少秒  
        /// <summary>  
        /// 插入缓存数据，指定缓存多少秒  
        /// </summary>  
        /// <param name="CacheKey">缓存的键</param>  
        /// <param name="objObject">缓存的数据</param>  
        /// <param name="seconds">缓存秒数</param>  
        /// <param name="slidingExpiration">传null就是禁用可调度过期</param>  
        public static void SetCacheSecond(string CacheKey, object objObject, int seconds, TimeSpan slidingExpiration)
        {
            DateTime absoluteExpiration = DateTime.Now.AddSeconds(seconds);
            if (slidingExpiration == null) slidingExpiration = System.Web.Caching.Cache.NoSlidingExpiration;
            HttpRuntime.Cache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
            HttpRuntime.Cache.Insert(CacheKey + "_time", absoluteExpiration, null, absoluteExpiration, slidingExpiration);//存储过期时间  
        }
        #endregion

        #region 依赖文件的缓存，文件没改不会过期  
        /// <summary>  
        /// 依赖文件的缓存，文件没改不会过期  
        /// </summary>  
        /// <param name="CacheKey">键</param>  
        /// <param name="objObject">数据</param>  
        /// <param name="depfilename">依赖文件，可调用 DataCache 里的变量</param>  
        public static void SetCacheDepFile(string CacheKey, object objObject, string depfilename)
        {
            //缓存依赖对象  
            System.Web.Caching.CacheDependency dep = new System.Web.Caching.CacheDependency(depfilename);
            DateTime absoluteExpiration = System.Web.Caching.Cache.NoAbsoluteExpiration;
            TimeSpan slidingExpiration = System.Web.Caching.Cache.NoSlidingExpiration;
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(
                CacheKey,
                objObject,
                dep,
                System.Web.Caching.Cache.NoAbsoluteExpiration, //从不过期  
                slidingExpiration, //禁用可调过期  
                System.Web.Caching.CacheItemPriority.Default,
                null);
            if (File.Exists(depfilename))
            {
                FileInfo fi = new FileInfo(depfilename);
                DateTime lastWriteTime = fi.LastWriteTime;
                HttpRuntime.Cache.Insert(CacheKey + "_time", lastWriteTime, null, absoluteExpiration, slidingExpiration);//存储文件最后修改时间  
            }

        }
        #endregion
    }
}