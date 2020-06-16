/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using Chloe.Annotations;
using System;

namespace WaterCloud.Domain.SystemManage
{
    [TableAttribute("sys_modulebutton")]
    public class ModuleButtonEntity : IEntity<ModuleButtonEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        [ColumnAttribute("F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        public string F_ModuleId { get; set; }
        public string F_ParentId { get; set; }
        public int? F_Layers { get; set; }
        public string F_EnCode { get; set; }
        public string F_FullName { get; set; }
        public string F_Icon { get; set; }
        public int? F_Location { get; set; }
        public string F_JsEvent { get; set; }
        public string F_UrlAddress { get; set; }
        public bool? F_Split { get; set; }
        public bool? F_IsPublic { get; set; }
        public bool? F_AllowEdit { get; set; }
        public bool? F_AllowDelete { get; set; }
        public int? F_SortCode { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
    }
}
