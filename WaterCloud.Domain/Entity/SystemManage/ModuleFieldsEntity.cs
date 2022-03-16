using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace WaterCloud.Domain.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-05-21 14:38
    /// 描 述：字段管理实体类
    /// </summary>
    [SugarTable("sys_modulefields")]
    public class ModuleFieldsEntity : IEntity<ModuleFieldsEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string Id { get; set; }
        /// <summary>
        /// 模块Id
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "模块不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "ModuleId",ColumnDataType = "nvarchar(50)", ColumnDescription = "模块Id", UniqueGroupNameList = new string[] { "sys_modulefields" })]
        public string ModuleId { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "编号不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "EnCode",ColumnDataType = "nvarchar(50)", ColumnDescription = "编号", UniqueGroupNameList = new string[] { "sys_modulefields" })]
        public string EnCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "名称不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "FullName",ColumnDataType = "nvarchar(50)", ColumnDescription = "名称")]
        public string FullName { get; set; }
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
        [SugarColumn(IsNullable = true,ColumnDescription = "有效标记")]
        public bool? EnabledMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "Description",ColumnDataType = "longtext", ColumnDescription = "备注")]
        public string Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true,ColumnDescription = "创建时间")]
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
        [SugarColumn(IsNullable = true, ColumnName = "DeleteUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "删除人Id")]
        public string DeleteUserId { get; set; }
        /// <summary>
        /// 是否公共
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否公共")]
        public bool? IsPublic { get; set; }
    }
}
