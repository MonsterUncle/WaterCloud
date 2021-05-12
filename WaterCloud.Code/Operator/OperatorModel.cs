/*******************************************************************************
 * Copyright © 2016 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System;
namespace WaterCloud.Code
{
    public class OperatorModel
    {
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string CompanyId { get; set; }
        public string DepartmentId { get; set; }
        public string RoleId { get; set; }
        public string LoginIPAddress { get; set; }
        public string LoginIPAddressName { get; set; }
        public string LoginToken { get; set; }
        public DateTime LoginTime { get; set; }
        public bool IsSystem { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBoss { get; set; }
        public bool IsSenior { get; set; }
        public bool IsLeaderInDepts { get; set; }
        public bool IsSaleman { get; set; }
        // 拓展字段，2019-03-03
        public string DdUserId { get; set; }
        public string WxOpenId { get; set; }
        public string Avatar { get; set; }
        public string loginMark { get; set; }
        //扩展字段 数据库序号，2021-05-12
        public string DbNumber { get; set; }
    }
}
