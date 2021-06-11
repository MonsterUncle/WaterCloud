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
		/// ����Id
		/// </summary>
		[SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true,ColumnDescription ="����Id")]
		public  string  F_Id { get; set; }
		/// <summary>
		/// ģ��Id
		/// </summary>					
		[SugarColumn(IsNullable = true, ColumnName = "F_ModuleId",ColumnDataType = "nvarchar(50)", ColumnDescription = "ģ��Id", UniqueGroupNameList = new string[] { "sys_quickmodule" })]
		public  string  F_ModuleId { get; set; }
		/// <summary>
		/// ɾ�����
		/// </summary>					
		[SugarColumn(IsNullable = true, ColumnDescription = "ɾ�����")]
		public  Boolean?  F_DeleteMark { get; set; }
		/// <summary>
		/// ��Ч���
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "��Ч���")]
		public  Boolean?  F_EnabledMark { get; set; }
		/// <summary>
		/// ��ע
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_Description", ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "��ע")]
		public  string  F_Description { get; set; }
		/// <summary>
		/// ����ʱ��
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "����ʱ��")]
		public  DateTime?  F_CreatorTime { get; set; }
		/// <summary>
		/// ������Id
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "������Id", UniqueGroupNameList = new string[] { "sys_quickmodule" })]
		public  string  F_CreatorUserId { get; set; }
		/// <summary>
		/// �޸�ʱ��
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "�޸�ʱ��")]
		public  DateTime?  F_LastModifyTime { get; set; }
		/// <summary>
		/// �޸���Id
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_LastModifyUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "�޸���Id")]
		public  string  F_LastModifyUserId { get; set; }
		/// <summary>
		/// ɾ��ʱ��
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "ɾ��ʱ��")]
		public  DateTime?  F_DeleteTime { get; set; }
		/// <summary>
		/// ɾ����Id
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_DeleteUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "ɾ����Id")]
		public  string  F_DeleteUserId { get; set; }
	}
}