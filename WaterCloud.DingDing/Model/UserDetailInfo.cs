using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;

namespace WaterCloud.DingTalk
{
    public class UserDetailInfo
    {
        public string OpenId { get; set; }
        
        /// <summary>
        /// 员工唯一标识ID（不可修改）
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 成员名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 是否已经激活，true表示已激活，false表示未激活
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// 在对应的部门中的排序，Map结构的json字符串，key是部门的Id，value是人员在这个部门的排序值
        /// </summary>
        public string OrderInDepts { get; set; }
        /// <summary>
        /// 是否为企业的管理员，true表示是，false表示不是
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 是否为企业的老板，true表示是，false表示不是
        /// </summary>
        public bool IsBoss { get; set; }
        /// <summary>
        ///  在当前isv全局范围内唯一标识一个用户的身份,用户无法修改
        /// </summary>
        public string Unionid { get; set; }
        /// <summary>
        /// 在对应的部门中是否为主管：Map结构的json字符串，key是部门的Id，value是人员在这个部门中是否为主管，true表示是，false表示不是
        /// </summary>
        public string IsLeaderInDepts { get; set; }
        /// <summary>
        ///  是否是部门的主管，true表示是，false表示不是
        /// </summary>
        public bool IsLeader { get; set; }
        /// <summary>
        /// 是否号码隐藏，true表示隐藏，false表示不隐藏
        /// </summary>
        public bool IsHide { get; set; }
        /// <summary>
        /// 成员所属部门id列表
        /// </summary>
        public List<long> Department { get; set; }
        /// <summary>
        /// 职位信息
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 头像url
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 入职时间。Unix时间戳 （在OA后台通讯录中的员工基础信息中维护过入职时间才会返回)
        /// </summary>
        public string HiredDate { get; set; }
        /// <summary>
        /// 员工工号
        /// </summary>
        public string JobNumber { get; set; }
        /// <summary>
        /// 是否是高管
        /// </summary>
        public bool IsSenior { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 部门排序
        /// </summary>
        public long Order { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WorkPlace { get; set; }
        

        ///// <summary>
        ///// 角色信息（ISV不可见），json数组格式
        ///// </summary>
        //public List<OapiUserGetResponse.RolesDomain> Roles { get; set; }  

        /// <summary>
        /// 新增的钉钉用户在系统中的角色
        /// </summary>
        public string RolesId { get; set; }

    }
}
