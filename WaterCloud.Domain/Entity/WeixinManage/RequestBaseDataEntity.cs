/*******************************************************************************
 * Copyright © 2018 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud
 * Website：
*********************************************************************************/

using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using Chloe.Annotations;

namespace WaterCloud.Entity.WeixinManage 
{
	 	//WeixinMP_RequestBaseData
    [TableAttribute("Wct_RequestBaseData")]

    public class RequestBaseDataEntity : IEntityV2<RequestBaseDataEntity>
	{
        [ColumnAttribute("uuId", IsPrimaryKey = true)]

        /// <summary>
        /// 主键
        /// </summary>		
        public string uuId { get; set; }        
		/// <summary>
		/// 用户的微信id
        /// </summary>		
        public string OpenId{ get; set; }        
		/// <summary>
		/// 数据类型 文本消息：text 图片消息:image 地理位置消息:location 链接消息:link 事件推送:event
        /// </summary>		
        public string MsgType{ get; set; }        
		/// <summary>
		/// 数据内容
        /// </summary>		
        public string DataContent{ get; set; }        
		/// <summary>
		/// 事件KEY值
        /// </summary>		
        public string EventKey{ get; set; }    
		/// <summary>
		/// url路径
        /// </summary>		
        public string Url{ get; set; }        
		/// <summary>
		/// 链接地址2
        /// </summary>		
        public string Url2{ get; set; }        
		/// <summary>
		/// xml原始内容
        /// </summary>		
        public string XmlContent{ get; set; }        
		/// <summary>
		/// 录入系统的时间
        /// </summary>		
        public DateTime CreateDate{ get; set; }        
		/// <summary>
		/// 状态
        /// </summary>		
        public int Flag{ get; set; }        
		/// <summary>
		/// 自定义类型
        /// </summary>		
        public string RType{ get; set; }        
		/// <summary>
		/// 备注
        /// </summary>		
        public string Remark{ get; set; }        
		/// <summary>
		/// 扩展Int字段
        /// </summary>		
        public int ExtInt{ get; set; }        
		/// <summary>
		/// 扩展str
        /// </summary>		
        public string ExtStr{ get; set; }        
		/// <summary>
		/// 扩展str2
        /// </summary>		
        public string ExtStr2{ get; set; }        
		/// <summary>
		/// 扩展str3
        /// </summary>		
        public string ExtStr3{ get; set; }
        /// <summary>
        ///创建人
        /// </summary>		
        public string Creator { get; set; }
        /// <summary>
		/// 创建人ID
        /// </summary>		
        public string CreatorId { get; set; }
        /// <summary>
		/// 创建时间
        /// </summary>		
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>		
        public string Modifier { get; set; }
        /// <summary>
        /// 修改人ID
        /// </summary>		
        public string ModifierId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>		
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 删除人ID
        /// </summary>		
        public string DeleterId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>		
        public DateTime? DeleteTime { get; set; }
    }
}

