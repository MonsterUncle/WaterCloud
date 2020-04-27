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
    [TableAttribute("Dtk_User")]
    //DingTalk_UserInfo
    public class DingTalkUserEntity
    {
        [ColumnAttribute("UserId", IsPrimaryKey = true)]
        /// <summary>
        /// 用户ID
        /// </summary>		
        public string UserId { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>		
        public string OpenId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>		
        public string UserName { get; set; }        
		/// <summary>
		/// 是否激活
        /// </summary>		
        public bool Active{ get; set; }        
		/// <summary>
		/// 部门中的排序
        /// </summary>		
        public string OrderInDepts{ get; set; }        
		/// <summary>
		/// 是否管理员
        /// </summary>		
        public bool IsAdmin{ get; set; }        
		/// <summary>
		/// 是否老板
        /// </summary>		
        public bool IsBoss{ get; set; }        
		/// <summary>
		/// Unionid
        /// </summary>		
        public byte[] Unionid{ get; set; }        
		/// <summary>
		/// 是否部门主管
        /// </summary>		
        public bool IsLeaderInDepts{ get; set; }        
		/// <summary>
		/// 是否领导
        /// </summary>		
        public bool IsLeader{ get; set; }        
		/// <summary>
		/// 是否隐藏
        /// </summary>		
        public bool IsHide{ get; set; }        
		/// <summary>
		/// 部门
        /// </summary>		
        public string Department{ get; set; }        
		/// <summary>
		/// 职位
        /// </summary>		
        public string Position{ get; set; }        
		/// <summary>
		/// 头像
        /// </summary>		
        public string Avatar{ get; set; }        
		/// <summary>
		/// 入职时间（时间戳）
        /// </summary>		
        public string HiredDate{ get; set; }        
		/// <summary>
		/// 员工ID
        /// </summary>		
        public string JobNumber{ get; set; }        
		/// <summary>
		/// 是否高管
        /// </summary>		
        public bool IsSenior{ get; set; }        
		/// <summary>
		/// 邮箱号
        /// </summary>		
        public string Email{ get; set; }        
		/// <summary>
		/// 手机号码
        /// </summary>		
        public string Mobile{ get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>		
        public string OrganizeId { get; set; }
        /// <summary>
		/// 系统角色
        /// </summary>		
        public string RoleId { get; set; }
        /// <summary>
		/// 系统账号
        /// </summary>		
        public string SysUserId { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
}
}

