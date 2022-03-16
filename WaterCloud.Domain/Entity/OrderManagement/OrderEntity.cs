using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace WaterCloud.Domain.OrderManagement
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-07-12 20:41
    /// 描 述：订单管理实体类
    /// </summary>
    [SugarTable("crm_order")]
    public class OrderEntity : IEntity<OrderEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(ColumnName="F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_OrderCode", ColumnDataType = "nvarchar(50)", ColumnDescription = "订单编号", UniqueGroupNameList = new string[] { "crm_order" })]
        public string F_OrderCode { get; set; }
        /// <summary>
        /// 订单状态(0待确认，待采购，1已完成)
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDescription = "订单状态")]
        public int? F_OrderState { get; set; }
        /// <summary>
        /// 需求时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "需求时间")]
        public DateTime? F_NeedTime { get; set; }
        /// <summary>
        /// 实际时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "实际时间")]
        public DateTime? F_ActualTime { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除标记")]
        public bool? F_DeleteMark { get; set; }
        /// <summary>
        /// 有效标记
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "有效标记")]
        public bool? F_EnabledMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_Description", ColumnDataType = "longtext", ColumnDescription = "备注")]
        public string F_Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserName", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人")]
        public string F_CreatorUserName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "修改时间")]
        public DateTime? F_LastModifyTime { get; set; }
        /// <summary>
        /// 修改人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_LastModifyUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "修改人Id")]
        public string F_LastModifyUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除时间")]
        public DateTime? F_DeleteTime { get; set; }
        /// <summary>
        /// 删除人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_DeleteUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "删除人Id")]
        public string F_DeleteUserId { get; set; }
        [SugarColumn(IsIgnore = true)]
        public int? F_NeedNum { get; set; }
        [SugarColumn(IsIgnore = true)]
        public int? F_ActualNum { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<OrderDetailEntity> list { get; set; }
    }
}
