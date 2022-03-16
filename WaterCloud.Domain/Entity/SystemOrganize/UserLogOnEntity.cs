/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace WaterCloud.Domain.SystemOrganize
{
    [SugarTable("sys_userlogon")]
    public class UserLogOnEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(ColumnName ="F_Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string F_Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_UserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "用户Id", UniqueGroupNameList = new string[] { "sys_userlogon" })]
        public string F_UserId { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "F_UserPassword",ColumnDataType = "nvarchar(50)", ColumnDescription = "用户密码")]
        public string F_UserPassword { get; set; }
        /// <summary>
        /// 用户密钥
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_UserSecretkey", ColumnDataType = "nvarchar(50)", ColumnDescription = "用户密钥")]
        public string F_UserSecretkey { get; set; }
        /// <summary>
        /// 登录开始时间
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "登录开始时间")]
        public DateTime? F_AllowStartTime { get; set; }
        /// <summary>
        /// 登录结束时间
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "登录结束时间")]
        public DateTime? F_AllowEndTime { get; set; }
        /// <summary>
        /// 锁定开始时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "锁定开始时间")]
        public DateTime? F_LockStartDate { get; set; }
        /// <summary>
        /// 锁定结束时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "锁定结束时间")]
        public DateTime? F_LockEndDate { get; set; }
        /// <summary>
        /// 第一次访问时间
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "第一次访问时间")]
        public DateTime? F_FirstVisitTime { get; set; }
        /// <summary>
        /// 上一次访问时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "上一次访问时间")]
        public DateTime? F_PreviousVisitTime { get; set; }
        /// <summary>
        /// 最后访问时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "最后访问时间")]
        public DateTime? F_LastVisitTime { get; set; }
        /// <summary>
        /// 修改密码时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "修改密码时间")]
        public DateTime? F_ChangePasswordDate { get; set; }
        /// <summary>
        /// 是否允许多用户登录
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否允许多用户登录")]
        public bool? F_MultiUserLogin { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "登录次数")]
        public int? F_LogOnCount { get; set; }
        /// <summary>
        /// 是否在线
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否在线")]
        public bool? F_UserOnLine { get; set; }
        /// <summary>
        /// 密保问题
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_Question",ColumnDataType = "nvarchar(50)", ColumnDescription = "密保问题")]
        public string F_Question { get; set; }
        /// <summary>
        /// 密保答案
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_AnswerQuestion",ColumnDataType = "longtext", ColumnDescription = "密保答案")]
        public string F_AnswerQuestion { get; set; }
        /// <summary>
        /// 是否校验登录IP
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否校验登录IP")]
        public bool? F_CheckIPAddress { get; set; }
        /// <summary>
        /// 系统语言
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_Language", ColumnDataType = "nvarchar(50)", ColumnDescription = "系统语言")]
        public string F_Language { get; set; }
        /// <summary>
        /// 系统主题
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_Theme", ColumnDataType = "nvarchar(50)", ColumnDescription = "系统主题")]
        public string F_Theme { get; set; }
        /// <summary>
        /// 登录session
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "F_LoginSession", ColumnDataType = "nvarchar(100)", ColumnDescription = "登录session")]
        public string F_LoginSession { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "错误码")]
        public Int32 F_ErrorNum { get; set; }
    }
}
