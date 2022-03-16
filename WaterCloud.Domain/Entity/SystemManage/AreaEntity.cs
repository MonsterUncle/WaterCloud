/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;
using Serenity.Data.Mapping;

namespace WaterCloud.Domain.SystemManage
{
    [SugarTable("sys_area")]
    public class AreaEntity : IEntity<AreaEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string Id { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        [Required(ErrorMessage = "父级不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "ParentId",ColumnDataType = "nvarchar(50)", ColumnDescription = "父级Id")]
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
        [SugarColumn(IsNullable = true, ColumnName = "EnCode", ColumnDataType = "nvarchar(50)", ColumnDescription = "编号", UniqueGroupNameList = new string[] { "sys_area" })]
        public string EnCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "FullName", ColumnDataType = "nvarchar(50)", ColumnDescription = "名称")]
        public string FullName { get; set; }
        /// <summary>
        /// 简拼
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnName = "SimpleSpelling",ColumnDataType = "nvarchar(50)", ColumnDescription = "简拼")]
        public string SimpleSpelling { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Required(ErrorMessage = "排序不能为空")]
        [Range(0, 999999999999, ErrorMessage = "排序大小必须介于1~999999999999之间")]
        [SugarColumn(IsNullable = true, ColumnDescription = "排序")]
        public long? SortCode { get; set; }
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
        [SugarColumn(IsNullable = true, ColumnName = "CreatorUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
        public string CreatorUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "修改时间")]
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

        [SugarColumn(IsIgnore=true)]
        //使用懒加载加此字段
        public bool haveChild { get; set; }
    }
}
