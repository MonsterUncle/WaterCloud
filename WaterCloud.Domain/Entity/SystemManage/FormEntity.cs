using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;
using Serenity.Data.Mapping;

namespace WaterCloud.Domain.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-07-08 14:33
    /// 描 述：表单设计实体类
    /// </summary>
    [SugarTable("sys_form")]
    public class FormEntity : IEntity<FormEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 表单模板Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string Id { get; set; }
        /// <summary>
        /// 表单名称
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "表单名称不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "Name",ColumnDataType = "nvarchar(200)", ColumnDescription = "表单名称", UniqueGroupNameList = new string[] { "sys_form" })]
        public string Name { get; set; }
        /// <summary>
        /// 表单类型，0：默认动态表单；1：Web自定义表单
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "表单类型不能为空")]
        [SugarColumn(IsNullable = true,ColumnDescription = "表单类型，0：默认动态表单；1：Web自定义表单")]
        public int? FrmType { get; set; }
        /// <summary>
        /// 系统页面标识，当表单类型为用Web自定义的表单时，需要标识加载哪个页面
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "WebId",ColumnDataType = "nvarchar(50)", ColumnDescription = "系统页面标识")]
        public string WebId { get; set; }
        /// <summary>
        /// 字段个数
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "字段个数")]
        public int? Fields { get; set; }
        /// <summary>
        /// 表单中的字段
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "ContentData", ColumnDataType = "longtext", ColumnDescription = "表单中的字段")]
        public string ContentData { get; set; }
        /// <summary>
        /// 表单参数（冗余）
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "ContentParse", ColumnDataType = "longtext", ColumnDescription = "表单参数")]
        public string ContentParse { get; set; }
        /// <summary>
        /// 表单参数Json
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "Content", ColumnDataType = "nvarchar(50)", ColumnDescription = "表单参数Json")]
        public string Content { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "排序不能为空")]
        [Range(0, 99999999, ErrorMessage = "排序大小必须介于1~99999999之间")]
        [SugarColumn(IsNullable = true, ColumnDescription = "排序码")]
        public int? SortCode { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否启用")]
        public bool? EnabledMark { get; set; }
        /// <summary>
        /// 逻辑删除标志
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "逻辑删除标志")]
        public bool? DeleteMark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? CreatorTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人")]
        public string CreatorUserId { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "最后修改时间")]
        public DateTime? LastModifyTime { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "LastModifyUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "最后修改人")]
        public string LastModifyUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除时间")]
        public DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "DeleteUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "删除人")]
        public string DeleteUserId { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "Description",ColumnDataType = "longtext", ColumnDescription = "内容")]
        public string Description { get; set; }
        /// <summary>
        /// 所属组织
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "OrganizeId", ColumnDataType = "nvarchar(50)", ColumnDescription = "所属组织")]
        public string OrganizeId { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "DbName", ColumnDataType = "nvarchar(50)", ColumnDescription = "数据库名称")]
        public string DbName { get; set; }
        [SugarColumn(IsIgnore=true)]
        public string OrganizeName { get; set; }
    }
}
