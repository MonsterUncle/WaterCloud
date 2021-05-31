/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;
using Serenity.Data.Mapping;

namespace WaterCloud.Domain.SystemOrganize
{
    /// <summary>
    /// 角色实体
    /// </summary>
    [SugarTable("sys_role")]
    public class RoleEntity : IEntity<RoleEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        /// <summary>
        /// 主键Id 
        /// </summary>
        [SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string F_Id { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        [Required(ErrorMessage = "公司不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_OrganizeId", ColumnDataType = "nvarchar(50)", ColumnDescription = "公司Id")]
        public string F_OrganizeId { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "类型")]
        public int? F_Category { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [Required(ErrorMessage = "编号不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_EnCode",ColumnDataType = "nvarchar(50)", ColumnDescription = "编号")]
        public string F_EnCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_FullName", ColumnDataType = "nvarchar(50)", ColumnDescription = "名称")]
        public string F_FullName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_Type", ColumnDataType = "nvarchar(50)", ColumnDescription = "类型")]
        public string F_Type { get; set; }
        /// <summary>
        /// 允许修改
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "允许修改")]
        public bool? F_AllowEdit { get; set; }
        /// <summary>
        /// 允许删除
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "允许删除")]
        public bool? F_AllowDelete { get; set; }
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
        [SugarColumn(IsNullable = true, ColumnName = "F_Description", ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "备注")]
        public string F_Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "创建时间")]
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "修改时间")]
        public DateTime? F_LastModifyTime { get; set; }
        /// <summary>
        /// 修改人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_LastModifyUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "修改人Id")]
        public string F_LastModifyUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除时间")]
        public DateTime? F_DeleteTime { get; set; }
        /// <summary>
        /// 删除人Id 
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_DeleteUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "删除人Id")]
        public string F_DeleteUserId { get; set; }
        [SugarColumn(IsIgnore=true)]
        //tablecheck字段
        public bool LAY_CHECKED { get; set; }
    }
}
