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
		[SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true)]
		public String F_Id { get; set; }
		[Required(ErrorMessage = "标题不能为空")]
		public String F_Title { get; set; }
		[Required(ErrorMessage = "内容不能为空")]
		public String F_Content { get; set; }

		public Boolean? F_DeleteMark { get; set; }

		public Boolean? F_EnabledMark { get; set; }

		public String F_Description { get; set; }

		public DateTime? F_CreatorTime { get; set; }

		public String F_CreatorUserId { get; set; }
		public String F_CreatorUserName { get; set; }
		public DateTime? F_LastModifyTime { get; set; }

		public String F_LastModifyUserId { get; set; }

		public DateTime? F_DeleteTime { get; set; }

		public String F_DeleteUserId { get; set; }
	}
}