using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;
using Serenity.Data.Mapping;

namespace WaterCloud.Domain.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-07-10 08:49
    /// 描 述：流程设计实体类
    /// </summary>
    [SugarTable("sys_flowscheme")]
    public class FlowschemeEntity : IEntity<FlowschemeEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string Id { get; set; }
        /// <summary>
        /// 流程编号
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "流程编号不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "SchemeCode",ColumnDataType = "nvarchar(50)", ColumnDescription = "流程编号", UniqueGroupNameList = new string[] { "sys_flowscheme" })]
        public string SchemeCode { get; set; }
        /// <summary>
        /// 流程名称
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "流程名称不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "SchemeName",ColumnDataType = "nvarchar(200)", ColumnDescription = "流程名称")]
        public string SchemeName { get; set; }
        /// <summary>
        /// 流程分类
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "SchemeType", ColumnDataType = "nvarchar(50)", ColumnDescription = "流程分类")]
        public string SchemeType { get; set; }
        /// <summary>
        /// 流程内容版本
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "SchemeVersion", ColumnDataType = "nvarchar(50)", ColumnDescription = "流程内容版本")]
        public string SchemeVersion { get; set; }
        /// <summary>
        /// 流程模板使用者
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "SchemeCanUser",ColumnDataType = "longtext", ColumnDescription = "流程模板使用者")]
        public string SchemeCanUser { get; set; }
        /// <summary>
        /// 流程内容
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "SchemeContent", ColumnDataType = "longtext", ColumnDescription = "流程内容")]
        public string SchemeContent { get; set; }
        /// <summary>
        /// 表单ID
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "FrmId",ColumnDataType = "nvarchar(50)", ColumnDescription = "表单ID")]
        public string FrmId { get; set; }
        /// <summary>
        /// 表单类型
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = false, ColumnDescription = "表单类型")]
        public int FrmType { get; set; }
        /// <summary>
        /// 模板权限类型：0完全公开,1指定部门/人员
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = false, ColumnDescription = "模板权限类型：0完全公开,1指定部门/人员")]
        public int AuthorizeType { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "排序不能为空")]
        [Range(0, 99999999, ErrorMessage = "排序大小必须介于1~99999999之间")]
        [SugarColumn(IsNullable = true,ColumnDescription = "排序码")]
        public int? SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除标记")]
        public bool? DeleteMark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "有效")]
        public bool? EnabledMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "Description",ColumnDataType = "nvarchar(200)", ColumnDescription = "备注")]
        public string Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true,ColumnDescription = "创建时间")]
        public DateTime? CreatorTime { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "CreatorUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "创建用户主键")]
        public string CreatorUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "CreatorUserName", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建用户")]
        public string CreatorUserName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "修改时间")]
        public DateTime? LastModifyTime { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "LastModifyUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "修改用户主键")]
        public string LastModifyUserId { get; set; }
        /// <summary>        
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "LastModifyUserName", ColumnDataType = "nvarchar(50)", ColumnDescription = "修改用户")]
        public string LastModifyUserName { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "OrganizeId", ColumnDataType = "nvarchar(50)", ColumnDescription = "所属部门")]
        public string OrganizeId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true,ColumnDescription = "删除时间")]
        public DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "DeleteUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "删除人")]
        public string DeleteUserId { get; set; }
        [SugarColumn(IsIgnore=true)]
        public string ParentId { get; set; }
    }
}
