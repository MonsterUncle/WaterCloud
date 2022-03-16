/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace WaterCloud.Domain.SystemManage
{
    [SugarTable("sys_module")]
    public class ModuleEntity : IEntity<ModuleEntity>, ICreationAudited, IModificationAudited, IDeleteAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string Id { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        [Required(ErrorMessage = "上级不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "ParentId", ColumnDataType = "nvarchar(50)", ColumnDescription = "父级Id")]
        public string ParentId { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "层级")]
        public int? Layers { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [Required(ErrorMessage = "编号不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "EnCode",ColumnDataType = "nvarchar(50)", ColumnDescription = "编号", UniqueGroupNameList = new string[] { "sys_module" })]
        public string EnCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "FullName", ColumnDataType = "nvarchar(50)", ColumnDescription = "删除人Id")]
        public string FullName { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "Icon",ColumnDataType = "nvarchar(50)", ColumnDescription = "图标")]
        public string Icon { get; set; }
        /// <summary>
        /// Url地址
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "UrlAddress", ColumnDataType = "longtext", ColumnDescription = "Url地址")]
        public string UrlAddress { get; set; }
        /// <summary>
        /// 目标
        /// </summary>
        [Required(ErrorMessage = "目标不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "Target",ColumnDataType = "nvarchar(50)", ColumnDescription = "目标")]
        public string Target { get; set; }
        /// <summary>
        /// 是否是菜单
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "是否是菜单")]
        public bool? IsMenu { get; set; }
        /// <summary>
        /// 是否展开
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否展开")]
        public bool? IsExpand { get; set; }
        /// <summary>
        /// 是否公共
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否公共")]
        public bool? IsPublic { get; set; }
        /// <summary>
        /// 是否字段
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否字段")]
        public bool? IsFields { get; set; }
        /// <summary>
        /// 允许修改
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "允许修改")]
        public bool? AllowEdit { get; set; }
        /// <summary>
        /// 允许删除
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "允许删除")]
        public bool? AllowDelete { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        [Required(ErrorMessage = "排序不能为空")]
        [Range(0, 99999999, ErrorMessage = "排序大小必须介于1~99999999之间")]
        [SugarColumn(IsNullable = true, ColumnDescription = "排序码")]
        public int? SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "删除标记")]
        public bool? DeleteMark { get; set; }
        /// <summary>
        /// 有效标记
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "有效标记")]
        public bool? EnabledMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "Description",ColumnDataType = "longtext", ColumnDescription = "备注")]
        public string Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? CreatorTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "CreatorUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
        public string CreatorUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "修改时间")]
        public DateTime? LastModifyTime { get; set; }
        /// <summary>
        /// 修改人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "LastModifyUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "修改人Id")]
        public string LastModifyUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "删除时间")]
        public DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 删除人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "DeleteUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "删除人Id")]
        public string DeleteUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "Authorize", ColumnDataType = "nvarchar(100)")]
        public string Authorize { get; set; }
    }
}
