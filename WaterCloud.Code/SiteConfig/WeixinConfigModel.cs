using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Code.SysConfig
{
    /// <summary>
    /// 微信平台配置实体类
    /// </summary>
    [Serializable]
    public class WeixinConfig
    {
        public WeixinConfig()
        { }

        private string _WxName = string.Empty;
        private string _WxId = string.Empty;
        private string _WxCode = string.Empty;
        private string _HeadPic = string.Empty;
        private string _Token = string.Empty;
        private string _ApiUrl = string.Empty;
        private string _AppId = string.Empty;
        private string _AgentId = string.Empty;
        private string _AppKey = string.Empty;
        private string _AppSecret = string.Empty;
        private string _Access_Token = string.Empty;
        private string _Encoding = string.Empty;
        private int? _WxType = 0;
        private bool _CloseKW = false;
        private string _TimeStamp = string.Empty;
        private DateTime? _PersonSynDate = new DateTime();
        private int? _OpenidCount = 0;

        #region 微信平台基本信息==================================
        /// <summary>
        /// 微信公众号名称
        /// </summary>
        public string WxName
        {
            get { return _WxName; }
            set { _WxName = value; }
        }
        /// <summary>
        /// 微信公众号ID
        /// </summary>
        public string WxId
        {
            get { return _WxId; }
            set { _WxId = value; }
        }
        /// <summary>
        /// 微信代码
        /// </summary>
        public string WxCode
        {
            get { return _WxCode; }
            set { _WxCode = value; }
        }
        /// <summary>
        /// 公众号头像
        /// </summary>
        public string HeadPic
        {
            get { return _HeadPic; }
            set { _HeadPic = value; }
        }
        /// <summary>
        /// Token值
        /// </summary>
        public string Token
        {
            get { return _Token; }
            set { _Token = value; }
        }
        /// <summary>
        /// API接口地址
        /// </summary>
        public string ApiUrl
        {
            get { return _ApiUrl; }
            set { _ApiUrl = value; }
        }
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId
        {
            get { return _AppId; }
            set { _AppId = value; }
        }
        /// <summary>
        /// AgentId
        /// </summary>
        public string AgentId
        {
            get { return _AgentId; }
            set { _AgentId = value; }
        }
        /// <summary>
        /// AppKey
        /// </summary>
        public string AppKey
        {
            get { return _AppKey; }
            set { _AppKey = value; }
        }
        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret
        {
            get { return _AppSecret; }
            set { _AppSecret = value; }
        }
        /// <summary>
        /// Access_Token
        /// </summary>
        public string Access_Token
        {
            get { return _Access_Token; }
            set { _Access_Token = value; }
        }
        /// <summary>
        /// Encoding
        /// </summary>
        public string Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }
        /// <summary>
        /// 公众号类型
        /// </summary>
        public int? WxType
        {
            get { return _WxType; }
            set { _WxType = value; }
        }
        /// <summary>
        /// 是否关闭客服
        /// </summary>
        public bool CloseKW
        {
            get { return _CloseKW; }
            set { _CloseKW = value; }
        }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp
        {
            get { return _TimeStamp; }
            set { _TimeStamp = value; }
        }
        /// <summary>
        /// 粉丝更新时间
        /// </summary>
        public DateTime? PersonSynDate
        {
            get { return _PersonSynDate; }
            set { _PersonSynDate = value; }
        }
        /// <summary>
        /// 粉丝更新数量
        /// </summary>
        public int? OpenidCount
        {
            get { return _OpenidCount; }
            set { _OpenidCount = value; }
        }
        #endregion
        
    }
}