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
        [SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string F_Id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [Required(ErrorMessage = "类型不能为空")]
        [SugarColumn(IsNullable = true, ColumnDescription = "类型")]
        public bool? F_Type { get; set; }
        /// <summary>
        /// 起始IP
        /// </summary>
        [Required(ErrorMessage = "起始IP不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_StartIP",ColumnDataType = "nvarchar(50)", ColumnDescription = "起始IP")]
        public string F_StartIP { get; set; }
        /// <summary>
        /// 结束IP
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_EndIP", ColumnDataType = "nvarchar(50)", ColumnDescription = "结束IP")]
        public string F_EndIP { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "排序码")]
        public int? F_SortCode { get; set; }
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
        [SugarColumn(IsNullable = true, ColumnName = "F_Description",ColumnDataType = "longtext,nvarchar(4000)", ColumnDescription = "备注")]
        public string F_Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDescription = "修改时间")]
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
        /// <summary>
        /// 删除时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "到期时间")]
        public DateTime? F_EndTime { get; set; }
    }
}
