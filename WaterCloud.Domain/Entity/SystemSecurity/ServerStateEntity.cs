//-----------------------------------------------------------------------
// <copyright file=" ServerState.cs" company="WaterCloud">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: ServerState.cs
// * history : Created by T4 04/13/2020 11:54:49 
// </copyright>
//-----------------------------------------------------------------------
using System;
using SqlSugar;

namespace WaterCloud.Domain.SystemSecurity
{
	/// <summary>
	/// ServerState Entity Model
	/// </summary>
	[SugarTable("sys_serverstate")]
    public class ServerStateEntity : IEntity<ServerStateEntity>
    {
		/// <summary>
		/// 主键Id
		/// </summary>
		[SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
		public  string  Id { get; set; }
		/// <summary>
		/// 网站站点
		/// </summary>					
		[SugarColumn(IsNullable = true, ColumnName = "WebSite",ColumnDataType = "nvarchar(200)", ColumnDescription = "网站站点")]
		public  string  WebSite { get; set; }
		/// <summary>
		/// ARM
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "ARM",ColumnDataType = "nvarchar(50)", ColumnDescription = "ARM")]
		public  string  ARM { get; set; }
		/// <summary>
		/// CPU
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "CPU",ColumnDataType = "nvarchar(50)", ColumnDescription = "CPU")]
		public  string  CPU { get; set; }
		/// <summary>
		/// IIS
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnName = "IIS", ColumnDataType = "nvarchar(50)", ColumnDescription = "IIS")]
		public  string  IIS { get; set; }
		/// <summary>
		/// 日期
		/// </summary>
		[SugarColumn(IsNullable = true, ColumnDescription = "日期")]
		public  DateTime  Date { get; set; }
		/// <summary>
		/// 次数
		/// </summary>
		[SugarColumn(IsNullable = true,ColumnDescription = "次数")]
		public int Cout { get; set; }
	}
}