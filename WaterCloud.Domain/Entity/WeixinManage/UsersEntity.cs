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
	 	//微信用户基本表
    [TableAttribute("Wct_Users")]

    public class UsersEntity : IEntityV2<UsersEntity>  
	{
        [ColumnAttribute("uuId", IsPrimaryKey = true)]
        /// <summary>
        /// 主键
        /// </summary>		
        public string uuId { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>		
        public string OpenId{ get; set; }        
		/// <summary>
		/// 昵称
        /// </summary>		
        public string Nickname{ get; set; }    
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 性别
        /// </summary>		
        public string Sex{ get; set; }        
		/// <summary>
		/// 所在城市
        /// </summary>		
        public string City{ get; set; }        
		/// <summary>
		/// 所在国家
        /// </summary>		
        public string Country{ get; set; }        
		/// <summary>
		/// 所在省份
        /// </summary>		
        public string Province{ get; set; }        
		/// <summary>
		/// 用户的语言，简体中文为zh_CN
        /// </summary>		
        public string Language{ get; set; }        
		/// <summary>
		/// 用户头像
        /// </summary>		
        public string HeadImgUrl{ get; set; }        
		/// <summary>
		/// 用户关注时间，为时间戳
        /// </summary>		
        public string SubscribeTime{ get; set; }        
		/// <summary>
		/// T_Unionid
        /// </summary>		
        public string Unionid{ get; set; }        
		/// <summary>
		/// 创建时间
        /// </summary>		
        public DateTime CreateDate{ get; set; }        
		/// <summary>
		/// 分组ID
        /// </summary>		
        public int GroupId{ get; set; }        
		/// <summary>
		/// 更新时间
        /// </summary>		
        public DateTime UpdateDate{ get; set; }        
		/// <summary>
		/// 当前状态
        /// </summary>		
        public int Status{ get; set; }
        /// <summary>
		/// 微信公众账号ID
        /// </summary>		
        public string WeixinId { get; set; }
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

