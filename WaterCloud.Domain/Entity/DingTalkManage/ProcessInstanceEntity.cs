/*******************************************************************************
 * Copyright © 2018 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud
 * Website：
*********************************************************************************/

using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using Chloe.Annotations;

namespace WaterCloud.Entity.DingTalkManage
{
    //DingTalkCorpConfig
    [TableAttribute("Dtk_ProcessInstanceList")]
    public class ProcessInstanceEntity : IEntityV2<ProcessInstanceEntity>, ICreationAuditedV2, IModificationAuditedV2
    {
        [ColumnAttribute("uuId", IsPrimaryKey = true)]
        /// <summary>
        /// 主键
        /// </summary>
        public string uuId { get; set; }
        /// <summary>
        /// 审批流的唯一码，process_code就在审批流编辑的页面URL中
        /// </summary>
        public string ProcessCode { get; set; }
        /// <summary>
        /// 审批人userid列表，最大列表长度：20。多个审批人用逗号分隔，按传入的顺序依次审批
        /// </summary>
        public string Approvers { get; set; }
        /// <summary>
        /// 审批实例发起人的userid
        /// </summary>
        public string OriginatorUserId { get; set; }
        /// <summary>
        /// 发起人所在的部门，如果发起人属于根部门，传-1
        /// </summary>
        public long OriginatorDeptId { get; set; }
        /// <summary>
        /// (可选)抄送人userid列表，最大列表长度：20。多个抄送人用逗号分隔
        /// </summary>
        public string CcUserids { get; set; }
        /// <summary>
        /// (可选)抄送时间，分为（START, FINISH, START_FINISH）
        /// </summary>
        public string CcPosition { get; set; }
        /// <summary>
        /// 审批实例标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string CreateTime2 { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string FinishTime { get; set; }
        /// <summary>
        /// 审批状态，分为:NEW（刚创建）|RUNNING（运行中）|TERMINATED（被终止）|COMPLETED（完成）|CANCELED（取消）
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 审批结果，分为 agree 和 refuse
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 审批实例业务动作，MODIFY表示该审批实例是基于原来的实例修改而来，REVOKE表示该审批实例对原来的实例进行撤销，NONE表示正常发起
        /// </summary>
        public string BizAction { get; set; }
        /// <summary>
        /// 审批实例业务编号
        /// </summary>
        public string BusinessId { get; set; }
        /// <summary>
        /// 发起部门
        /// </summary>
        public string OriginatorDeptName { get; set; }
        public string name { get; set; }
        /// <summary>
        /// 表单详情列表
        /// </summary>
        public string FormComponentValues { get; set; }
        /// <summary>
        /// 已审批任务列表，可以通过此列表获取已审批人
        /// </summary>
        public string Tasks { get; set; }
        /// <summary>
        ///创建人
        /// </summary>		
        public string Creator { get; set; }
        /// <summary>
		/// 创建人ID
        /// </summary>		
        public string CreatorId { get; set; }
        /// <summary>
		/// 创建时间
        /// </summary>		
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>		
        public string Modifier { get; set; }
        /// <summary>
        /// 修改人ID
        /// </summary>		
        public string ModifierId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>		
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 删除人ID
        /// </summary>		
        public string DeleterId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>		
        public DateTime? DeleteTime { get; set; }
    }
}

