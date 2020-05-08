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

namespace WaterCloud.Domain.DingTalkManage
{
    //DingTalkCorpConfig
    [TableAttribute("Dtk_MessageSendRecord")]
    public class MessageSendRecordEntity : IEntityV2<MessageSendRecordEntity>, ICreationAuditedV2, IModificationAuditedV2
    {
        [ColumnAttribute("uuId", IsPrimaryKey = true)]
        /// <summary>
        /// 主键
        /// </summary>		
        public string uuId { get; set; }
        /// <summary>
        /// Code
        /// </summary>		
        public string Code{ get; set; }        
		/// <summary>
		/// RecType
        /// </summary>		
        public string RecType{ get; set; }        
		/// <summary>
		/// UseridList
        /// </summary>		
        public string UseridList{ get; set; }        
		/// <summary>
		/// DeptList
        /// </summary>		
        public string DeptList{ get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// Title
        /// </summary>		
        public string Title{ get; set; }        
		/// <summary>
		/// Content
        /// </summary>		
        public string Content{ get; set; }    
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// SendType
        /// </summary>		
        public string SendType{ get; set; }        
		/// <summary>
		/// SendDate
        /// </summary>		
        public DateTime? SendDate{ get; set; }        
		/// <summary>
		/// State
        /// </summary>		
        public int State{ get; set; }
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

