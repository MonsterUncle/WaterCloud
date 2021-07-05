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
        [SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string F_Id { get; set; }
        /// <summary>
        /// Logo图标
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "Logo图标不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_Logo", ColumnDataType = "nvarchar(50)", ColumnDescription = "Logo图标")]
        public string F_Logo { get; set; }
        /// <summary>
        /// Logo编号
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "Logo编号不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_LogoCode",ColumnDataType = "nvarchar(50)", ColumnDescription = "Logo编号")]
        public string F_LogoCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "项目名称不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_ProjectName", ColumnDataType = "nvarchar(50)", ColumnDescription = "项目名称")]
        public string F_ProjectName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "公司名称不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_CompanyName", ColumnDataType = "nvarchar(50)", ColumnDescription = "公司名称")]
        public string F_CompanyName { get; set; }
        /// <summary>
        /// 系统账号
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "系统账户不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_AdminAccount", ColumnDataType = "nvarchar(50)", ColumnDescription = "系统账号")]
        public string F_AdminAccount { get; set; }
        /// <summary>
        /// 系统密码
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "系统密码不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_AdminPassword",ColumnDataType = "nvarchar(50)", ColumnDescription = "系统密码")]
        public string F_AdminPassword { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true,ColumnDescription = "删除标记")]
        public bool? F_DeleteMark { get; set; }
        /// <summary>
        /// 有效标记
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "有效标记")]
        public bool? F_EnabledMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_Description", ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "备注")]
        public string F_Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "修改时间")]
        public DateTime? F_LastModifyTime { get; set; }
        /// <summary>
        /// 修改人Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_LastModifyUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "修改人Id")]
        public string F_LastModifyUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除时间")]
        public DateTime? F_DeleteTime { get; set; }
        /// <summary>
        /// 删除人Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_DeleteUserId",ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "删除人Id")]
        public string F_DeleteUserId { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "联系电话不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_MobilePhone", ColumnDataType = "nvarchar(20)", ColumnDescription = "联系电话")]
        public string F_MobilePhone { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "联系人不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_PrincipalMan", ColumnDataType = "nvarchar(50)", ColumnDescription = "联系人")]
        public string F_PrincipalMan { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "到期时间不能为空")]
        [SugarColumn(IsNullable = true, ColumnDescription = "到期时间")]
        public DateTime? F_EndTime { get; set; }
        /// <summary>
        /// 数据库连接串
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnName = "F_DbString", ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "数据库连接串")]
        public string F_DbString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnName = "F_DBProvider",ColumnDataType = "nvarchar(50)")]
        public string F_DBProvider { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        [Required(ErrorMessage = "域名不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_HostUrl",ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "域名")]
        public string F_HostUrl { get; set; }
        /// <summary>
        /// 数据库序号
        /// </summary>
        [SugarColumn(IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnName = "F_DbNumber",ColumnDataType = "nvarchar(50)", ColumnDescription = "数据库序号", UniqueGroupNameList = new string[] { "sys_systemset" })]
        public string F_DbNumber { get; set; }
    }
}
