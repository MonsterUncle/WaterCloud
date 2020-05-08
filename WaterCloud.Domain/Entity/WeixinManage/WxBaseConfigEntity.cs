/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using Chloe.Annotations;
using System;

namespace WaterCloud.Domain.WeixinManage
{
    [TableAttribute("Wct_WxAppConfig")]

    public class WxBaseConfigEntity : IEntityV2<WxBaseConfigEntity>, ICreationAuditedV2, IModificationAuditedV2
    {
        [ColumnAttribute("uuId", IsPrimaryKey = true)]
        /// <summary>
        /// 主键
        /// </summary>		s
        public string uuId { get; set; }
        /// <summary>
        /// 微信名称
        /// </summary>
        public string WxName { get; set; }
        /// <summary>
        /// 微信原始ID
        /// </summary>
        public string WxId { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        public string WxCode { get; set; }
        /// <summary>
        /// 微信头像
        /// </summary>
        public string HeadPic { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// WxToken值
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// ApiUrl
        /// </summary>
        public string ApiUrl { get; set; }
        /// <summary>
        /// _AppId
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// AppSecret
        /// </summary>
        public string Access_Token { get; set; }
        /// <summary>
        /// T_Encoding
        /// </summary>
        public string Encoding { get; set; }
        /// <summary>
        /// 微信类型（0，订阅号，1服务号，2企业号）
        /// </summary>
        public int WxType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 是否关闭自动回复
        /// </summary>
        public bool CloseKW { get; set; }
        /// <summary>
        /// 拓展字段
        /// </summary>
        public string ExtStr { get; set; }
        /// <summary>
        /// 拓展字段2
        /// </summary>
        public string ExtStr2 { get; set; }
        /// <summary>
        /// 拓展字段3
        /// </summary>
        public string ExtStr3 { get; set; }
        /// <summary>
        /// 微信粉丝更新日期
        /// </summary>
        public DateTime? PersonSynDate { get; set; }
        /// <summary>
        /// 更新的粉丝数
        /// </summary>
        public int? OpenidCount { get; set; }
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
