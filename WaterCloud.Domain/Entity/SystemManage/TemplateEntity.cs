using System;
using SqlSugar;

namespace WaterCloud.Domain.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-10-06 14:11
    /// 描 述：打印模板实体类
    /// </summary>
    [SugarTable("sys_template")]
    public class TemplateEntity : IEntity<TemplateEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(ColumnName="F_Id", ColumnDescription = "主键",IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        [SugarColumn(ColumnName="F_TemplateName", ColumnDescription = "模板名称",ColumnDataType = "nvarchar(50)", IsNullable = true)]
        public string F_TemplateName { get; set; }
        /// <summary>
        /// 模板文件
        /// </summary>
        [SugarColumn(ColumnName="F_TemplateFile", ColumnDescription = "模板文件",ColumnDataType = "nvarchar(100)")]
        public string F_TemplateFile { get; set; }
        /// <summary>
        /// 模板执行库
        /// </summary>
        [SugarColumn(ColumnName="F_TemplateDBProvider", ColumnDescription = "模板执行库",ColumnDataType = "nvarchar(50)", IsNullable = true)]
        public string F_TemplateDBProvider { get; set; }
        /// <summary>
        /// 模板执行sql
        /// </summary>
        [SugarColumn(ColumnName="F_TemplateSql", ColumnDescription = "模板执行sql",ColumnDataType = "longtext", IsNullable = true)]
        public string F_TemplateSql { get; set; }
        /// <summary>
        /// 模板执行参数
        /// </summary>
        [SugarColumn(ColumnName="F_TemplateSqlParm", ColumnDescription = "模板执行参数",ColumnDataType = "longtext", IsNullable = true)]
        public string F_TemplateSqlParm { get; set; }
		/// <summary>
		/// 打印方式(1.Fastreport,2.水晶报表,3.html,4.pdf,5.图片,6.bartender)
		/// </summary>
		[SugarColumn(ColumnName="F_PrintType", ColumnDescription = "打印方式",ColumnDataType = "int", IsNullable = true)]
        public int? F_PrintType { get; set; }
        /// <summary>
        /// 是否批量
        /// </summary>
		[SugarColumn(ColumnName = "F_Batch", ColumnDescription = "是否批量", IsNullable = true)]
		public bool? F_Batch { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SugarColumn(ColumnName="F_DeleteMark", ColumnDescription = "", IsNullable = true)]
        public bool? F_DeleteMark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_EnabledMark", ColumnDescription = "", IsNullable = true)]
        public bool? F_EnabledMark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_Description", ColumnDescription = "",ColumnDataType = "longtext", IsNullable = true)]
        public string F_Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_CreatorTime", ColumnDescription = "", IsNullable = true)]
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_CreatorUserId", ColumnDescription = "",ColumnDataType = "nvarchar(50)", IsNullable = true)]
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_CreatorUserName", ColumnDescription = "",ColumnDataType = "nvarchar(50)", IsNullable = true)]
        public string F_CreatorUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_LastModifyTime", ColumnDescription = "", IsNullable = true)]
        public DateTime? F_LastModifyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_LastModifyUserId", ColumnDescription = "",ColumnDataType = "nvarchar(50)", IsNullable = true)]
        public string F_LastModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_DeleteTime", ColumnDescription = "", IsNullable = true)]
        public DateTime? F_DeleteTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_DeleteUserId", ColumnDescription = "",ColumnDataType = "nvarchar(50)", IsNullable = true)]
        public string F_DeleteUserId { get; set; }
    }
}
