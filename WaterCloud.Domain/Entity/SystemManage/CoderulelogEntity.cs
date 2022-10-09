using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace WaterCloud.Domain.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-10-08 16:20
    /// 描 述：条码规则计数实体类
    /// </summary>
    [SugarTable("sys_coderulelog")]
    public class CoderulelogEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(ColumnName="F_Id", ColumnDescription = "主键",ColumnDataType = "nvarchar(50)",IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 规则Id
        /// </summary>
        [SugarColumn(ColumnName="F_RuleId", ColumnDescription = "规则Id",ColumnDataType = "nvarchar(50)")]
        public string F_RuleId { get; set; }
        /// <summary>
        /// key
        /// </summary>
        [SugarColumn(ColumnName="F_Key", ColumnDescription = "key",ColumnDataType = "nvarchar(100)")]
        public string F_Key { get; set; }
        /// <summary>
        /// value
        /// </summary>
        [SugarColumn(ColumnName="F_Value", ColumnDescription = "value",ColumnDataType = "nvarchar(100)", IsNullable = true)]
        public string F_Value { get; set; }
        /// <summary>
        /// 计数
        /// </summary>
        [SugarColumn(ColumnName="F_Score", ColumnDescription = "计数", IsNullable = true)]
        public int? F_Score { get; set; }
    }
}
