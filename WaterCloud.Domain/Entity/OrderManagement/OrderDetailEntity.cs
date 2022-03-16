using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace WaterCloud.Domain.OrderManagement
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-07-12 20:40
    /// 描 述：订单明细实体类
    /// </summary>
    [SugarTable("crm_orderdetail")]
    public class OrderDetailEntity : IEntity<OrderDetailEntity>,ICreationAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(ColumnName="F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnName = "F_OrderId", ColumnDataType = "nvarchar(50)", ColumnDescription = "订单Id", UniqueGroupNameList = new string[] { "crm_orderdetail" })]
        public string F_OrderId { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnName = "F_OrderState", ColumnDescription = "订单状态")]
        public int? F_OrderState { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_ProductName", ColumnDataType = "nvarchar(50)", ColumnDescription = "产品名称", UniqueGroupNameList = new string[] { "crm_orderdetail" })]
        public string F_ProductName { get; set; }
        /// <summary>
        /// 产品规格
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_ProductDescription", ColumnDataType = "nvarchar(100)", ColumnDescription = "产品规格")]
        public string F_ProductDescription { get; set; }
        /// <summary>
        /// 产品单位
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_ProductUnit", ColumnDataType = "nvarchar(5)", ColumnDescription = "产品单位")]
        public string F_ProductUnit { get; set; }
        /// <summary>
        /// 需求数量
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_NeedNum", ColumnDescription = "需求数量")]
        public int? F_NeedNum { get; set; }
        /// <summary>
        /// 实际数量
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_ActualNum", ColumnDescription = "实际数量")]
        public int? F_ActualNum { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_Description", ColumnDataType = "longtext,nvarchar(4000)", ColumnDescription = "备注")]
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
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserName", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人")]
        public string F_CreatorUserName { get; set; }
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
    }
}
