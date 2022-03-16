/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace WaterCloud.Domain.SystemManage
{
    [SugarTable("sys_itemsdetail")]
    public class ItemsDetailEntity : IEntity<ItemsDetailEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string F_Id { get; set; }
        /// <summary>
        /// 分类Id
        /// </summary>
        [Required(ErrorMessage = "分类不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_ItemId", ColumnDataType = "nvarchar(50)", ColumnDescription = "分类Id", UniqueGroupNameList = new string[] { "sys_itemsdetail" })]
        public string F_ItemId { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_ParentId", ColumnDataType = "nvarchar(50)", ColumnDescription = "父级Id", UniqueGroupNameList = new string[] { "sys_itemsdetail" })]
        public string F_ParentId { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [Required(ErrorMessage = "编号不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_ItemCode",ColumnDataType = "nvarchar(50)", ColumnDescription = "编号", UniqueGroupNameList = new string[] { "sys_itemsdetail" })]
        public string F_ItemCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_ItemName", ColumnDataType = "nvarchar(50)", ColumnDescription = "名称")]
        public string F_ItemName { get; set; }
        /// <summary>
        /// 简拼
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_SimpleSpelling", ColumnDataType = "longtext", ColumnDescription = "简拼")]
        public string F_SimpleSpelling { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否默认")]
        public bool? F_IsDefault { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "层级")]
        public int? F_Layers { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        [Required(ErrorMessage = "排序不能为空")]
        [Range(0, 99999999, ErrorMessage = "排序大小必须介于1~99999999之间")]
        [SugarColumn(IsNullable = true, ColumnDescription = "排序码")]
        public int? F_SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "删除标记")]
        public bool? F_DeleteMark { get; set; }
        /// <summary>
        /// 有效标记
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "有效标记")]
        public bool? F_EnabledMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_Description",ColumnDataType = "longtext", ColumnDescription = "备注")]
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
    }
}
