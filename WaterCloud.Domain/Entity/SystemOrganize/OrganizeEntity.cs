/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/

using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace WaterCloud.Domain.SystemOrganize
{
	/// <summary>
	/// 机构实体
	/// </summary>
	[SugarTable("sys_organize")]
	public class OrganizeEntity : IEntity<OrganizeEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
	{
		/// <summary>
		/// 主键Id
		/// </summary>
		[SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true, ColumnDescription = "主键Id")]
		public string F_Id { get; set; }

		/// <summary>
		/// 父级Id
		/// </summary>
		[Required(ErrorMessage = "父级不能为空")]
		[SugarColumn(IsNullable = true, ColumnName = "F_ParentId", ColumnDataType = "nvarchar(50)", ColumnDescription = "父级Id")]
		public string F_ParentId { get; set; }

		/// <summary>
		/// 层级
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_Layers", ColumnDescription = "层级")]
		public int? F_Layers { get; set; }

		/// <summary>
		/// 编号
		/// </summary>
		[Required(ErrorMessage = "编号不能为空")]
		[SugarColumn(IsNullable = true, ColumnName = "F_EnCode", ColumnDataType = "nvarchar(50)", ColumnDescription = "编号", UniqueGroupNameList = new string[] { "sys_organize" })]
		public string F_EnCode { get; set; }

		/// <summary>
		/// 全名称
		/// </summary>
		[Required(ErrorMessage = "名称不能为空")]
		[SugarColumn(IsNullable = true, ColumnName = "F_FullName", ColumnDataType = "nvarchar(50)", ColumnDescription = "全名称")]
		public string F_FullName { get; set; }

		/// <summary>
		/// 简称
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_ShortName", ColumnDataType = "nvarchar(50)", ColumnDescription = "简称")]
		public string F_ShortName { get; set; }

		/// <summary>
		/// 类型
		/// </summary>
		[Required(ErrorMessage = "类型不能为空")]
		[SugarColumn(IsNullable = true, ColumnName = "F_CategoryId", ColumnDataType = "nvarchar(50)", ColumnDescription = "类型")]
		public string F_CategoryId { get; set; }

		/// <summary>
		///
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_ManagerId", ColumnDataType = "nvarchar(50)")]
		public string F_ManagerId { get; set; }

		/// <summary>
		/// 电话
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_TelePhone", ColumnDataType = "nvarchar(20)", ColumnDescription = "电话")]
		public string F_TelePhone { get; set; }

		/// <summary>
		/// 手机
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_MobilePhone", ColumnDataType = "nvarchar(20)", ColumnDescription = "手机")]
		public string F_MobilePhone { get; set; }

		/// <summary>
		/// 微信号
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_WeChat", ColumnDataType = "nvarchar(50)", ColumnDescription = "微信号")]
		public string F_WeChat { get; set; }

		/// <summary>
		/// 传真号
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_Fax", ColumnDataType = "nvarchar(20)", ColumnDescription = "传真号")]
		public string F_Fax { get; set; }

		/// <summary>
		/// 邮箱
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_Email", ColumnDataType = "nvarchar(50)", ColumnDescription = "邮箱")]
		public string F_Email { get; set; }

		/// <summary>
		/// 区域Id
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_AreaId", ColumnDataType = "nvarchar(50)", ColumnDescription = "区域Id")]
		public string F_AreaId { get; set; }

		/// <summary>
		/// 地址
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_Address", ColumnDataType = "longtext", ColumnDescription = "地址")]
		public string F_Address { get; set; }

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
		[SugarColumn(IsNullable = true, ColumnName = "F_Description", ColumnDataType = "longtext", ColumnDescription = "备注")]
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

		[SugarColumn(IsIgnore = true)]
		public bool LAY_CHECKED { get; set; }
	}
}