/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System;
using Chloe.Annotations;

namespace WaterCloud.Domain.FlowManage
{
    [TableAttribute("oms_formtest")]
    public class FormTestEntity : IEntity<FormTestEntity>, ICreationAudited
    {
        [ColumnAttribute("F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        public string F_UserName { get; set; }
        public string F_RequestType { get; set; }
        public DateTime? F_StartTime { get; set; }
        public DateTime? F_EndTime { get; set; }
        public string F_RequestComment { get; set; }
        public string F_Attachment { get; set; }
        public string F_FlowInstanceId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public string F_CreatorUserName { get; set; }

    }
}
