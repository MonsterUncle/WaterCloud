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
	 	//WeixinMP_RequestRule
    [TableAttribute("Wct_RequestRule")]

    public class RequestRuleEntity : IEntityV2<RequestRuleEntity>  
	{
        [ColumnAttribute("uuId", IsPrimaryKey = true)]
        /// <summary>
        /// 主键
        /// </summary>		
        public string uuId{ get; set; }        
		/// <summary>
		/// 用户主键ID
        /// </summary>		
        public string UserId{ get; set; }        
		/// <summary>
		/// RuleName
        /// </summary>		
        public string RuleName{ get; set; }        
		/// <summary>
		/// ReqKeywords
        /// </summary>		
        public string ReqKeywords{ get; set; }        
		/// <summary>
		/// ReqestType
        /// </summary>		
        public string ReqestType{ get; set; }        
		/// <summary>
		/// 回复类型（文本1，图文2，语音3，视频4,第三方接口5）
        /// </summary>		
        public int ResponseType{ get; set; }        
		/// <summary>
		/// 是默认回复
        /// </summary>		
        public bool IsDefault{ get; set; }        
		/// <summary>
		/// 功能模版名称
        /// </summary>		
        public string ModelFunctionName{ get; set; }        
		/// <summary>
		/// 功能模块Id
        /// </summary>		
        public int ModelFunctionId{ get; set; }        
		/// <summary>
		/// 排序号
        /// </summary>		
        public int Seq{ get; set; }        
		/// <summary>
		/// 创建时间
        /// </summary>		
        public DateTime CreateDate{ get; set; }        
		/// <summary>
		/// 第三方接口的url
        /// </summary>		
        public string AgentUrl{ get; set; }        
		/// <summary>
		/// 第三方token值
        /// </summary>		
        public string AgentToken{ get; set; }        
		/// <summary>
		/// 是模糊查询
        /// </summary>		
        public bool IsLikeSearch{ get; set; }        
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
        public string ExtStr3{ get; set; }        
		/// <summary>
		/// 扩展str4
        /// </summary>		
        public string ExtStr4{ get; set; }
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

