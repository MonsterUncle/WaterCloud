using Chloe;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.Repository.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-07-08 14:33
    /// 描 述：表单设计数据实现类
    /// </summary>
    public class FormRepository : RepositoryBase<FormEntity>,IFormRepository
    {
        private IDbContext dbcontext;
        public FormRepository(IDbContext context)
             : base(context)
        {
            dbcontext = context;
        }
        public FormRepository(string ConnectStr, string providerName)
             : base(ConnectStr, providerName)
        {
            dbcontext = GetDbContext();
        }
    }
}
