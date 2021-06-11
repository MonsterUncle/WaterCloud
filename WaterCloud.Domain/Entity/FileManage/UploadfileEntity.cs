using System;
using Serenity.Data.Mapping;
using SqlSugar;

namespace WaterCloud.Domain.FileManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-07-22 12:04
    /// 描 述：文件管理实体类
    /// </summary>
    [SugarTable("oms_uploadfile")]
    public class UploadfileEntity : IEntity<UploadfileEntity>,ICreationAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string F_Id { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_FilePath",ColumnDataType = "nvarchar(50)", ColumnDescription = "文件路径")]
        public string F_FilePath { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = false, ColumnName = "F_FileName",ColumnDataType = "nvarchar(200)", ColumnDescription = "文件名称", UniqueGroupNameList = new string[] { "oms_uploadfile" })]
        public string F_FileName { get; set; }
        /// <summary>
        /// 文件类型(0文件、1图片)
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "文件类型")]
        public int? F_FileType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "文件大小")]
        public int? F_FileSize { get; set; }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_FileExtension", ColumnDataType = "nvarchar(20)", ColumnDescription = "文件扩展名")]
        public string F_FileExtension { get; set; }
        /// <summary>
        /// 文件所属
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_FileBy",ColumnDataType = "nvarchar(50)", ColumnDescription = "文件所属")]
        public string F_FileBy { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_Description", ColumnDataType = "nvarchar(200)", ColumnDescription = "备注")]
        public string F_Description { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_OrganizeId", ColumnDataType = "nvarchar(50)", ColumnDescription = "所属部门")]
        public string F_OrganizeId { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "有效")]
        public bool? F_EnabledMark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "创建用户主键")]
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserName", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建用户")]
        public string F_CreatorUserName { get; set; }
        [SugarColumn(IsIgnore=true)]
        public string F_OrganizeName { get; set; }
    }
}
