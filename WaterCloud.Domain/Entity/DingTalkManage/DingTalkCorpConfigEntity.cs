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

namespace WaterCloud.Entity.DingTalkManage
{
    //DingTalkCorpConfig
    [TableAttribute("Dtk_CorpConfig")]
    //DingTalkCorpConfig
    public class DingTalkCorpConfigEntity : IEntityV2<DingTalkCorpConfigEntity>, ICreationAuditedV2, IModificationAuditedV2
    {
        [ColumnAttribute("uuId", IsPrimaryKey = true)]

        /// <summary>
        /// 主键
        /// </summary>		
        public string uuId { get; set; }
        /// <summary>
        /// CorpId
        /// </summary>	
        public string CorpId{ get; set; }        
		/// <summary>
		/// CorpSecret
        /// </summary>	
        public string CorpSecret{ get; set; }        
		/// <summary>
		/// AppName
        /// </summary>	
        public string AppName{ get; set; }        
		/// <summary>
		/// HeadPic
        /// </summary>	
        public string HeadPic{ get; set; }        
		/// <summary>
		/// ApiUrl
        /// </summary>	
        public string ApiUrl{ get; set; }        
		/// <summary>
		/// AgentId
        /// </summary>
        public string AgentId{ get; set; }
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// AppKey
        /// </summary>	
        public string AppKey{ get; set; }        
		/// <summary>
		/// AppSecret
        /// </summary>	
        public string AppSecret{ get; set; }        
		/// <summary>
		/// Access_Token
        /// </summary>	
        public string Access_Token{ get; set; }         
		/// <summary>
		/// Status
        /// </summary>	
        public int Status{ get; set; }        
		/// <summary>
		/// Encoding
        /// </summary>		
        public string Encoding{ get; set; }        
		/// <summary>
		/// Remark
        /// </summary>	
        public string Remark{ get; set; }
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
        /// <summary>
        /// AppType
        /// </summary>	
        public int AppType { get; set; }
    }
}


