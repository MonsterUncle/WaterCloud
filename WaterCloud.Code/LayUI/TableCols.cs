using System;
using System.Collections.Generic;
using System.Text;

namespace WaterCloud.Code
{
    /// <summary>
    /// LayUI Table 列
    /// </summary>
    public class TableCols
    {
        /// <summary>
        /// 类型（normal（常规列）、checkbox（复选框）、radio（单选框）、numbers（序号）、space（空））
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        public string field { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int? width { get; set; }
        /// <summary>
        /// 最小宽度
        /// </summary>
        public int? minWidth { get; set; }
        /// <summary>
        /// 是否全选
        /// </summary>
        public bool? LAY_CHECKED { get; set; }
        /// <summary>
        /// 固定列
        /// </summary>
        public string Fixed { get; set; }
        /// <summary>
        /// 隐藏
        /// </summary>
        public string hide { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public bool? sort { get; set; }
        /// <summary>
        /// 是否禁用拖到列
        /// </summary>
        public bool? unresize { get; set; }
        /// <summary>
        /// 样式
        /// </summary>
        public string style { get; set; }
        /// <summary>
        /// 对齐方式
        /// </summary>
        public string align { get; set; }
        /// <summary>
        /// 所占列数
        /// </summary>
        public int? colspan { get; set; }
        /// <summary>
        /// 所占行数
        /// </summary>
        public int? rowspan { get; set; }
        /// <summary>
        /// 绑定工具栏模板
        /// </summary>
        public string toolbar { get; set; }
       
    }
}
