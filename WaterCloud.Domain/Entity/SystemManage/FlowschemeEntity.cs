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
        [SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 流程编号
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "流程编号不能为空")]
        public string F_SchemeCode { get; set; }
        /// <summary>
        /// 流程名称
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "流程名称不能为空")]
        public string F_SchemeName { get; set; }
        /// <summary>
        /// 流程分类
        /// </summary>
        /// <returns></returns>
        public string F_SchemeType { get; set; }
        /// <summary>
        /// 流程内容版本
        /// </summary>
        /// <returns></returns>
        public string F_SchemeVersion { get; set; }
        /// <summary>
        /// 流程模板使用者
        /// </summary>
        /// <returns></returns>
        public string F_SchemeCanUser { get; set; }
        /// <summary>
        /// 流程内容
        /// </summary>
        /// <returns></returns>
        public string F_SchemeContent { get; set; }
        /// <summary>
        /// 表单ID
        /// </summary>
        /// <returns></returns>
        public string F_FrmId { get; set; }
        /// <summary>
        /// 表单类型
        /// </summary>
        /// <returns></returns>
        public int? F_FrmType { get; set; }
        /// <summary>
        /// 模板权限类型：0完全公开,1指定部门/人员
        /// </summary>
        /// <returns></returns>
        public int? F_AuthorizeType { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "排序不能为空")]
        [Range(0, 99999999, ErrorMessage = "排序大小必须介于1~99999999之间")]
        public int? F_SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        public bool? F_DeleteMark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        /// <returns></returns>
        public bool? F_EnabledMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string F_Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public string F_CreatorUserName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime? F_LastModifyTime { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>
        /// <returns></returns>
        public string F_LastModifyUserId { get; set; }
        /// <summary>        [SugarColumn(IsIgnore=true)]
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        public string F_LastModifyUserName { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        /// <returns></returns>
        public string F_OrganizeId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        /// <returns></returns>
        public DateTime? F_DeleteTime { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        /// <returns></returns>
        public string F_DeleteUserId { get; set; }
        [SugarColumn(IsIgnore=true)]
        public string F_ParentId { get; set; }
    }
}
