using System;
using System.ComponentModel.DataAnnotations;
using Chloe.Annotations;

namespace WaterCloud.Domain.OrderManagement
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-07-12 20:41
    /// 描 述：订单管理实体类
    /// </summary>
    [TableAttribute("crm_order")]
    public class OrderEntity : IEntity<OrderEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        [ColumnAttribute("F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string F_OrderCode { get; set; }
        /// <summary>
        /// 订单状态(0待确认，待采购，1已完成)
        /// </summary>
        public int? F_OrderState { get; set; }
        /// <summary>
        /// 需求时间
        /// </summary>
        public DateTime? F_NeedTime { get; set; }
        /// <summary>
        /// 实际时间
        /// </summary>
        public DateTime? F_ActualTime { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public bool? F_DeleteMark { get; set; }
        /// <summary>
        /// 有效标记
        /// </summary>
        public bool? F_EnabledMark { get; set; }
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
        /// 创建人
        /// </summary>
        public string F_CreatorUserName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? F_LastModifyTime { get; set; }
        /// <summary>
        /// 修改人Id
        /// </summary>
        public string F_LastModifyUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? F_DeleteTime { get; set; }
        /// <summary>
        /// 删除人Id
        /// </summary>
        public string F_DeleteUserId { get; set; }
        [NotMapped]
        public int? F_NeedNum { get; set; }
        [NotMapped]
        public int? F_ActualNum { get; set; }
        [NotMapped]
        public List<OrderDetailEntity> list { get; set; }
    }
}
