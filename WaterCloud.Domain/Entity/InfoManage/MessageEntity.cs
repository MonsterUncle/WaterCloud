using System;
using System.ComponentModel.DataAnnotations;
using Serenity.Data.Mapping;
using SqlSugar;

namespace WaterCloud.Domain.InfoManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-07-29 16:41
    /// 描 述：通知管理实体类
    /// </summary>
    [SugarTable("oms_message")]
    public class MessageEntity : IEntity<MessageEntity>,ICreationAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string F_Id { get; set; }
        /// <summary>
        /// 信息类型（通知、私信、处理）
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "信息类型不能为空")]
        [SugarColumn(IsNullable = true,  ColumnDescription = "信息类型")]
        public int? F_MessageType { get; set; }
        /// <summary>
        /// 收件人主键
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_ToUserId",ColumnDataType = "longtext,nvarchar(4000)", ColumnDescription = "收件人主键")]
        public string F_ToUserId { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_ToUserName", ColumnDataType = "longtext,nvarchar(4000)", ColumnDescription = "收件人")]
        public string F_ToUserName { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "内容不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_MessageInfo",ColumnDataType = "longtext,nvarchar(4000)", ColumnDescription = "内容")]
        public string F_MessageInfo { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "有效")]
        public bool? F_EnabledMark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建用户主键")]
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserName",ColumnDataType = "nvarchar(50)", ColumnDescription = "创建用户")]
        public string F_CreatorUserName { get; set; }
        /// <summary>
        /// 跳转类型
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_HrefTarget", ColumnDataType = "nvarchar(50)", ColumnDescription = "跳转类型")]
        public string F_HrefTarget { get; set; }
        /// <summary>
        /// 跳转地址
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_Href", ColumnDataType = "nvarchar(100)", ColumnDescription = "跳转地址")]
        public string F_Href { get; set; }
        /// <summary>
        /// 待办关联键
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_KeyValue", ColumnDataType = "nvarchar(50)", ColumnDescription = "待办关联键")]
        public string F_KeyValue { get; set; }
        /// <summary>
        /// 点击已读
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "点击已读")]
        public bool? F_ClickRead { get; set; }
        [SugarColumn(IsIgnore=true)]
        public string companyId { get; set; }
    }
}
