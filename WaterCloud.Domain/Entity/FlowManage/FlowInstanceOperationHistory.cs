﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a CodeSmith Template.
//
//     DO NOT MODIFY contents of this file. Changes to this
//     file will be lost if the code is regenerated.
//     Author:Yubao Li
// </autogenerated>
//------------------------------------------------------------------------------
using System;
using Chloe.Annotations;

namespace WaterCloud.Domain.FlowManage
{
    /// <summary>
	/// 工作流实例操作记录
	/// </summary>
      [Table("oms_flowInstanceoperationhistory")]
    public class FlowInstanceOperationHistory
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        /// <returns></returns>
        [ColumnAttribute("F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 实例进程Id
        /// </summary>
        public string F_InstanceId { get; set; }
        /// <summary>
	    /// 操作内容
	    /// </summary>
        public string F_Content { get; set; }
        /// <summary>
	    /// 创建时间
	    /// </summary>
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
	    /// 创建用户主键
	    /// </summary>
        public string F_CreatorUserId { get; set; }
        /// <summary>
	    /// 创建用户
	    /// </summary>
        public string F_CreatorUserName { get; set; }
    }
}