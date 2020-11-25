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
    //WeixinMP_TemplateMessage
    [TableAttribute("Wct_TemplateMessage")]
    public class TemplateMessageEntity : IEntityV2<TemplateMessageEntity>  
	{
        [ColumnAttribute("uuId", IsPrimaryKey = true)]
        /// <summary>
        /// 主键
        /// </summary>		
        public string uuId{ get; set; }        
		/// <summary>
		/// 模板代码
        /// </summary>		
        public string Code{ get; set; }        
		/// <summary>
		/// 官方模板ID
        /// </summary>		
        public string TemplateId{ get; set; }        
		/// <summary>
		/// 官方模板标题
        /// </summary>		
        public string Title{ get; set; }        
		/// <summary>
		/// 模板所属行业的一级行业
        /// </summary>		
        public int Primary_Industry{ get; set; }        
		/// <summary>
		/// 模板所属行业的二级行业
        /// </summary>		
        public int Deputy_Industry{ get; set; }        
		/// <summary>
		/// Data_First
        /// </summary>		
        public string Data_First{ get; set; }        
		/// <summary>
		/// Data_Remark
        /// </summary>		
        public string Data_Remark{ get; set; }        
		/// <summary>
		/// Data_Keyword1
        /// </summary>		
        public string Data_Keyword1{ get; set; }        
		/// <summary>
		/// Data_Keyword2
        /// </summary>		
        public string Data_Keyword2{ get; set; }        
		/// <summary>
		/// Data_Keyword3
        /// </summary>		
        public string Data_Keyword3{ get; set; }        
		/// <summary>
		/// Data_Keyword4
        /// </summary>		
        public string Data_Keyword4{ get; set; }        
		/// <summary>
		/// Data_Keyword5
        /// </summary>		
        public string Data_Keyword5{ get; set; }        
		/// <summary>
		/// 模板跳转链接
        /// </summary>		
        public string LinkUrl{ get; set; }        
		/// <summary>
		/// MiniProgram
        /// </summary>		
        public int MiniProgram{ get; set; }        
		/// <summary>
		/// 小程序appid
        /// </summary>		
        public string Appid{ get; set; }        
		/// <summary>
		/// 所需跳转到小程序的具体页面路径，支持带参数
        /// </summary>		
        public string Pagepath{ get; set; }        
		/// <summary>
		/// 首部颜色
        /// </summary>		
        public string FirstColor{ get; set; }        
		/// <summary>
		/// 备注部分颜色
        /// </summary>		
        public string RemarkColor{ get; set; }        
		/// <summary>
		/// 模板内容
        /// </summary>		
        public string Content{ get; set; }        
		/// <summary>
		/// 状态
        /// </summary>		
        public bool Status{ get; set; }        
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

