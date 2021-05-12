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
						[SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true)]
			public  String  F_Id { get; set; }
					
			public  String  F_ModuleId { get; set; }
					
			public  Boolean?  F_DeleteMark { get; set; }
					
			public  Boolean?  F_EnabledMark { get; set; }
					
			public  String  F_Description { get; set; }
					
			public  DateTime?  F_CreatorTime { get; set; }
					
			public  String  F_CreatorUserId { get; set; }
					
			public  DateTime?  F_LastModifyTime { get; set; }
					
			public  String  F_LastModifyUserId { get; set; }
					
			public  DateTime?  F_DeleteTime { get; set; }
					
			public  String  F_DeleteUserId { get; set; }
		    }
}