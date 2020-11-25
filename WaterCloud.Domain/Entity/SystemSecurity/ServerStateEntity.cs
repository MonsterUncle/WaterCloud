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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.Annotations;
using WaterCloud.Entity;

namespace WaterCloud.Entity.SystemSecurity
{
    /// <summary>
    /// ServerState Entity Model
    /// </summary>
	[TableAttribute("Sys_ServerState")]
    public class ServerStateEntity : IEntity<ServerStateEntity>
    {
						[ColumnAttribute("F_Id", IsPrimaryKey = true)]
			public  String  F_Id { get; set; }
					
			public  String  F_WebSite { get; set; }
					
			public  String  F_ARM { get; set; }
					
			public  String  F_CPU { get; set; }
					
			public  String  F_IIS { get; set; }
					
			public  DateTime  F_Date { get; set; }
		public int F_Cout { get; set; }
	}
}