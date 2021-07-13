using System;
using System.ComponentModel.DataAnnotations;
using Chloe.Annotations;


namespace WaterCloud.Domain.OrderManagement
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-07-12 20:40
    /// 描 述：订单明细实体类
    /// </summary>
    [TableAttribute("crm_orderdetail")]
    public class OrderDetailEntity : IEntity<OrderDetailEntity>,ICreationAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        [ColumnAttribute("F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public string F_OrderId { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int? F_OrderState { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string F_ProductName { get; set; }
        /// <summary>
        /// 产品规格
        /// </summary>
        public string F_ProductDescription { get; set; }
        /// <summary>
        /// 产品单位
        /// </summary>
        public string F_ProductUnit { get; set; }
        /// <summary>
        /// 需求数量
        /// </summary>
        public int? F_NeedNum { get; set; }
        /// <summary>
        /// 实际数量
        /// </summary>
        public int? F_ActualNum { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string F_Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string F_CreatorUserName { get; set; }
        /// <summary>
        /// 需求时间
        /// </summary>
        public DateTime? F_NeedTime { get; set; }
        /// <summary>
        /// 实际时间
        /// </summary>
        public DateTime? F_ActualTime { get; set; }
    }
}
