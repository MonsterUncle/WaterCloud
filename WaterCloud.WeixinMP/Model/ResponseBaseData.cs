using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.WeixinMP
{
    public class ResponseBaseData
    {
        /// <summary>
		/// 编号
		/// </summary>
		public int Id { set;get; }
       
        /// <summary>
        /// 用户的微信id
        /// </summary>
        public string openId { set; get; }
        /// <summary>
        /// 数据类型 文本消息：text 图片消息:image 地理位置消息:location 链接消息:link 事件:event
        /// </summary>
        public string RequestType { set; get; }
        /// <summary>
        /// 数据内容
        /// </summary>
        public string RequestContent { set; get; }
        /// <summary>
        /// 回复的类型 文本消息：text 图片消息:image 地理位置消息:location 链接消息:link
        /// </summary>
        public string ResponseType { set; get; }
        /// <summary>
        /// 系统回复的内容
        /// </summary>
        public string ReponseContent { set; get; }
        /// <summary>
        /// 消息创建时间
        /// </summary>
        public string CreateTime { set; get; }
        /// <summary>
        /// xml原始内容
        /// </summary>
        public string XmlContent { set; get; }
        /// <summary>
        /// 录入系统的时间
        /// </summary>
        public DateTime? CreateDate { set; get; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Flag { set; get; }
        /// <summary>
        /// 自定义类型
        /// </summary>
        public string RType { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 扩展Int字段
        /// </summary>
        public int? ExtInt { set; get; }
        /// <summary>
        /// 扩展str
        /// </summary>
        public string ExtStr { set; get; }
        /// <summary>
        /// 扩展str2
        /// </summary>
        public string ExtStr2 { set; get; }
        /// <summary>
        /// 扩展str3
        /// </summary>
        public string ExtStr3 { set; get; }
    }
}
