using Chloe;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Repository.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-01 09:44
    /// 描 述：数据权限数据实现类
    /// </summary>
    public class DataPrivilegeRuleRepository : RepositoryBase<DataPrivilegeRuleEntity>,IDataPrivilegeRuleRepository
    {
        private IDbContext dbcontext;
        public DataPrivilegeRuleRepository(IDbContext context) : base(context)
        {
            dbcontext = context;
        }
        public DataPrivilegeRuleRepository(string ConnectStr, string providerName)
             : base(ConnectStr, providerName)
        {
            dbcontext = GetDbContext();
        }
    }
}
