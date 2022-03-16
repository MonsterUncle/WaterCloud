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
		[SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="����Id")]
		public  string  Id { get; set; }
		/// <summary>
		/// ģ��Id
		/// </summary>					
		[SugarColumn(IsNullable = true, ColumnName = "ModuleId",ColumnDataType = "nvarchar(50)", ColumnDescription = "ģ��Id", UniqueGroupNameList = new string[] { "sys_quickmodule" })]
		public  string  ModuleId { get; set; }
		/// <summary>
		/// ɾ�����
		/// </summary>					
		[SugarColumn(IsNullable = true, ColumnDescription = "ɾ�����")]
		public  Boolean?  DeleteMark { get; set; }
		/// <summary>
		/// ��Ч���
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "��Ч���")]
		public  Boolean?  EnabledMark { get; set; }
		/// <summary>
		/// ��ע
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "Description", ColumnDataType = "longtext", ColumnDescription = "��ע")]
		public  string  Description { get; set; }
		/// <summary>
		/// ����ʱ��
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "����ʱ��")]
		public  DateTime?  CreatorTime { get; set; }
		/// <summary>
		/// ������Id
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "������Id", UniqueGroupNameList = new string[] { "sys_quickmodule" })]
		public  string  CreatorUserId { get; set; }
		/// <summary>
		/// �޸�ʱ��
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "�޸�ʱ��")]
		public  DateTime?  LastModifyTime { get; set; }
		/// <summary>
		/// �޸���Id
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "LastModifyUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "�޸���Id")]
		public  string  LastModifyUserId { get; set; }
		/// <summary>
		/// ɾ��ʱ��
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "ɾ��ʱ��")]
		public  DateTime?  DeleteTime { get; set; }
		/// <summary>
		/// ɾ����Id
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "DeleteUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "ɾ����Id")]
		public  string  DeleteUserId { get; set; }
	}
}