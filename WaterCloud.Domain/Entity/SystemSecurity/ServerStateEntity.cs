//-----------------------------------------------------------------------
// <copyright file=" ServerState.cs" company="WaterCloud">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: ServerState.cs
// * history : Created by T4 04/13/2020 11:54:49
// </copyright>
//-----------------------------------------------------------------------
using SqlSugar;
using System;

namespace WaterCloud.Domain.SystemSecurity
{
	/// <summary>
	/// ServerState Entity Model
	/// </summary>
	[SugarTable("sys_serverstate")]
	public class ServerStateEntity : IEntity<ServerStateEntity>
	{
		/// <summary>
		/// ����Id
		/// </summary>
		[SugarColumn(ColumnName = "F_Id", IsPrimaryKey = true, ColumnDescription = "����Id")]
		public string F_Id { get; set; }

		/// <summary>
		/// ��վվ��
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_WebSite", ColumnDataType = "nvarchar(200)", ColumnDescription = "��վվ��")]
		public string F_WebSite { get; set; }

		/// <summary>
		/// ARM
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_ARM", ColumnDataType = "nvarchar(50)", ColumnDescription = "ARM")]
		public string F_ARM { get; set; }

		/// <summary>
		/// CPU
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_CPU", ColumnDataType = "nvarchar(50)", ColumnDescription = "CPU")]
		public string F_CPU { get; set; }

		/// <summary>
		/// IIS
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "F_IIS", ColumnDataType = "nvarchar(50)", ColumnDescription = "IIS")]
		public string F_IIS { get; set; }

		/// <summary>
		/// ����
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "����")]
		public DateTime F_Date { get; set; }

		/// <summary>
		/// ����
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "����")]
		public int F_Cout { get; set; }
	}
}