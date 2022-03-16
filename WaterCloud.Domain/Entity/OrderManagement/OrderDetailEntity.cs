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
        [SugarColumn(ColumnName="Id", IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnName = "OrderId", ColumnDataType = "nvarchar(50)", ColumnDescription = "订单Id", UniqueGroupNameList = new string[] { "crm_orderdetail" })]
        public string OrderId { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnName = "OrderState", ColumnDescription = "订单状态")]
        public int? OrderState { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "ProductName", ColumnDataType = "nvarchar(50)", ColumnDescription = "产品名称", UniqueGroupNameList = new string[] { "crm_orderdetail" })]
        public string ProductName { get; set; }
        /// <summary>
        /// 产品规格
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "ProductDescription", ColumnDataType = "nvarchar(100)", ColumnDescription = "产品规格")]
        public string ProductDescription { get; set; }
        /// <summary>
        /// 产品单位
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "ProductUnit", ColumnDataType = "nvarchar(5)", ColumnDescription = "产品单位")]
        public string ProductUnit { get; set; }
        /// <summary>
        /// 需求数量
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "NeedNum", ColumnDescription = "需求数量")]
        public int? NeedNum { get; set; }
        /// <summary>
        /// 实际数量
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "ActualNum", ColumnDescription = "实际数量")]
        public int? ActualNum { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "Description", ColumnDataType = "longtext", ColumnDescription = "备注")]
        public string Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? CreatorTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
        public string CreatorUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "CreatorUserName", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人")]
        public string CreatorUserName { get; set; }
        /// <summary>
        /// 需求时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "需求时间")]
        public DateTime? NeedTime { get; set; }
        /// <summary>
        /// 实际时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "实际时间")]
        public DateTime? ActualTime { get; set; }
    }
}
