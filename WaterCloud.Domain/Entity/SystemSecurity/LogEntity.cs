/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using SqlSugar;
using System;

namespace WaterCloud.Domain.SystemSecurity
{
    /// <summary>
    /// 日志实体
    /// </summary>
    [SugarTable("sys_log")]
    public class LogEntity : IEntity<LogEntity>, ICreationAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string F_Id { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "日期")]
        public DateTime? F_Date { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_Account",ColumnDataType = "nvarchar(50)", ColumnDescription = "账户")]
        public string F_Account { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_NickName",ColumnDataType = "nvarchar(50)", ColumnDescription = "昵称")]
        public string F_NickName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_Type", ColumnDataType = "nvarchar(50)", ColumnDescription = "类型")]
        public string F_Type { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_IPAddress",ColumnDataType = "nvarchar(50)", ColumnDescription = "IP地址")]
        public string F_IPAddress { get; set; }
        /// <summary>
        /// IP地址名
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_IPAddressName", ColumnDataType = "nvarchar(50)", ColumnDescription = "IP地址名")]
        public string F_IPAddressName { get; set; }
        /// <summary>
        /// 模块Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_ModuleId", ColumnDataType = "nvarchar(50)", ColumnDescription = "模块Id")]
        public string F_ModuleId { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_ModuleName", ColumnDataType = "nvarchar(50)", ColumnDescription = "模块名称")]
        public string F_ModuleName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? F_Result { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_Description", ColumnDataType = "longtext,nvarchar(4000)", ColumnDescription = "备注")]
        public string F_Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "创建时间")]
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_KeyValue",ColumnDataType = "longtext,nvarchar(4000)")]
        public string F_KeyValue { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_CompanyId", ColumnDataType = "nvarchar(50)", ColumnDescription = "公司Id")]
        public string F_CompanyId { get; set; }
        public  LogEntity()
        {

        }
        //重载构造方法
        public LogEntity(string module,string moduleitem,string optiontype)
        {
            this.F_ModuleName = module+ moduleitem;
            this.F_Description = moduleitem+"操作,";
            this.F_Type = optiontype;
            this.F_Result = true;
        }
    }
}
