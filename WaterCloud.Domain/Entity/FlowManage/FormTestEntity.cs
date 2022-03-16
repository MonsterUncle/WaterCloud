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
        [SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true, ColumnDescription ="主键Id")]
        public string F_Id { get; set; }
        [SugarColumn(IsNullable = false, ColumnName = "F_UserName",ColumnDataType = "nvarchar(10)")]
        public string F_UserName { get; set; }
        [SugarColumn(IsNullable = false)]
        public string F_RequestType { get; set; }
        [SugarColumn(IsNullable = true)]
        public DateTime? F_StartTime { get; set; }
        [SugarColumn(IsNullable = true)]
        public DateTime? F_EndTime { get; set; }
        [SugarColumn(IsNullable = false, ColumnName = "F_RequestComment",ColumnDataType = "longtext")]
        public string F_RequestComment { get; set; }
        [SugarColumn(IsNullable = true, ColumnName = "F_Attachment", ColumnDataType = "longtext")]
        public string F_Attachment { get; set; }
        [SugarColumn(IsNullable = true, ColumnName = "F_FlowInstanceId", ColumnDataType = "nvarchar(50)")]
        public string F_FlowInstanceId { get; set; }
        [SugarColumn(IsNullable = true)]
        public DateTime? F_CreatorTime { get; set; }
        [SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserId",ColumnDataType = "nvarchar(50)")]
        public string F_CreatorUserId { get; set; }
        [SugarColumn(IsNullable = true, ColumnName = "F_CreatorUserName",ColumnDataType = "nvarchar(50)")]
        public string F_CreatorUserName { get; set; }

    }
}
