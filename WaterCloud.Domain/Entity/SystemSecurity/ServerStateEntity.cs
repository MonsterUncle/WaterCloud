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
						[SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true)]
			public  String  F_Id { get; set; }
					
			public  String  F_WebSite { get; set; }
					
			public  String  F_ARM { get; set; }
					
			public  String  F_CPU { get; set; }
					
			public  String  F_IIS { get; set; }
					
			public  DateTime  F_Date { get; set; }
		public int F_Cout { get; set; }
	}
}