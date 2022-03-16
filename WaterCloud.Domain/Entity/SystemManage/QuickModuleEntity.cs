//-----------------------------------------------------------------------
// <copyright file=" QuickModule.cs" company="WaterCloud">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: QuickModule.cs
// * history : Created by T4 04/13/2020 16:51:14 
// </copyright>
//-----------------------------------------------------------------------
using System;
using SqlSugar;

namespace WaterCloud.Domain.SystemManage
{
	/// <summary>
	/// QuickModule Entity Model
	/// </summary>
	[SugarTable("sys_quickmodule")]
    public class QuickModuleEntity : IEntity<QuickModuleEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
		/// <summary>
		/// 主键Id
		/// </summary>
		[SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
		public  string  Id { get; set; }
		/// <summary>
		/// 模块Id
		/// </summary>					
		[SugarColumn(IsNullable = true, ColumnName = "ModuleId",ColumnDataType = "nvarchar(50)", ColumnDescription = "模块Id", UniqueGroupNameList = new string[] { "sys_quickmodule" })]
		public  string  ModuleId { get; set; }
		/// <summary>
		/// 删除标记
		/// </summary>					
		[SugarColumn(IsNullable = true, ColumnDescription = "删除标记")]
		public  Boolean?  DeleteMark { get; set; }
		/// <summary>
		/// 有效标记
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "有效标记")]
		public  Boolean?  EnabledMark { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "Description", ColumnDataType = "longtext", ColumnDescription = "备注")]
		public  string  Description { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
		public  DateTime?  CreatorTime { get; set; }
		/// <summary>
		/// 创建人Id
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id", UniqueGroupNameList = new string[] { "sys_quickmodule" })]
		public  string  CreatorUserId { get; set; }
		/// <summary>
		/// 修改时间
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "修改时间")]
		public  DateTime?  LastModifyTime { get; set; }
		/// <summary>
		/// 修改人Id
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "LastModifyUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "修改人Id")]
		public  string  LastModifyUserId { get; set; }
		/// <summary>
		/// 删除时间
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "删除时间")]
		public  DateTime?  DeleteTime { get; set; }
		/// <summary>
		/// 删除人Id
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "DeleteUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "删除人Id")]
		public  string  DeleteUserId { get; set; }
	}
}