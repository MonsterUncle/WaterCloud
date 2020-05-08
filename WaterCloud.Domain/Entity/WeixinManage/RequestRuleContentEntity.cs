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

namespace WaterCloud.Domain.WeixinManage 
{
    //WeixinMP_RequestRuleContent
    [TableAttribute("Wct_RequestRuleContent")]
    public class RequestRuleContentEntity : IEntityV2<RequestRuleContentEntity>
	{
        [ColumnAttribute("uuId", IsPrimaryKey = true)]
        /// <summary>
        /// 主键
        /// </summary>		
        public string uuId { get; set; }
        /// <summary>
        /// 用户表主键Id
        /// </summary>		
        public string UId { get; set; }        
		/// <summary>
		/// 规则主键Id
        /// </summary>		
        public string RId { get; set; }        
		/// <summary>
		/// 内容
        /// </summary>		
        public string RContent{ get; set; }        
		/// <summary>
		/// 内容2
        /// </summary>		
        public string RContent2{ get; set; }        
		/// <summary>
		/// 详情链接地址
        /// </summary>		
        public string DetailUrl{ get; set; }        
		/// <summary>
		/// 图片地址
        /// </summary>		
        public string PicUrl{ get; set; }        
		/// <summary>
		/// 语音或视频地址
        /// </summary>		
        public string MediaUrl{ get; set; }        
		/// <summary>
		/// 高清语音或者视频地址
        /// </summary>		
        public string MeidaHDUrl{ get; set; }        
		/// <summary>
		/// 备注
        /// </summary>		
        public string Remark{ get; set; }        
		/// <summary>
		/// 排序号
        /// </summary>		
        public int Seq{ get; set; }        
		/// <summary>
		/// 创建时间
        /// </summary>		
        public DateTime CreateDate{ get; set; }        
		/// <summary>
		/// 扩展int
        /// </summary>		
        public int ExtInt{ get; set; }        
		/// <summary>
		/// 扩展int
        /// </summary>		
        public int ExtInt2{ get; set; }        
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
        public string Extstr3{ get; set; }
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

