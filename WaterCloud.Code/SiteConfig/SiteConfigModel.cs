﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Code.SysConfig
{
    /// <summary>
    /// 系统配置实体类
    /// </summary>
    [Serializable]
    public class SiteConfig
    {
        public SiteConfig()
        { }

        private string _webname = string.Empty;
        private string _weburl = string.Empty;
        private string _webcompany = string.Empty;
        private string _webaddress = string.Empty;
        private string _webtel = string.Empty;
        private string _webfax = string.Empty;
        private string _webmail = string.Empty;
        private string _webcrod = string.Empty;
        private string _webpath = string.Empty;

        private string _smsapiurl = string.Empty;
        private string _smsusername = string.Empty;
        private string _smspassword = string.Empty;

        private string _emailsmtp = string.Empty;
        private int _emailssl = 0;
        private int _emailport = 25;
        private string _emailfrom = string.Empty;
        private string _emailusername = string.Empty;
        private string _emailpassword = string.Empty;
        private string _emailnickname = string.Empty;

        private string _filepath = string.Empty;
        private int _filesave = 1;
        private int _fileanonymous = 0;
        private int _fileremote = 0;
        private string _fileextension = string.Empty;
        private string _videoextension = string.Empty;
        private int _attachsize = 0;
        private int _videosize = 0;
        private int _imgsize = 0;
        private int _imgmaxheight = 0;
        private int _imgmaxwidth = 0;
        private int _thumbnailheight = 0;
        private int _thumbnailwidth = 0;
        private string _thumbnailmode = "Cut";
        private int _watermarktype = 0;
        private int _watermarkposition = 9;
        private int _watermarkimgquality = 80;
        private string _watermarkpic = string.Empty;
        private int _watermarktransparency = 10;
        private string _watermarktext = string.Empty;
        private string _watermarkfont = string.Empty;
        private int _watermarkfontsize = 12;
        private string _fileserver = "localhost";

        #region 主站基本信息==================================
        /// <summary>
        /// 网站名称
        /// </summary>
        public string webname
        {
            get { return _webname; }
            set { _webname = value; }
        }
        /// <summary>
        /// 网站域名
        /// </summary>
        public string weburl
        {
            get { return _weburl; }
            set { _weburl = value; }
        }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string webcompany
        {
            get { return _webcompany; }
            set { _webcompany = value; }
        }
        /// <summary>
        /// 通讯地址
        /// </summary>
        public string webaddress
        {
            get { return _webaddress; }
            set { _webaddress = value; }
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string webtel
        {
            get { return _webtel; }
            set { _webtel = value; }
        }
        /// <summary>
        /// 传真号码
        /// </summary>
        public string webfax
        {
            get { return _webfax; }
            set { _webfax = value; }
        }
        /// <summary>
        /// 管理员邮箱
        /// </summary>
        public string webmail
        {
            get { return _webmail; }
            set { _webmail = value; }
        }
        /// <summary>
        /// 网站备案号
        /// </summary>
        public string webcrod
        {
            get { return _webcrod; }
            set { _webcrod = value; }
        }
        #endregion

        #region 功能权限设置==================================
        /// <summary>
        /// 网站安装目录
        /// </summary>
        public string webpath
        {
            get { return _webpath; }
            set { _webpath = value; }
        }
       
        #endregion

        #region 短信平台设置==================================
        /// <summary>
        /// 短信API地址
        /// </summary>
        public string smsapiurl
        {
            get { return _smsapiurl; }
            set { _smsapiurl = value; }
        }
        /// <summary>
        /// 短信平台登录账户名
        /// </summary>
        public string smsusername
        {
            get { return _smsusername; }
            set { _smsusername = value; }
        }
        /// <summary>
        /// 短信平台登录密码
        /// </summary>
        public string smspassword
        {
            get { return _smspassword; }
            set { _smspassword = value; }
        }
        #endregion

        #region 邮件发送设置==================================
        /// <summary>
        /// STMP服务器
        /// </summary>
        public string emailsmtp
        {
            get { return _emailsmtp; }
            set { _emailsmtp = value; }
        }
        /// <summary>
        /// 是否启用SSL加密连接
        /// </summary>
        public int emailssl
        {
            get { return _emailssl; }
            set { _emailssl = value; }
        }
        /// <summary>
        /// SMTP端口
        /// </summary>
        public int emailport
        {
            get { return _emailport; }
            set { _emailport = value; }
        }
        /// <summary>
        /// 发件人地址
        /// </summary>
        public string emailfrom
        {
            get { return _emailfrom; }
            set { _emailfrom = value; }
        }
        /// <summary>
        /// 邮箱账号
        /// </summary>
        public string emailusername
        {
            get { return _emailusername; }
            set { _emailusername = value; }
        }
        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string emailpassword
        {
            get { return _emailpassword; }
            set { _emailpassword = value; }
        }
        /// <summary>
        /// 发件人昵称
        /// </summary>
        public string emailnickname
        {
            get { return _emailnickname; }
            set { _emailnickname = value; }
        }
        #endregion

        #region 文件上传设置==================================
        /// <summary>
        /// 附件上传目录
        /// </summary>
        public string filepath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }
        /// <summary>
        /// 附件保存方式
        /// </summary>
        public int filesave
        {
            get { return _filesave; }
            set { _filesave = value; }
        }
        /// <summary>
        /// 允许匿名上传(0否1是)
        /// </summary>
        public int fileanonymous
        {
            get { return _fileanonymous; }
            set { _fileanonymous = value; }
        }
        /// <summary>
        /// 编辑器远程图片上传
        /// </summary>
        public int fileremote
        {
            get { return _fileremote; }
            set { _fileremote = value; }
        }
        /// <summary>
        /// 附件上传类型
        /// </summary>
        public string fileextension
        {
            get { return _fileextension; }
            set { _fileextension = value; }
        }
        /// <summary>
        /// 视频上传类型
        /// </summary>
        public string videoextension
        {
            get { return _videoextension; }
            set { _videoextension = value; }
        }
        /// <summary>
        /// 文件上传大小
        /// </summary>
        public int attachsize
        {
            get { return _attachsize; }
            set { _attachsize = value; }
        }
        /// <summary>
        /// 视频上传大小
        /// </summary>
        public int videosize
        {
            get { return _videosize; }
            set { _videosize = value; }
        }
        /// <summary>
        /// 图片上传大小
        /// </summary>
        public int imgsize
        {
            get { return _imgsize; }
            set { _imgsize = value; }
        }
        /// <summary>
        /// 图片最大高度(像素)
        /// </summary>
        public int imgmaxheight
        {
            get { return _imgmaxheight; }
            set { _imgmaxheight = value; }
        }
        /// <summary>
        /// 图片最大宽度(像素)
        /// </summary>
        public int imgmaxwidth
        {
            get { return _imgmaxwidth; }
            set { _imgmaxwidth = value; }
        }
        /// <summary>
        /// 生成缩略图高度(像素)
        /// </summary>
        public int thumbnailheight
        {
            get { return _thumbnailheight; }
            set { _thumbnailheight = value; }
        }
        /// <summary>
        /// 生成缩略图宽度(像素)
        /// </summary>
        public int thumbnailwidth
        {
            get { return _thumbnailwidth; }
            set { _thumbnailwidth = value; }
        }
        /// <summary>
        /// 缩略图生成方式
        /// </summary>
        public string thumbnailmode
        {
            get { return _thumbnailmode; }
            set { _thumbnailmode = value; }
        }
        /// <summary>
        /// 图片水印类型
        /// </summary>
        public int watermarktype
        {
            get { return _watermarktype; }
            set { _watermarktype = value; }
        }
        /// <summary>
        /// 图片水印位置
        /// </summary>
        public int watermarkposition
        {
            get { return _watermarkposition; }
            set { _watermarkposition = value; }
        }
        /// <summary>
        /// 图片生成质量
        /// </summary>
        public int watermarkimgquality
        {
            get { return _watermarkimgquality; }
            set { _watermarkimgquality = value; }
        }
        /// <summary>
        /// 图片水印文件
        /// </summary>
        public string watermarkpic
        {
            get { return _watermarkpic; }
            set { _watermarkpic = value; }
        }
        /// <summary>
        /// 水印透明度
        /// </summary>
        public int watermarktransparency
        {
            get { return _watermarktransparency; }
            set { _watermarktransparency = value; }
        }
        /// <summary>
        /// 水印文字
        /// </summary>
        public string watermarktext
        {
            get { return _watermarktext; }
            set { _watermarktext = value; }
        }
        /// <summary>
        /// 文字字体
        /// </summary>
        public string watermarkfont
        {
            get { return _watermarkfont; }
            set { _watermarkfont = value; }
        }
        /// <summary>
        /// 文字大小(像素)
        /// </summary>
        public int watermarkfontsize
        {
            get { return _watermarkfontsize; }
            set { _watermarkfontsize = value; }
        }
        #endregion

       
    }
}