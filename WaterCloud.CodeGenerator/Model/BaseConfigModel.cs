using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.CodeGenerator
{
    public class BaseConfigModel
    {
        /// <summary>
        /// 数据库表名sys_menu
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表名首字母大写Sys_Menu
        /// </summary>
        public string TableNameUpper { get; set; }
        public FileConfigModel FileConfig { get; set; }
        public OutputConfigModel OutputConfig { get; set; }
        public PageIndexModel PageIndex { get; set; }
        public PageFormModel PageForm { get; set; }
    }

    public class FileConfigModel
    {
        public string ClassPrefix { get; set; }
        public string ClassDescription { get; set; }
        public string CreateUserName { get; set; }
        public string CreateDate { get; set; }
        public string EntityName { get; set; }
        public string ServiceName { get; set; }
        public string ControllerName { get; set; }
        public string PageIndexName { get; set; }
        public string PageFormName { get; set; }
        public string PageDetailsName { get; set; }
    }
    public class OutputConfigModel
    {
        public string OutputModule { get; set; }
        public string OutputEntity { get; set; }
        public string OutputService { get; set; }
        public string OutputWeb { get; set; }
    }
    public class PageIndexModel
    {
        /// <summary>
        /// 是否菜单显示
        /// </summary>
        public int IsMunu { get; set; }
        /// <summary>
        /// 是否需要搜索框
        /// </summary>
        public int IsSearch { get; set; }
        /// <summary>
        /// 是否树形表格
        /// </summary>
        public int IsTree { get; set; }
        /// <summary>
        /// 是否字段控制
        /// </summary>
        public int IsFields { get; set; }
        /// <summary>
        /// 是否公共
        /// </summary>
        public int IsPublic { get; internal set; }

        /// <summary>
        /// 工具栏按钮（新增 修改 删除 查看）
        /// </summary>
        public List<string> ButtonList { get; set; }

        /// <summary>
        /// 是否有分页
        /// </summary>
        public int IsPagination { get; set; }

        public Dictionary<string, string> ColumnList { get; set; }

    }
    public class PageFormModel
    {
        /// <summary>
        /// 1表示显示成1列，2表示显示成2列
        /// </summary>
        public int ShowMode { get; set; }
        public Dictionary<string,string> FieldList { get; set; }
    }
}
