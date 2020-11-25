using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.WeixinMP
{
    partial class RequestRuleContent
    {
        /// <summary>
		/// 编号
		/// </summary>
		public int id { set; get; }
        /// <summary>
        /// 用户表主键Id
        /// </summary>
        public int? uId { set; get; }
        /// <summary>
        /// 规则主键Id
        /// </summary>
        public int? rId { set; get; }
        /// <summary>
        /// 内容
        /// </summary>
        public string rContent { set; get; }
        /// <summary>
        /// 内容2
        /// </summary>
        public string rContent2 { set; get; }
        /// <summary>
        /// 详情链接地址
        /// </summary>
        public string detailUrl { set; get; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string picUrl { set; get; }
        /// <summary>
        /// 语音或视频地址
        /// </summary>
        public string mediaUrl { set; get; }
        /// <summary>
        /// 高清语音或者视频地址
        /// </summary>
        public string meidaHDUrl { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { set; get; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int? seq { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createDate { set; get; }
        /// <summary>
        /// 扩展int
        /// </summary>
        public int? extInt { set; get; }
        /// 扩展int
        /// </summary>
        public int? extInt2 { set; get; }
        /// <summary>
        /// 扩展str
        /// </summary>
        public string extStr { set; get; }
        /// <summary>
        /// 扩展str2
        /// </summary>
        public string extStr2 { set; get; }
        /// <summary>
        /// 扩展str3
        /// </summary>
        public string extstr3 { set; get; }
    }
}
