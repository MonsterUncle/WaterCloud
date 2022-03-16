/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace WaterCloud.Domain.SystemSecurity
{
    /// <summary>
    /// IP过滤实体
    /// </summary>
    [SugarTable("sys_filterip")]
    public class FilterIPEntity : IEntity<FilterIPEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string Id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [Required(ErrorMessage = "类型不能为空")]
        [SugarColumn(IsNullable = true, ColumnDescription = "类型")]
        public bool? Type { get; set; }
        /// <summary>
        /// 起始IP
        /// </summary>
        [Required(ErrorMessage = "起始IP不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "StartIP",ColumnDataType = "nvarchar(50)", ColumnDescription = "起始IP")]
        public string StartIP { get; set; }
        /// <summary>
        /// 结束IP
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "EndIP", ColumnDataType = "nvarchar(50)", ColumnDescription = "结束IP")]
        public string EndIP { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "排序码")]
        public int? SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除标记")]
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
        [SugarColumn(IsNullable = false, ColumnDescription = "修改时间")]
        public DateTime? LastModifyTime { get; set; }
        /// <summary>
        /// 修改人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "LastModifyUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "修改人Id")]
        public string LastModifyUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除时间")]
        public DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 删除人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "DeleteUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "删除人Id")]
        public string DeleteUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "到期时间")]
        public DateTime? EndTime { get; set; }
    }
}
