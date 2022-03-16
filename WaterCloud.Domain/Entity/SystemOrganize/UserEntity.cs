/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;
using Serenity.Data.Mapping;

namespace WaterCloud.Domain.SystemOrganize
{
    /// <summary>
    /// 用户实体
    /// </summary>
    [SugarTable("sys_user")]
    public class UserEntity : IEntity<UserEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(ColumnName ="Id", IsPrimaryKey = true,ColumnDescription ="主键Id")]
        public string Id { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        [Required(ErrorMessage = "账户不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "Account",ColumnDataType = "nvarchar(50)", ColumnDescription = "账户", UniqueGroupNameList = new string[] { "sys_user" })]
        public string Account { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "姓名不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "RealName",ColumnDataType = "nvarchar(50)", ColumnDescription = "姓名")]
        public string RealName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "NickName",ColumnDataType = "nvarchar(50)", ColumnDescription = "昵称")]
        public string NickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "HeadIcon", ColumnDataType = "nvarchar(50)", ColumnDescription = "头像")]
        public string HeadIcon { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Required(ErrorMessage = "性别不能为空")]
        [SugarColumn(IsNullable = true,ColumnDescription = "性别")]
        public bool? Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "生日")]
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "MobilePhone", ColumnDataType = "nvarchar(20)", ColumnDescription = "手机")]
        public string MobilePhone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "Email",ColumnDataType = "nvarchar(50)", ColumnDescription = "邮箱")]
        public string Email { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "WeChat",ColumnDataType = "nvarchar(50)", ColumnDescription = "微信号")]
        public string WeChat { get; set; }
        /// <summary>
        /// 管理Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "ManagerId",ColumnDataType = "nvarchar(50)", ColumnDescription = "管理Id")]
        public string ManagerId { get; set; }
        /// <summary>
        /// 安全级别
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "安全级别")]
        public int? SecurityLevel { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "Signature",ColumnDataType = "longtext", ColumnDescription = "个性签名")]
        public string Signature { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary> 
        [Required(ErrorMessage = "公司不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "OrganizeId",ColumnDataType = "nvarchar(50)", ColumnDescription = "公司Id")]
        public string OrganizeId { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>
        [Required(ErrorMessage = "部门不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "DepartmentId",ColumnDataType = "longtext", ColumnDescription = "部门Id")]
        public string DepartmentId { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        [Required(ErrorMessage = "角色不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "RoleId",ColumnDataType = "longtext", ColumnDescription = "角色Id")]
        public string RoleId { get; set; }
        /// <summary>
        /// 岗位Id
        /// </summary>
        [Required(ErrorMessage = "职位不能为空")]
        [SugarColumn(IsNullable = true, ColumnName = "DutyId",ColumnDataType = "longtext", ColumnDescription = "岗位Id")]
        public string DutyId { get; set; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "是否管理员")]
        public bool? IsAdmin { get; set; }
        /// <summary>
        /// 是否老板
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "是否老板")]
        public bool? IsBoss { get; set; }
        /// <summary>
        /// 是否高管
        /// </summary>
        [SugarColumn(IsNullable = true,  ColumnDescription = "是否高管")]
        public bool? IsSenior { get; set; }
        /// <summary>
        /// 是否部门领导
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否部门领导")]
        public bool? IsLeaderInDepts { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "排序码")]
        public int? SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除标记")]
        public bool? DeleteMark { get; set; }
        /// <summary>
        /// 有效标记
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "有效标记")]
        public bool? EnabledMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "Description", ColumnDataType = "longtext", ColumnDescription = "备注")]
        public string Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? CreatorTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "CreatorUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "创建人Id")]
        public string CreatorUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "修改时间")]
        public DateTime? LastModifyTime { get; set; }
        /// <summary>
        /// 修改人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "LastModifyUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "修改人Id")]
        public string LastModifyUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        [SugarColumn(IsNullable = true,ColumnDescription = "删除时间")]
        public DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 删除人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "DeleteUserId",ColumnDataType = "nvarchar(50)", ColumnDescription = "删除人Id")]
        public string DeleteUserId { get; set; }

        // 拓展字段，2019-03-03
        /// <summary>
        /// 钉钉用户Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "DingTalkUserId", ColumnDataType = "nvarchar(50)", ColumnDescription = "钉钉用户Id")]
        public string DingTalkUserId { get; set; }
        /// <summary>
        /// 钉钉用户名称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "DingTalkUserName", ColumnDataType = "nvarchar(50)", ColumnDescription = "钉钉用户名称")]
        public string DingTalkUserName { get; set; }
        /// <summary>
        /// 钉钉头像
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "DingTalkAvatar", ColumnDataType = "nvarchar(100)", ColumnDescription = "钉钉头像")]
        public string DingTalkAvatar { get; set; }
        /// <summary>
        /// 微信开放Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "WxOpenId", ColumnDataType = "nvarchar(50)", ColumnDescription = "微信开放Id")]
        public string WxOpenId { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "WxNickName",ColumnDataType = "nvarchar(50)", ColumnDescription = "微信昵称")]
        public string WxNickName { get; set; }
        /// <summary>
        /// 微信头像
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnName = "HeadImgUrl", ColumnDataType = "nvarchar(100)", ColumnDescription = "微信头像")]
        public string HeadImgUrl { get; set; }

        [SugarColumn(IsIgnore=true)]
        //多选显示字段
        public string DepartmentName { get; set; }
        [SugarColumn(IsIgnore=true)]
        //多选显示字段
        public string RoleName { get; set; }
        [SugarColumn(IsIgnore=true)]
        //tablecheck字段
        public bool LAY_CHECKED { get; set; }
        [SugarColumn(IsIgnore=true)]
        //tablecheck字段
        public int MsgCout { get; set; }
    }
}
