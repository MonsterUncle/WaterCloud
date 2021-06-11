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
		/// ����Id
		/// </summary>
		[SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true,ColumnDescription ="����Id")]
		public string F_Id { get; set; }
		/// <summary>
		/// ����
		/// </summary>
		[Required(ErrorMessage = "���ⲻ��Ϊ��")]
		[SugarColumn(IsNullable = true, ColumnName = "F_Title",ColumnDataType = "nvarchar(50)", ColumnDescription = "����", UniqueGroupNameList = new string[] { "sys_notice" })]
		public string F_Title { get; set; }
		/// <summary>
		/// ����
		/// </summary>
		[Required(ErrorMessage = "���ݲ���Ϊ��")]
		[SugarColumn(IsNullable = true, ColumnName = "F_Content", ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "����")]
		public string F_Content { get; set; }
		/// <summary>
		/// ɾ�����
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "ɾ�����")]
		public Boolean? F_DeleteMark { get; set; }
		/// <summary>
		/// ��Ч���
		/// </summary>
		[SugarColumn(IsNullable = true,ColumnDescription = "��Ч���")]
		public Boolean? F_EnabledMark { get; set; }
		/// <summary>
		/// ��ע
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_Description",ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "ɾ����Id")]
		public string F_Description { get; set; }
		/// <summary>
		/// ����ʱ��
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "����ʱ��")]
		public DateTime? F_CreatorTime { get; set; }
		/// <summary>
		/// ������Id
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "������Id")]
		public string F_CreatorUserId { get; set; }
		/// <summary>
		/// ����������
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserName", ColumnDataType = "nvarchar(50)", ColumnDescription = "����������")]
		public string F_CreatorUserName { get; set; }
		/// <summary>
		/// �޸�ʱ��
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "�޸�ʱ��")]
		public DateTime? F_LastModifyTime { get; set; }
		/// <summary>
		/// �޸���Id
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_LastModifyUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "�޸���Id")]
		public string F_LastModifyUserId { get; set; }
		/// <summary>
		/// ɾ��ʱ��
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "ɾ��ʱ��")]
		public DateTime? F_DeleteTime { get; set; }
		/// <summary>
		/// ɾ����Id
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_DeleteUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "ɾ����Id")]
		public string F_DeleteUserId { get; set; }
	}
}