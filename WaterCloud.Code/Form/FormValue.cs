using System;
using System.Collections.Generic;
using System.Text;

namespace WaterCloud.Code
{
    /// <summary>
    /// 表单设计类
    /// </summary>
    public class FormValue
    {
        public string id { get; set; }
        public string label { get; set; }
        public int index { get; set; }
        public string tag { get; set; }
        public int span { get; set; }
        public List<FormEx> columns { get; set; }
        public string name { get; set; }
    }
    public class FormEx
    {
        public int span { get; set; }
        public List<FormValue> list { get; set; }
    }
}
