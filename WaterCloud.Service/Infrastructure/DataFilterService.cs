using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.Service
{
    public class DataFilterService<T> where T : class, new()
    {
        protected OperatorModel loginUser = OperatorProvider.Provider.GetCurrent();
        /// <summary>
        /// 用于普通的数据库操作
        /// </summary>
        /// <value>The repository.</value>
        protected RepositoryBase<T> Repository = new RepositoryBase<T>();
        protected RepositoryBase UnitWork = new RepositoryBase();
        /// <summary>
        ///  获取当前登录用户的数据访问权限
        /// </summary>
        /// <param name=""parameterName>linq表达式参数的名称，如u=>u.name中的"u"</param>
        /// <param name=""moduleName>菜单名称</param>
        /// <returns></returns>
        protected IQuery<T> GetDataPrivilege(string parametername,string moduleName)
        {
            if (loginUser.UserCode == Define.SYSTEM_USERNAME) return Repository.IQueryable();  //超级管理员特权
            var rule = UnitWork.FindEntity<DataPrivilegeRuleEntity>(u => u.F_ModuleCode == moduleName).Result;
            if (rule == null) return Repository.IQueryable(); //没有设置数据规则，那么视为该资源允许被任何主体查看
            if (rule.F_PrivilegeRules.Contains(Define.DATAPRIVILEGE_LOGINUSER) ||
                                             rule.F_PrivilegeRules.Contains(Define.DATAPRIVILEGE_LOGINROLE) ||
                                             rule.F_PrivilegeRules.Contains(Define.DATAPRIVILEGE_LOGINORG))
            {

                //即把{loginUser} =='xxxxxxx'换为 loginUser.User.Id =='xxxxxxx'，从而把当前登录的用户名与当时设计规则时选定的用户id对比
                rule.F_PrivilegeRules = rule.F_PrivilegeRules.Replace(Define.DATAPRIVILEGE_LOGINUSER, loginUser.UserId);
                var roles = loginUser.RoleId;
                //var roles = loginUser.Roles.Select(u => u.Id).ToList();//多角色
                //roles.Sort();
                rule.F_PrivilegeRules = rule.F_PrivilegeRules.Replace(Define.DATAPRIVILEGE_LOGINROLE,
                    string.Join(',', roles));
                var orgs = loginUser.DepartmentId;
                //var orgs = loginUser.Orgs.Select(u => u.Id).ToList();//多部门
                //orgs.Sort();
                rule.F_PrivilegeRules = rule.F_PrivilegeRules.Replace(Define.DATAPRIVILEGE_LOGINORG,
                    string.Join(',', orgs));
            }
            return Repository.IQueryable().GenerateFilter(parametername,
                JsonHelper.ToObject<List<FilterList>>(rule.F_PrivilegeRules));
        }
    }
}
