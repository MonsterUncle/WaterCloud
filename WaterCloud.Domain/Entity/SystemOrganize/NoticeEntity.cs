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
		[SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
		public string Id { get; set; }
		/// <summary>
		/// 标题
		/// </summary>
		[Required(ErrorMessage = "标题不能为空")]
		[SugarColumn(IsNullable = true, ColumnName = "Title",ColumnDataType = "nvarchar(50)", ColumnDescription = "标题", UniqueGroupNameList = new string[] { "sys_notice" })]
		public string Title { get; set; }
		/// <summary>
		/// 内容
		/// </summary>
		[Required(ErrorMessage = "内容不能为空")]
		[SugarColumn(IsNullable = true, ColumnName = "Content", ColumnDataType = "longtext", ColumnDescription = "内容")]
		public string Content { get; set; }
		/// <summary>
		/// 删除标记
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "删除标记")]
		public Boolean? DeleteMark { get; set; }
		/// <summary>
		/// 有效标记
		/// </summary>
		[SugarColumn(IsNullable = true,ColumnDescription = "有效标记")]
		public Boolean? EnabledMark { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "Description",ColumnDataType = "longtext", ColumnDescription = "删除人Id")]
		public string Description { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
		public DateTime? CreatorTime { get; set; }
		/// <summary>
		/// 创建人Id
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
		public string CreatorUserId { get; set; }
		/// <summary>
		/// 创建人姓名
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "CreatorUserName", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人姓名")]
		public string CreatorUserName { get; set; }
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
		[SugarColumn(IsNullable = true, ColumnDescription = "删除时间")]
		public DateTime? DeleteTime { get; set; }
		/// <summary>
		/// 删除人Id
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "DeleteUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "删除人Id")]
		public string DeleteUserId { get; set; }
	}
}