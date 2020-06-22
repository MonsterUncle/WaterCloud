using Chloe;
using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Service
{
    public class DataFilterService<T> where T : class, new()
    {
        // 用户信息
        protected OperatorModel currentuser;
        // 用于当前表操作
        protected RepositoryBase<T> Repository;
        // 用于其他表操作
        protected IDbContext dbcontext;
        public DataFilterService(IDbContext context) {
            currentuser = OperatorProvider.Provider.GetCurrent();
            if (currentuser != null&& !(currentuser.DBProvider == GlobalContext.SystemConfig.DBProvider && currentuser.DbString == GlobalContext.SystemConfig.DBConnectionString))
            {
                Repository = new RepositoryBase<T>(currentuser.DbString, currentuser.DBProvider);
                dbcontext = Repository.GetDbContext();
            }
            else
            {
                Repository = new RepositoryBase<T>(context);
                dbcontext = context;
            }
        }
        /// <summary>
        ///  获取当前登录用户的数据访问权限(单表)
        /// </summary>
        /// <param name=""parameterName>linq表达式参数的名称，如u=>u.name中的"u"</param>
        /// <param name=""moduleName>菜单名称</param>
        /// <returns></returns>
        protected IQuery<T> GetDataPrivilege(string parametername, string moduleName, IQuery<T> query=null)
        {
            if (query==null)
            {
                query = Repository.IQueryable();
            }
            if (!CheckDataPrivilege(moduleName))
            {
                return query;
            }
            var rule = dbcontext.Query<DataPrivilegeRuleEntity>(u => u.F_ModuleCode == moduleName).FirstOrDefault();
            if (rule.F_PrivilegeRules.Contains(Define.DATAPRIVILEGE_LOGINUSER) ||
                                             rule.F_PrivilegeRules.Contains(Define.DATAPRIVILEGE_LOGINROLE) ||
                                             rule.F_PrivilegeRules.Contains(Define.DATAPRIVILEGE_LOGINORG))
            {

                //即把{loginUser} =='xxxxxxx'换为 loginUser.User.Id =='xxxxxxx'，从而把当前登录的用户名与当时设计规则时选定的用户id对比
                rule.F_PrivilegeRules = rule.F_PrivilegeRules.Replace(Define.DATAPRIVILEGE_LOGINUSER, currentuser.UserId);
                var roles = currentuser.RoleId;
                //var roles = loginUser.Roles.Select(u => u.Id).ToList();//多角色
                //roles.Sort();
                rule.F_PrivilegeRules = rule.F_PrivilegeRules.Replace(Define.DATAPRIVILEGE_LOGINROLE,
                    roles);
                var orgs = currentuser.DepartmentId;
                //var orgs = loginUser.Orgs.Select(u => u.Id).ToList();//多部门
                //orgs.Sort();
                rule.F_PrivilegeRules = rule.F_PrivilegeRules.Replace(Define.DATAPRIVILEGE_LOGINORG,
                    orgs);
            }
            return query.GenerateFilter(parametername,
                JsonHelper.ToObject<List<FilterList>>(rule.F_PrivilegeRules));
        }
        /// <summary>
        ///  获取当前登录用户的数据访问权限(复杂查询)
        /// </summary>
        /// <param name=""parameterName>linq表达式参数的名称，如u=>u.name中的"u"</param>
        /// <param name=""moduleName>菜单名称</param>
        /// <returns></returns>
        protected IQuery<TEntity> GetDataPrivilege<TEntity>(string parametername, string moduleName, IQuery<TEntity> query)
        {
            if (!CheckDataPrivilege(moduleName))
            {
                return query;
            }
            var rule = dbcontext.Query<DataPrivilegeRuleEntity>(u => u.F_ModuleCode == moduleName).FirstOrDefault();
            if (rule.F_PrivilegeRules.Contains(Define.DATAPRIVILEGE_LOGINUSER) ||
                                             rule.F_PrivilegeRules.Contains(Define.DATAPRIVILEGE_LOGINROLE) ||
                                             rule.F_PrivilegeRules.Contains(Define.DATAPRIVILEGE_LOGINORG))
            {

                //即把{loginUser} =='xxxxxxx'换为 loginUser.User.Id =='xxxxxxx'，从而把当前登录的用户名与当时设计规则时选定的用户id对比
                rule.F_PrivilegeRules = rule.F_PrivilegeRules.Replace(Define.DATAPRIVILEGE_LOGINUSER, currentuser.UserId);
                var roles = currentuser.RoleId;
                rule.F_PrivilegeRules = rule.F_PrivilegeRules.Replace(Define.DATAPRIVILEGE_LOGINROLE,
                    roles);
                var orgs = currentuser.DepartmentId;
                rule.F_PrivilegeRules = rule.F_PrivilegeRules.Replace(Define.DATAPRIVILEGE_LOGINORG,
                    orgs);
            }
            return query.GenerateFilter(parametername,
                JsonHelper.ToObject<List<FilterList>>(rule.F_PrivilegeRules));
        }
        /// <summary>
        ///  获取当前登录用户是否需要数据控制
        /// </summary>
        /// <param name=""moduleName>菜单名称</param>
        /// <returns></returns>
        protected bool CheckDataPrivilege(string moduleName)
        {
            if (currentuser.UserCode == Define.SYSTEM_USERNAME) return false;  //超级管理员特权
            var rule = dbcontext.Query<DataPrivilegeRuleEntity>(u => u.F_ModuleCode == moduleName).FirstOrDefault();
            ////系统菜单也不需要数据权限 跟字段重合取消这样处理
            //var module = UnitWork.FindEntity<ModuleEntity>(u => u.F_EnCode == moduleName).Result;
            if (rule == null)
            {
                return false; //没有设置数据规则，那么视为该资源允许被任何主体查看
            }
            //if (rule == null|| module.F_IsPublic==true)
            //{
            //    return false; //没有设置数据规则，那么视为该资源允许被任何主体查看
            //}
            return true;
        }
        /// <summary>
        ///  字段权限处理
        /// </summary>
        ///<param name=""list>数据列表</param>
        /// <param name=""moduleName>菜单名称</param>
        /// <returns></returns>
        protected List<TEntity> GetFieldsFilterData<TEntity>(List<TEntity> list, string moduleName)
        {
            //管理员跳过
            if (currentuser.RoleId == "admin"|| currentuser.IsSystem)
            {
                return list;
            }
            //系统菜单跳过
            var module = dbcontext.Query<ModuleEntity>(u => u.F_EnCode == moduleName).FirstOrDefault();
            //判断是否需要字段权限
            if (module.F_IsFields==false)
            {
                return list;
            }
            var rolelist = currentuser.RoleId.Split(',');
            var rule = dbcontext.Query<RoleAuthorizeEntity>(u=> rolelist.Contains(u.F_ObjectId)&&u.F_ItemType==3).Select(a=>a.F_ItemId).Distinct().ToList();
            var fieldsList = dbcontext.Query<ModuleFieldsEntity>(u => (rule.Contains(u.F_Id)||u.F_IsPublic==true)&&u.F_ModuleId==module.F_Id).Select(u => u.F_EnCode).ToList();
            //反射获取主键
            PropertyInfo pkProp = typeof(TEntity).GetProperties().Where(p => p.GetCustomAttributes(typeof(ColumnAttribute), false).Length > 0).FirstOrDefault();
            var idName = "F_Id";
            if (pkProp == null)
            {
                idName = pkProp.Name;
            }
            fieldsList.Add(idName);
            fieldsList = fieldsList.Distinct().ToList();
            if (list.Count==0)
            {
                return list;
            }
            return DataTableHelper.ListFilter(list, fieldsList);
        }
        /// <summary>
        ///  字段权限处理
        /// </summary>
        /// <returns></returns>
        protected TEntity GetFieldsFilterData<TEntity>(TEntity entity, string moduleName)
        {
            List<TEntity> list = new List<TEntity>();
            list.Add(entity);
            return GetFieldsFilterData(list, moduleName)[0];
        }
    }
}
