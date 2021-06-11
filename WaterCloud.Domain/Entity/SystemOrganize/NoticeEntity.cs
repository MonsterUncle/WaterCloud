//-----------------------------------------------------------------------
// <copyright file=" Notice.cs" company="WaterCloud">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: Notice.cs
// * history : Created by T4 04/13/2020 16:51:21 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace WaterCloud.Domain.SystemOrganize
{
	/// <summary>
	/// Notice Entity Model
	/// </summary>
	[SugarTable("sys_notice")]
    public class NoticeEntity : IEntity<NoticeEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
		/// <summary>
		/// 主键Id
		/// </summary>
		[SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
		public string F_Id { get; set; }
		/// <summary>
		/// 标题
		/// </summary>
		[Required(ErrorMessage = "标题不能为空")]
		[SugarColumn(IsNullable = true, ColumnName = "F_Title",ColumnDataType = "nvarchar(50)", ColumnDescription = "标题", UniqueGroupNameList = new string[] { "sys_notice" })]
		public string F_Title { get; set; }
		/// <summary>
		/// 内容
		/// </summary>
		[Required(ErrorMessage = "内容不能为空")]
		[SugarColumn(IsNullable = true, ColumnName = "F_Content", ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "内容")]
		public string F_Content { get; set; }
		/// <summary>
		/// 删除标记
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "删除标记")]
		public Boolean? F_DeleteMark { get; set; }
		/// <summary>
		/// 有效标记
		/// </summary>
		[SugarColumn(IsNullable = true,ColumnDescription = "有效标记")]
		public Boolean? F_EnabledMark { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_Description",ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "删除人Id")]
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
		/// 创建人姓名
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserName", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人姓名")]
		public string F_CreatorUserName { get; set; }
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