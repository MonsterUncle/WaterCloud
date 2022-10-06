using System;
using System.ComponentModel.DataAnnotations;
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
        [SugarColumn(ColumnName="F_Id", ColumnDescription = "主键",ColumnDataType = "varchar(50)",IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        [SugarColumn(ColumnName="F_RuleName", ColumnDescription = "规则名称",ColumnDataType = "varchar(50)", IsNullable = true)]
        public string F_RuleName { get; set; }
        /// <summary>
        /// 规则内容
        /// </summary>
        [SugarColumn(ColumnName="F_RuleJson", ColumnDescription = "规则内容",ColumnDataType = "text")]
        public string F_RuleJson { get; set; }
        /// <summary>
        /// 重设机制
        /// </summary>
        [SugarColumn(ColumnName="F_Reset", ColumnDescription = "重设机制",ColumnDataType = "varchar(50)", IsNullable = true)]
        public string F_Reset { get; set; }
        /// <summary>
        /// 打印模板
        /// </summary>
        [SugarColumn(ColumnName="F_PrintTemplate", ColumnDescription = "打印模板",ColumnDataType = "varchar(255)", IsNullable = true)]
        public string F_PrintTemplate { get; set; }
        /// <summary>
        /// 打印方式
        /// </summary>
        [SugarColumn(ColumnName="F_PrintType", ColumnDescription = "打印方式",ColumnDataType = "int", IsNullable = true)]
        public int? F_PrintType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_DeleteMark", ColumnDescription = "",ColumnDataType = "tinyint(1)", IsNullable = true)]
        public bool? F_DeleteMark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_EnabledMark", ColumnDescription = "",ColumnDataType = "tinyint(1)", IsNullable = true)]
        public bool? F_EnabledMark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_Description", ColumnDescription = "",ColumnDataType = "text", IsNullable = true)]
        public string F_Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_CreatorTime", ColumnDescription = "",ColumnDataType = "datetime", IsNullable = true)]
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_CreatorUserId", ColumnDescription = "",ColumnDataType = "varchar(50)", IsNullable = true)]
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_CreatorUserName", ColumnDescription = "",ColumnDataType = "varchar(50)", IsNullable = true)]
        public string F_CreatorUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_LastModifyTime", ColumnDescription = "",ColumnDataType = "datetime", IsNullable = true)]
        public DateTime? F_LastModifyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_LastModifyUserId", ColumnDescription = "",ColumnDataType = "varchar(50)", IsNullable = true)]
        public string F_LastModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_DeleteTime", ColumnDescription = "",ColumnDataType = "datetime", IsNullable = true)]
        public DateTime? F_DeleteTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="F_DeleteUserId", ColumnDescription = "",ColumnDataType = "varchar(50)", IsNullable = true)]
        public string F_DeleteUserId { get; set; }
    }
}
