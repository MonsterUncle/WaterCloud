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
        [SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string Id { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "日期")]
        public DateTime? Date { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "Account",ColumnDataType = "nvarchar(50)", ColumnDescription = "账户")]
        public string Account { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "NickName",ColumnDataType = "nvarchar(50)", ColumnDescription = "昵称")]
        public string NickName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "Type", ColumnDataType = "nvarchar(50)", ColumnDescription = "类型")]
        public string Type { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "IPAddress",ColumnDataType = "nvarchar(50)", ColumnDescription = "IP地址")]
        public string IPAddress { get; set; }
        /// <summary>
        /// IP地址名
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "IPAddressName", ColumnDataType = "nvarchar(50)", ColumnDescription = "IP地址名")]
        public string IPAddressName { get; set; }
        /// <summary>
        /// 模块Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "ModuleId", ColumnDataType = "nvarchar(50)", ColumnDescription = "模块Id")]
        public string ModuleId { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "ModuleName", ColumnDataType = "nvarchar(50)", ColumnDescription = "模块名称")]
        public string ModuleName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? Result { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "Description", ColumnDataType = "longtext", ColumnDescription = "备注")]
        public string Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "创建时间")]
        public DateTime? CreatorTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
        public string CreatorUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "KeyValue",ColumnDataType = "longtext")]
        public string KeyValue { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "CompanyId", ColumnDataType = "nvarchar(50)", ColumnDescription = "公司Id")]
        public string CompanyId { get; set; }
        public  LogEntity()
        {

        }
        //重载构造方法
        public LogEntity(string module,string moduleitem,string optiontype)
        {
            this.ModuleName = module+ moduleitem;
            this.Description = moduleitem+"操作,";
            this.Type = optiontype;
            this.Result = true;
        }
    }
}
