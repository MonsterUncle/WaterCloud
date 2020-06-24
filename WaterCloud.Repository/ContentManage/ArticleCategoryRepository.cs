using Chloe;
using WaterCloud.DataBase;
using WaterCloud.Domain.ContentManage;

namespace WaterCloud.Repository.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-09 19:42
    /// 描 述：新闻类别数据实现类
    /// </summary>
    public class ArticleCategoryRepository : RepositoryBase<ArticleCategoryEntity>,IArticleCategoryRepository
    {
        private string ConnectStr;
        private string providerName;

        private IDbContext dbcontext;
        public ArticleCategoryRepository(IDbContext context) : base(context)
        {
            dbcontext = context;
        }
        public ArticleCategoryRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            dbcontext = GetDbContext();
        }
    }
}
