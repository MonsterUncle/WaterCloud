﻿/*******************************************************************************
 * Copyright © 2016 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
namespace WaterCloud.Code
{
    public class TreeViewModel
    {
        public string parentId { get; set; }
        public string id { get; set; }
        public string text { get; set; }
        public string value { get; set; }
        public int? checkstate { get; set; }
        public bool showcheck { get; set; }
        public bool complete { get; set; }
        public bool isexpand { get; set; }
        public bool hasChildren { get; set; }
        public string img { get; set; }
        public string title { get; set; }
    }
}
