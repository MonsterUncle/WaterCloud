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
    [TableAttribute("Dtk_MessageTemplate")]
    //DingTalk_MessageTemplate

    public class MessageTemplateEntity : IEntityV2<MessageTemplateEntity>
	{
        [ColumnAttribute("T_Id", IsPrimaryKey = true)]
        /// <summary>
        /// T_Id
        /// </summary>		
        public string T_Id{ get; set; }        
		/// <summary>
		/// 消息模板代码
        /// </summary>		
        public string T_Code{ get; set; }        
		/// <summary>
		/// 消息模板标题
        /// </summary>		
        public string T_Title{ get; set; }        
		/// <summary>
		/// 接收类型：UseridList，DeptList，AllUser
        /// </summary>		
        public string T_RecvType{ get; set; }        
		/// <summary>
		/// 接收者ID列表
        /// </summary>		
        public string T_UseridList{ get; set; }        
		/// <summary>
		/// 接收部门ID列表
        /// </summary>		
        public string T_DeptList{ get; set; }        
		/// <summary>
		/// 消息类型：text，image，file，link，markdown，oa，action_card
        /// </summary>		
        public string T_MsgType{ get; set; }        
		/// <summary>
		/// 消息标题、首屏会话展示内容，类型为link和markdown有效
        /// </summary>		
        public string T_MsgTitle{ get; set; }        
		/// <summary>
		/// 消息文本内容
        /// </summary>		
        public string T_MsgContent{ get; set; }        
		/// <summary>
		/// 消息跳转链接地址，类型为link有效
        /// </summary>		
        public string T_MessageUrl{ get; set; }        
		/// <summary>
		/// 消息图片地址，类型为link有效
        /// </summary>		
        public string T_PicUrl{ get; set; }        
		/// <summary>
		/// 媒体文件ID，类型为image和file有效
        /// </summary>		
        public string T_MediaId{ get; set; }        
		/// <summary>
		/// markdown格式的消息
        /// </summary>		
        public string T_Markdown{ get; set; }        
		/// <summary>
		/// 整体跳转ActionCard样式时的标题
        /// </summary>		
        public string T_SingleTitle{ get; set; }        
		/// <summary>
		/// 消息点击链接地址
        /// </summary>		
        public string T_SingleUrl{ get; set; }        
		/// <summary>
		/// 钉钉用户ID
        /// </summary>		
        public string T_CreatorUserId{ get; set; }        
		/// <summary>
		/// 钉钉用户名称
        /// </summary>		
        public string T_CreatorUserName{ get; set; }        
		/// <summary>
		/// 操作时间
        /// </summary>		
        public DateTime T_CreateTime{ get; set; }        
		/// <summary>
		/// 更新用户
        /// </summary>		
        public string T_ModifyUserId{ get; set; }        
		/// <summary>
		/// 更新时间
        /// </summary>		
        public DateTime T_ModifyTime{ get; set; }        
		/// <summary>
		/// 状态
        /// </summary>		
        public int T_State{ get; set; }     
        /// <summary>
        /// 允许发送次数，0表示一直发送
        /// </summary>
        public int T_SendTimes { get; set; }


    }
}

