using System;
using System.ComponentModel.DataAnnotations;
using Serenity.Data.Mapping;
using SqlSugar;

namespace WaterCloud.Domain.FlowManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-07-14 09:18
    /// 描 述：我的流程实体类
    /// </summary>
    [SugarTable("oms_flowinstance")]
    public class FlowinstanceEntity : IEntity<FlowinstanceEntity>, ICreationAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string F_Id { get; set; }
        /// <summary>
        /// 流程实例模板Id
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = false, ColumnName = "F_InstanceSchemeId",ColumnDataType = "nvarchar(50)", ColumnDescription = "流程实例模板Id")]
        public string F_InstanceSchemeId { get; set; }
        /// <summary>
        /// 实例编号
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "编号不能为空")]
        [SugarColumn(IsNullable = false, ColumnName = "F_Code", ColumnDataType = "nvarchar(200)", ColumnDescription = "实例编号", UniqueGroupNameList = new string[] { "oms_flowinstance" })]
        public string F_Code { get; set; }
        /// <summary>
        /// 自定义名称
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_CustomName",ColumnDataType = "nvarchar(200)", ColumnDescription = "自定义名称")]
        public string F_CustomName { get; set; }
        /// <summary>
        /// 当前节点ID
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_ActivityId",ColumnDataType = "nvarchar(50)", ColumnDescription = "当前节点ID")]
        public string F_ActivityId { get; set; }
        /// <summary>
        /// 当前节点类型（0会签节点）
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnDescription = "当前节点类型")]
        public int? F_ActivityType { get; set; }
        /// <summary>
        /// 当前节点名称
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_ActivityName", ColumnDataType = "nvarchar(200)", ColumnDescription = "当前节点名称")]
        public string F_ActivityName { get; set; }
        /// <summary>
        /// 前一个ID
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_PreviousId",ColumnDataType = "nvarchar(50)", ColumnDescription = "前一个ID")]
        public string F_PreviousId { get; set; }
        /// <summary>
        /// 流程模板内容
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_SchemeContent", ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "流程模板内容")]
        public string F_SchemeContent { get; set; }
        /// <summary>
        /// 流程模板ID
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "模板不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_SchemeId", ColumnDataType = "nvarchar(50)", ColumnDescription = "流程模板ID")]
        public string F_SchemeId { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_DbName",ColumnDataType = "nvarchar(50)", ColumnDescription = "数据库名称")]
        public string F_DbName { get; set; }
        /// <summary>
        /// 表单数据
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_FrmData", ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "表单数据")]
        public string F_FrmData { get; set; }
        /// <summary>
        /// 表单类型
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = false, ColumnDescription = "表单类型")] 
        public int? F_FrmType { get; set; }
        /// <summary>
        /// 表单中的字段
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_FrmContentData", ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "表单中的字段")]
        public string F_FrmContentData { get; set; }
        /// <summary>
        /// 表单字段（冗余)
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_FrmContentParse",ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "表单字段")]
        public string F_FrmContentParse { get; set; }
        /// <summary>
        /// 表单参数
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_FrmContent",ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "表单参数")]
        public string F_FrmContent { get; set; }
        /// <summary>
        /// 表单ID
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_FrmId",ColumnDataType = "nvarchar(50)", ColumnDescription = "表单ID")]
        public string F_FrmId { get; set; }
        /// <summary>
        /// 流程类型
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_SchemeType", ColumnDataType = "nvarchar(50)", ColumnDescription = "流程类型")]
        public string F_SchemeType { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = false, ColumnDescription = "等级")]
        public int F_FlowLevel { get; set; }
        /// <summary>
        /// 实例备注
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_Description",ColumnDataType = "nvarchar(200)", ColumnDescription = "实例备注")]
        public string F_Description { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = false, ColumnDescription = "是否完成")]
        public int F_IsFinish { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        /// <returns></returns>
        [SugarColumn(IsNullable = true, ColumnName = "F_MakerList", ColumnDataType = "longtext,nvarchar(max)", ColumnDescription = "执行人")]
        public string F_MakerList { get; set; }
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
        //[SugarColumn(IsNullable = true,ColumnDescription = "创建时间")]
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
        /// <summary>
        /// 如果下个执行节点是运行时指定执行者。需要传指定的类型
        /// <para>取值为RUNTIME_SPECIAL_ROLE、RUNTIME_SPECIAL_USER</para>
        /// </summary>
        [SugarColumn(IsIgnore=true)]
        public string NextNodeDesignateType { get; set; }

        /// <summary>
        /// 如果下个执行节点是运行时指定执行者。该值表示具体的执行者
        /// <para>如果NodeDesignateType为RUNTIME_SPECIAL_ROLE，则该值为指定的角色</para>
        /// <para>如果NodeDesignateType为RUNTIME_SPECIAL_USER，则该值为指定的用户</para>
        /// </summary>
        [SugarColumn(IsIgnore=true)]
        public string[] NextNodeDesignates { get; set; }
        [SugarColumn(IsIgnore=true)]
        public string NextMakerName { get; set; }
        [SugarColumn(IsIgnore=true)]
        public string CurrentMakerName { get; set; }
        [SugarColumn(IsIgnore=true)]
        public string CurrentNodeDesignateType { get; set; }
    }
}
