using System;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace WaterCloud.Domain.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-12 13:50
    /// 描 述：系统设置实体类
    /// </summary>
    [SugarTable("sys_systemset")]
    public class SystemSetEntity : IEntity<SystemSetEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string Id { get; set; }
        /// <summary>
        /// Logo图标
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "Logo图标不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "Logo", ColumnDataType = "nvarchar(50)", ColumnDescription = "Logo图标")]
        public string Logo { get; set; }
        /// <summary>
        /// Logo编号
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "Logo编号不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "LogoCode",ColumnDataType = "nvarchar(50)", ColumnDescription = "Logo编号")]
        public string LogoCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "项目名称不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "ProjectName", ColumnDataType = "nvarchar(50)", ColumnDescription = "项目名称")]
        public string ProjectName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "公司名称不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "CompanyName", ColumnDataType = "nvarchar(50)", ColumnDescription = "公司名称")]
        public string CompanyName { get; set; }
        /// <summary>
        /// 系统账号
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "系统账户不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "AdminAccount", ColumnDataType = "nvarchar(50)", ColumnDescription = "系统账号")]
        public string AdminAccount { get; set; }
        /// <summary>
        /// 系统密码
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "系统密码不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "AdminPassword",ColumnDataType = "nvarchar(50)", ColumnDescription = "系统密码")]
        public string AdminPassword { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true,ColumnDescription = "删除标记")]
        public bool? DeleteMark { get; set; }
        /// <summary>
        /// 有效标记
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "有效标记")]
        public bool? EnabledMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "Description", ColumnDataType = "longtext", ColumnDescription = "备注")]
        public string Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? CreatorTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
        public string CreatorUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "修改时间")]
        public DateTime? LastModifyTime { get; set; }
        /// <summary>
        /// 修改人Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "LastModifyUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "修改人Id")]
        public string LastModifyUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除时间")]
        public DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 删除人Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "DeleteUserId",ColumnDataType = "longtext", ColumnDescription = "删除人Id")]
        public string DeleteUserId { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "联系电话不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "MobilePhone", ColumnDataType = "nvarchar(20)", ColumnDescription = "联系电话")]
        public string MobilePhone { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "联系人不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "PrincipalMan", ColumnDataType = "nvarchar(50)", ColumnDescription = "联系人")]
        public string PrincipalMan { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "到期时间不能为空")]
        [SugarColumn(IsNullable = true, ColumnDescription = "到期时间")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 数据库连接串
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "DbString", ColumnDataType = "longtext", ColumnDescription = "数据库连接串")]
        public string DbString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "DBProvider",ColumnDataType = "nvarchar(50)")]
        public string DBProvider { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        [Required(ErrorMessage = "域名不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "HostUrl",ColumnDataType = "longtext", ColumnDescription = "域名")]
        public string HostUrl { get; set; }
        /// <summary>
        /// 数据库序号
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "DbNumber",ColumnDataType = "nvarchar(50)", ColumnDescription = "数据库序号", UniqueGroupNameList = new string[] { "sys_systemset" })]
        public string DbNumber { get; set; }
    }
}
