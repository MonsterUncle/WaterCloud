using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterCloud.DingTalk
{
    /// <summary>
    /// 审批实例
    /// </summary>
    public class  Processinstance
    {
        /// <summary>
        /// AccessToken
        /// </summary>
        public string AccessToken { get; set; }
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
        public List<string> CcUserids { get; set; }
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
        public string CreateTime { get; set; }
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
        public List<FormComponentValue> FormComponentValues { get; set; }

        /// <summary>
        /// 表单协作明细字段（字段应属于
        /// </summary>
        public List<FormComponentValue> ValueDetails { get; set; }
        /// <summary>
        /// 操作记录列表
        /// </summary>
        public List<OperationRecord> OperationRecords { get; set; }
        /// <summary>
        /// 已审批任务列表，可以通过此列表获取已审批人
        /// </summary>
        public List<Task> Tasks { get; set; }

        public void AddForm(FormComponentValue form)
        {
            if (FormComponentValues == null)
            {
                FormComponentValues = new List<FormComponentValue>();
            }
            FormComponentValues.Add(form);
        }
    }
    /// <summary>
    /// 审批form表单实体
    /// </summary>
    public class FormComponentValue
    {
        public FormField FieldType { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 拓展属性值
        /// </summary>
        public string ExtValue { get; set; }
       
    }
    /// <summary>
    /// 操作记录实体
    /// </summary>
    public class OperationRecord
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 操作类型，分为:EXECUTE_TASK_NORMAL（正常执行任务），EXECUTE_TASK_AGENT（代理人执行任务），APPEND_TASK_BEFORE（前加签任务），APPEND_TASK_AFTER（后加签任务），REDIRECT_TASK（转交任务），START_PROCESS_INSTANCE（发起流程实例），TERMINATE_PROCESS_INSTANCE（终止(撤销)流程实例），FINISH_PROCESS_INSTANCE（结束流程实例），ADD_REMARK（添加评论）
        /// </summary>
        public string OperationType { get; set; }
        /// <summary>
        /// AGREE（同意），REFUSE（拒绝）
        /// </summary>
        public string OperationResult { get; set; }
        /// <summary>
        /// 评论
        /// </summary>
        public string Remark { get; set; }
    }
    /// <summary>
    /// 已审批任务实体
    /// </summary>
    public class Task
    {
        /// <summary>
        /// 任务处理人
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 任务状态，分为:NEW（未启动），RUNNING（处理中），PAUSED（暂停），CANCELED（取消）
        /// </summary>
        public string TaskStatus { get; set; }
        /// <summary>
        /// 任务结果，分为:AGREE（同意），REFUSE（拒绝），REDIRECTED（转交）
        /// </summary>
        public string TaskResult { get; set; }
        /// <summary>
        /// 开始时间。yyyy-MM-dd HH:mm:ss格式
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 结束时间。yyyy-MM-dd HH:mm:ss格式
        /// </summary>
        public DateTime FinishTime { get; set; }
        /// <summary>
        /// 任务节点id
        /// </summary>
        public string TaskId { get; set; }
    }

    /// <summary>
    /// 审批模板 实体
    /// </summary>
    public class ProcessTemplate
    {
        /// <summary>
        /// 审批模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图标url
        /// </summary>
        public string IconUrl { get; set; }
        /// <summary>
        /// 模板唯一标识
        /// </summary>
        public string ProcessCode { get; set; }
        /// <summary>
        /// 模板跳转url
        /// </summary>
        public string Url { get; set; }
    }
    #region 表单字段类型
    public enum FormField
    {
        /// <summary>
        /// 单选文本（常规字段）
        /// </summary>
        singletext,
        /// <summary>
        /// 多选文本（常规字段）
        /// </summary>
        multipletext,
        /// <summary>
        /// 图片
        /// </summary>
        image,
        /// <summary>
        /// 明细（由多个常规表单字段组成）
        /// </summary>
        detail,
        /// <summary>
        /// 附件,由[spaceId，fileName，fileSize，fileType，fileId]格式组成
        /// </summary>
        attach

    }
    #endregion
    #region 审批业务状态枚举
    /// <summary>
    /// 任务状态，分为:NEW（未启动），RUNNING（处理中），PAUSED（暂停），CANCELED（取消）
    /// </summary>
    public enum TaskStatus
    {
        NEW,
        RUNNING,
        PAUSED,
        CANCELED
    }
    /// <summary>
    /// 任务结果，分为:AGREE（同意），REFUSE（拒绝），REDIRECTED（转交）
    /// </summary>
    public enum TaskResult
    {
        AGREE,
        REFUSE,
        REDIRECTED
    }
    /// <summary>
    /// 操作类型，分为:EXECUTE_TASK_NORMAL（正常执行任务），EXECUTE_TASK_AGENT（代理人执行任务），APPEND_TASK_BEFORE（前加签任务），APPEND_TASK_AFTER（后加签任务），REDIRECT_TASK（转交任务），START_PROCESS_INSTANCE（发起流程实例），TERMINATE_PROCESS_INSTANCE（终止(撤销)流程实例），FINISH_PROCESS_INSTANCE（结束流程实例），ADD_REMARK（添加评论）
    /// </summary>
    public enum OperationType
    {
        EXECUTE_TASK_NORMAL,
        EXECUTE_TASK_AGENT,
        APPEND_TASK_BEFORE,
        APPEND_TASK_AFTER,
        REDIRECT_TASK,
        START_PROCESS_INSTANCE,
        FINISH_PROCESS_INSTANCE,
        ADD_REMARK
    }
    #endregion
}
