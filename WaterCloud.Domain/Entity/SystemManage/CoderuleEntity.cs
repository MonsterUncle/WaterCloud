using System;
using SqlSugar;

namespace WaterCloud.Domain.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-10-06 11:25
    /// 描 述：条码规则实体类
    /// </summary>
    [SugarTable("sys_coderule")]
    public class CoderuleEntity : IEntity<CoderuleEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(ColumnName="F_Id", ColumnDescription = "主键",IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        [SugarColumn(ColumnName="F_RuleName", ColumnDescription = "规则名称",ColumnDataType = "nvarchar(50)", IsNullable = true)]
        public string F_RuleName { get; set; }
        /// <summary>
        /// 规则内容
        /// </summary>
        [SugarColumn(ColumnName="F_RuleJson", ColumnDescription = "规则内容",ColumnDataType = "longtext")]
        public string F_RuleJson { get; set; }
        /// <summary>
        /// 重设机制
        /// </summary>
        [SugarColumn(ColumnName="F_Reset", ColumnDescription = "重设机制",ColumnDataType = "nvarchar(50)", IsNullable = true)]
        public string F_Reset { get; set; }
		/// <summary>
		/// 打印模板Id
		/// </summary>
		[SugarColumn(ColumnName = "F_TemplateId", ColumnDescription = "模板Id", ColumnDataType = "nvarchar(50)", IsNullable = true)]
		public string F_TemplateId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_DeleteMark", IsNullable = true)]
        public bool? F_DeleteMark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_EnabledMark", IsNullable = true)]
        public bool? F_EnabledMark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_Description",ColumnDataType = "longtext", IsNullable = true)]
        public string F_Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_CreatorTime", IsNullable = true)]
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
        [SugarColumn(ColumnName="F_LastModifyTime", IsNullable = true)]
        public DateTime? F_LastModifyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_LastModifyUserId", ColumnDescription = "",ColumnDataType = "nvarchar(50)", IsNullable = true)]
        public string F_LastModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_DeleteTime", IsNullable = true)]
        public DateTime? F_DeleteTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_DeleteUserId", ColumnDescription = "",ColumnDataType = "nvarchar(50)", IsNullable = true)]
        public string F_DeleteUserId { get; set; }
		/// <summary>
		/// 打印模板
		/// </summary>
		[SugarColumn(IsIgnore = true)]
		public string F_TemplateName { get; set; }
		/// <summary>
		/// 打印方式
		/// </summary>
		[SugarColumn(IsIgnore = true)]
		public int? F_PrintType { get; set; }
		/// <summary>
		/// 是否批量
		/// </summary>
		[SugarColumn(IsIgnore = true)]
		public bool? F_Batch { get; set; }
	}
}
