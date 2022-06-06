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
        [SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string Id { get; set; }
        /// <summary>
        /// 项目类型(1菜单，2按钮，3字段)
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "项目类型")]
        public int? ItemType { get; set; }
        /// <summary>
        /// /项目Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "ItemId", ColumnDataType = "nvarchar(50)", ColumnDescription = "项目Id")]
        public string ItemId { get; set; }
        /// <summary>
        /// 目标类型(1角色，2租户)
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "目标类型")]
        public int? ObjectType { get; set; }
        /// <summary>
        /// 目标Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "ObjectId",ColumnDataType = "nvarchar(50)", ColumnDescription = "目标Id")]
        public string ObjectId { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "排序码")]
        public int? SortCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? CreatorTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "CreatorUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
        public string CreatorUserId { get; set; }
    }
}
