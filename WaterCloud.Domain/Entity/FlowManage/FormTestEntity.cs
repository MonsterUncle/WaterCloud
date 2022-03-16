/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System;
using SqlSugar;

namespace WaterCloud.Domain.FlowManage
{
    [SugarTable("oms_formtest")]
    public class FormTestEntity : IEntity<FormTestEntity>, ICreationAudited
    {
        [SugarColumn(ColumnName ="Id", IsPrimaryKey = true, ColumnDescription ="主键Id")]
        public string Id { get; set; }
        [SugarColumn(IsNullable = false, ColumnName = "UserName",ColumnDataType = "nvarchar(10)")]
        public string UserName { get; set; }
        [SugarColumn(IsNullable = false)]
        public string RequestType { get; set; }
        [SugarColumn(IsNullable = true)]
        public DateTime? StartTime { get; set; }
        [SugarColumn(IsNullable = true)]
        public DateTime? EndTime { get; set; }
        [SugarColumn(IsNullable = false, ColumnName = "RequestComment",ColumnDataType = "longtext")]
        public string RequestComment { get; set; }
        [SugarColumn(IsNullable = true, ColumnName = "Attachment", ColumnDataType = "longtext")]
        public string Attachment { get; set; }
        [SugarColumn(IsNullable = true, ColumnName = "FlowInstanceId", ColumnDataType = "nvarchar(50)")]
        public string FlowInstanceId { get; set; }
        [SugarColumn(IsNullable = true)]
        public DateTime? CreatorTime { get; set; }
        [SugarColumn(IsNullable = true, ColumnName = "CreatorUserId",ColumnDataType = "nvarchar(50)")]
        public string CreatorUserId { get; set; }
        [SugarColumn(IsNullable = true, ColumnName = "CreatorUserName",ColumnDataType = "nvarchar(50)")]
        public string CreatorUserName { get; set; }

    }
}
