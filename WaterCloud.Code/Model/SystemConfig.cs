using System;
using System.Collections.Generic;
using System.Text;

namespace WaterCloud.Code.Model
{
    public class SystemConfig
    {
        /// <summary>
        /// 是否是Demo模式
        /// </summary>
        public bool Demo { get; set; }
        /// <summary>
        /// 是否是调试模式
        /// </summary>
        public bool Debug { get; set; }
        /// <summary>
        /// 允许一个用户在多个电脑同时登录
        /// </summary>
        public bool LoginMultiple { get; set; }
        public string LoginProvider { get; set; }
        /// <summary>
        ///  数据库超时间（秒）
        /// </summary>
        public int CommandTimeout { get; set; }
        /// <summary>
        /// Snow Flake Worker Id
        /// </summary>
        public int SnowFlakeWorkerId { get; set; }
        /// <summary>
        /// api地址
        /// </summary>
        public string ApiSite { get; set; }
        /// <summary>
        /// 允许跨域的站点
        /// </summary>
        public string AllowCorsSite { get; set; }
        /// <summary>
        /// 网站虚拟目录
        /// </summary>
        public string VirtualDirectory { get; set; }

        public string DBProvider { get; set; }
        public string DBConnectionString { get; set; }
        public string DBCommandTimeout { get; set; }
        /// <summary>
        /// 数据库备份路径
        /// </summary>
        public string DBBackup { get; set; }
        
        public string CacheProvider { get; set; }
        public string HandleLogProvider { get; set; }
        public string RedisConnectionString { get; set; }
        public string SysemUserId { get; set; }
        public string SysemUserCode { get; set; }
        public string SysemUserPwd { get; set; }
        public string SysemMasterProject { get; set; }
        public string TokenName { get; set; }
        //缓存过期时间
        public int LoginExpire { get; set; }
        public string HomePage { get; set; }
        public string MainProgram { get; set; }
        public bool? LocalLAN { get; set; }
        /// <summary>
        /// 雪花id配置
        /// </summary>
        public int WorkId { get; set; }
    }
}
