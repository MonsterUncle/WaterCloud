/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using SqlSugar;
using System.ComponentModel.DataAnnotations;
using System;

namespace WaterCloud.Domain.SystemManage
{
    [SugarTable("sys_modulebutton")]
    public class ModuleButtonEntity : IEntity<ModuleButtonEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string F_Id { get; set; }
        /// <summary>
        /// 模块Id
        /// </summary>
        [Required(ErrorMessage = "模块不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_ModuleId", ColumnDataType = "nvarchar(50)", ColumnDescription = "模块Id", UniqueGroupNameList = new string[] { "sys_modulebutton" })]
        public string F_ModuleId { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_ParentId",ColumnDataType = "nvarchar(50)", ColumnDescription = "父级", UniqueGroupNameList = new string[] { "sys_modulebutton" })]
        public string F_ParentId { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "层级")]
        public int? F_Layers { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [Required(ErrorMessage = "编号不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_EnCode",ColumnDataType = "nvarchar(50)", ColumnDescription = "编号", UniqueGroupNameList = new string[] { "sys_modulebutton" })]
        public string F_EnCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_FullName", ColumnDataType = "nvarchar(50)", ColumnDescription = "名称")]
        public string F_FullName { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_Icon", ColumnDataType = "nvarchar(50)", ColumnDescription = "图标")]
        public string F_Icon { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        [Required(ErrorMessage = "位置不能为空")]
        [SugarColumn(IsNullable = true,ColumnDescription = "位置")]
        public int? F_Location { get; set; }
        /// <summary>
        /// 事件
        /// </summary>
        [Required(ErrorMessage = "事件不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_JsEvent", ColumnDataType = "nvarchar(50)", ColumnDescription = "事件")]
        public string F_JsEvent { get; set; }
        /// <summary>
        /// Url地址
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_UrlAddress", ColumnDataType = "longtext,nvarchar(4000)", ColumnDescription = "Url地址")]
        public string F_UrlAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? F_Split { get; set; }
        /// <summary>
        /// 是否公共
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否公共")]
        public bool? F_IsPublic { get; set; }
        /// <summary>
        /// 是否允许修改
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否允许修改")]
        public bool? F_AllowEdit { get; set; }
        /// <summary>
        /// 是否允许删除
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "是否允许删除")]
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
        [SugarColumn(IsNullable = true, ColumnName = "F_DeleteUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "删除人Id")]
        public string F_DeleteUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_Authorize",ColumnDataType = "nvarchar(200)", ColumnDescription = "权限标识")]
        public string F_Authorize { get; set; }
    }
}
