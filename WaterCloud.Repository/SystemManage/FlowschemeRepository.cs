using Chloe;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.Repository.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-07-10 08:49
    /// 描 述：流程设计数据实现类
    /// </summary>
    public class FlowschemeRepository : RepositoryBase<FlowschemeEntity>,IFlowschemeRepository
    {
        private IDbContext dbcontext;
        public FlowschemeRepository(IDbContext context)
             : base(context)
        {
            dbcontext = context;
        }
        public FlowschemeRepository(string ConnectStr, string providerName)
             : base(ConnectStr, providerName)
        {
            dbcontext = GetDbContext();
        }
    }
}
