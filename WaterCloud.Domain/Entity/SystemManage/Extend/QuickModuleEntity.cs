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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.Annotations;
using WaterCloud.Domain;

namespace WaterCloud.Domain.SystemManage
{
    /// <summary>
    /// QuickModule Entity Model
    /// </summary>
    public class QuickModuleExtend
	{
        public string id { get; set; }
        public string title { get; set; }
        public string href { get; set; }
        public string icon { get; set; }

    }
}