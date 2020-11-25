using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Code.SysConfig
{
    /// <summary>
    /// 钉钉平台配置实体类
    /// </summary>
    [Serializable]
    public class DingTalkConfig
    {
        public DingTalkConfig()
        { }

        private string _CorpId = string.Empty;
        private string _CorpSecret = string.Empty;
        private string _AppName = string.Empty;
        private string _HeadPic = string.Empty;
        private int? _AppType = 0;
        private string _AgentId = string.Empty;
        private string _AppKey = string.Empty;
        private string _AppSecret = string.Empty;
        private string _ApiUrl = string.Empty;
        private string _Access_Token = string.Empty;
        private string _Encoding = string.Empty;


        #region 钉钉配置基本信息==================================
        /// <summary>
        /// 企业ID
        /// </summary>
        public string CorpId
        {
            get { return _CorpId; }
            set { _CorpId = value; }
        }
        /// <summary>
        /// 企业密钥
        /// </summary>
        public string CorpSecret
        {
            get { return _CorpSecret; }
            set { _CorpSecret = value; }
        }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName
        {
            get { return _AppName; }
            set { _AppName = value; }
        }
        /// <summary>
        /// 应用图标
        /// </summary>
        public string HeadPic
        {
            get { return _HeadPic; }
            set { _HeadPic = value; }
        }
        /// <summary>
        /// AppType
        /// </summary>
        public int? AppType
        {
            get { return _AppType; }
            set { _AppType = value; }
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
        /// 应用访问地址
        /// </summary>
        public string ApiUrl
        {
            get { return _ApiUrl; }
            set { _ApiUrl = value; }
        }
        /// <summary>
        /// Encoding
        /// </summary>
        public string Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }
        #endregion




    }
}