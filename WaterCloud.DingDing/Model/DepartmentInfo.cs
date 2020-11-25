using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;

namespace WaterCloud.DingTalk
{
    /// <summary>
    /// 部门信息
    /// </summary>
    public class DepartmentInfo
    {
        /// <summary>
        /// 部门id
        /// </summary>		
        public string Id { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>		
        public string Name { get; set; }
        /// <summary>
        /// 部门排序
        /// </summary>		
        public string Order { get; set; }
        /// <summary>
        /// 父部门id
        /// </summary>		
        public string Parentid { get; set; }
        /// <summary>
        /// 是否同步创建一个关联此部门的企业群
        /// </summary>		
        public bool CreateDeptGroup { get; set; }
        /// <summary>
        /// 入部门会自动加入该群
        /// </summary>		
        public bool AutoAddUser { get; set; }
        /// <summary>
        /// 是否隐藏部门
        /// </summary>		
        public bool DeptHiding { get; set; }
        /// <summary>
        /// 可以查看指定隐藏部门的其他部门列表,使用“\|”符号进行分割
        /// </summary>		
        public string DeptPermits { get; set; }
        /// <summary>
        /// 可以查看指定隐藏部门的其他人员列表,使用“\|”符号进行分割
        /// </summary>		
        public string UserPermits { get; set; }
        /// <summary>
        /// 是否本部门的员工仅可见员工自己
        /// </summary>		
        public bool OuterDept { get; set; }
        /// <summary>
        /// 额外可见部门,使用“\|”符号进行分割
        /// </summary>		
        public string OuterPermitDepts { get; set; }
        /// <summary>
        /// 额外可见人员,使用“\|”符号进行分割
        /// </summary>		
        public string OuterPermitUsers { get; set; }
        /// <summary>
        /// 企业群群主
        /// </summary>		
        public string OrgDeptOwner { get; set; }
        /// <summary>
        /// 部门的主管列表
        /// </summary>		
        public string DeptManagerUseridList { get; set; }
        /// <summary>
        /// 部门标识字段
        /// </summary>		
        public string SourceIdentifier { get; set; }
        /// <summary>
        /// 部门群是否包含子部门
        /// </summary>		
        public bool GroupContainSubDept { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

    }
}
