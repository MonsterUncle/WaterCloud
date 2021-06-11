/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using SqlSugar;
using System;

namespace WaterCloud.Domain.SystemOrganize
{
    /// <summary>
    /// 角色权限实体
    /// </summary>
    [SugarTable("sys_roleauthorize")]
    public class RoleAuthorizeEntity : IEntity<RoleAuthorizeEntity>, ICreationAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string F_Id { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "项目类型")]
        public int? F_ItemType { get; set; }
        /// <summary>
        /// /项目Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_ItemId", ColumnDataType = "nvarchar(50)", ColumnDescription = "项目Id")]
        public string F_ItemId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "目标类型")]
        public int? F_ObjectType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_ObjectId",ColumnDataType = "nvarchar(50)", ColumnDescription = "目标Id")]
        public string F_ObjectId { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "排序码")]
        public int? F_SortCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
        public string F_CreatorUserId { get; set; }
    }
}
