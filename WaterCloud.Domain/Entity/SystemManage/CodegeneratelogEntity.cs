using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace WaterCloud.Domain.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-10-06 14:18
    /// 描 述：条码生成记录实体类
    /// </summary>
    [SugarTable("sys_codegeneratelog")]
    public class CodegeneratelogEntity : IEntity<CodegeneratelogEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(ColumnName="F_Id", ColumnDescription = "主键",IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        [SugarColumn(ColumnName="F_Code", ColumnDescription = "条码",ColumnDataType = "nvarchar(50)")]
        public string F_Code { get; set; }
        /// <summary>
        /// 规则id
        /// </summary>
        [SugarColumn(ColumnName="F_RuleId", ColumnDescription = "规则id",ColumnDataType = "nvarchar(50)")]
        public string F_RuleId { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        [SugarColumn(ColumnName="F_RuleName", ColumnDescription = "规则名称",ColumnDataType = "nvarchar(50)", IsNullable = true)]
        public string F_RuleName { get; set; }
        /// <summary>
        /// 打印Json
        /// </summary>
        [SugarColumn(ColumnName="F_PrintJson", ColumnDescription = "打印Json",ColumnDataType = "longtext", IsNullable = true)]
        public string F_PrintJson { get; set; }
        /// <summary>
        /// 打印次数
        /// </summary>
        [SugarColumn(ColumnName="F_PrintCount", ColumnDescription = "打印次数", IsNullable = true)]
        public int? F_PrintCount { get; set; }
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
    }
}
