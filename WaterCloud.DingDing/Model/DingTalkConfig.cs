using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterCloud.DingTalk
{
    /// <summary>
    ///  钉钉企业参数配置
    /// </summary>
    public class DingTalkCorpConfig
    {
        public string CorpId { get; set; }
        public string CorpSecret { get; set; }
        public string AppName { get; set; }
        public string HeadPic { get; set; }
        public string ApiUrl { get; set; }
        public string AgentId { get; set; }
        public string AppId { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }

        public DateTime ExpiresTime { get; set; }
        /// <summary>
        /// 当前时间戳
        /// </summary>
        public long TimeStamp { get; set; }
        /// <summary>
        /// 签名随机码
        /// </summary>
        public string NonceStr { get; set; }
        /// <summary>
        /// 签名字符串
        /// </summary>
        public string Signature { get; set; }

        public string jsapi_ticket { get; set; }
        public string url { get; set; }
    }

    /// <summary>
    /// 钉钉应用配置
    /// </summary>
    public class DingTalkAppConfig
    {
        public string AppCode { get; set; }
        public string AppName { get; set; }
        public string AgentId { get; set; }
        public string AppId { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiresTime { get; set; }
        /// <summary>
        /// 当前时间戳
        /// </summary>
        public long TimeStamp { get; set; }
        /// <summary>
        /// 签名随机码
        /// </summary>
        public string NonceStr { get; set; }
        /// <summary>
        /// 签名字符串
        /// </summary>
        public string Signature { get; set; }
        public string jsapi_ticket { get; set; }
        public string url { get; set; }
        
    }
   
    /// <summary>
    /// 
    /// </summary>
    public class AccessSignature
    {
        public string signature { get; set; }
        public string nonceStr { get; set; }
        public string timeStamp { get; set; }
    }
}
