using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace WaterCloud.Domain.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-01 09:44
    /// 描 述：数据权限实体类
    /// </summary>
    [SugarTable("sys_dataprivilegerule")]
    public class DataPrivilegeRuleEntity : IEntity<DataPrivilegeRuleEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
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
        [SugarColumn(IsNullable = true, ColumnName = "ModuleId",ColumnDataType = "nvarchar(50)", ColumnDescription = "模块Id", UniqueGroupNameList = new string[] { "sys_dataprivilegerule" })]
        public string ModuleId { get; set; }
        /// <summary>
        /// 模块编号
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "ModuleCode",ColumnDataType = "nvarchar(50)", ColumnDescription = "模块编号")]
        public string ModuleCode { get; set; }
        /// <summary>
        /// 权限规则
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "PrivilegeRules",ColumnDataType = "longtext", ColumnDescription = "权限规则")]
        public string PrivilegeRules { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "排序不能为空")]
        [Range(0, 99999999, ErrorMessage = "排序大小必须介于1~99999999之间")]
        [SugarColumn(IsNullable = true, ColumnDescription = "排序")]
        public int? SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除标记")]
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
        [SugarColumn(IsNullable = true, ColumnName = "Description",ColumnDataType = "longtext", ColumnDescription = "备注")]
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
        [SugarColumn(IsNullable = true,ColumnDescription = "修改时间")]
        public DateTime? LastModifyTime { get; set; }
        /// <summary>
        /// 修改人Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "LastModifyUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "修改人Id")]
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
    }
}
