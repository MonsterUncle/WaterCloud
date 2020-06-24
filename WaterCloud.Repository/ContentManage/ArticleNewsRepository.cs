using Chloe;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ubiety.Dns.Core.Records.General;
using WaterCloud.DataBase;
using WaterCloud.Domain.ContentManage;

namespace WaterCloud.Repository.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-09 19:42
    /// 描 述：新闻管理数据实现类
    /// </summary>
    public class ArticleNewsRepository : RepositoryBase<ArticleNewsEntity>,IArticleNewsRepository
    {
        private string ConnectStr;
        private string providerName;
        

        private IDbContext dbcontext;
        public ArticleNewsRepository(IDbContext context) : base(context)
        {
            dbcontext = context;
        }
        public ArticleNewsRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            dbcontext = GetDbContext();
        }
        public IQuery<ArticleNewsEntity> GetList(string _keyword,string _categoryId)
        {
            using (var db = new RepositoryBase(ConnectStr, providerName))
            {
                var query = db.IQueryable<ArticleNewsEntity>(a => a.F_EnabledMark == true)
                    .LeftJoin<ArticleCategoryEntity>((a, b) => a.F_CategoryId == b.F_Id && b.F_EnabledMark == true)
                    .Select((a, b) => new ArticleNewsEntity
                    {
                        F_Id = a.F_Id,
                        F_CategoryId=a.F_CategoryId,
                        F_CategoryName=b.F_FullName,
                        F_Title=a.F_Title,
                        F_LinkUrl = a.F_LinkUrl,
                        F_ImgUrl = a.F_ImgUrl,
                        F_SeoTitle = a.F_SeoTitle,
                        F_SeoKeywords = a.F_SeoKeywords,
                        F_SeoDescription = a.F_SeoDescription,
                        F_Tags = a.F_Tags,
                        F_Zhaiyao = a.F_Zhaiyao,
                        F_Description = a.F_Description,
                        F_SortCode = a.F_SortCode,
                        F_IsTop = a.F_IsTop,
                        F_IsHot = a.F_IsHot,
                        F_IsRed = a.F_IsRed,
                        F_Click = a.F_Click,
                        F_Source = a.F_Source,
                        F_Author = a.F_Author,
                        F_EnabledMark = a.F_EnabledMark,
                        F_CreatorTime = a.F_CreatorTime,
                        F_CreatorUserId = a.F_CreatorUserId,
                        F_DeleteMark = a.F_DeleteMark,
                        F_DeleteTime = a.F_DeleteTime,
                        F_DeleteUserId = a.F_DeleteUserId,
                        F_LastModifyTime = a.F_LastModifyTime,
                        F_LastModifyUserId = a.F_LastModifyUserId,
                    });
                if (!string.IsNullOrEmpty(_keyword))
                {
                    query = query.Where(a => a.F_Title.Contains(_keyword));
                }
                if (!string.IsNullOrEmpty(_categoryId))
                {
                    query = query.Where(a => a.F_CategoryId.Contains(_categoryId));
                }
                return query;
            }
        }
        public IQuery<ArticleNewsEntity> GetForm(string keyValue)
        {
            using (var db = new RepositoryBase(ConnectStr, providerName))
            {
                var query = db.IQueryable<ArticleNewsEntity>(a => a.F_EnabledMark == true)
                    .LeftJoin<ArticleCategoryEntity>((a, b) => a.F_CategoryId == b.F_Id && b.F_EnabledMark == true)
                    .Select((a, b) => new ArticleNewsEntity
                    {
                        F_Id = a.F_Id,
                        F_CategoryId = a.F_CategoryId,
                        F_CategoryName = b.F_FullName,
                        F_Title = a.F_Title,
                        F_LinkUrl = a.F_LinkUrl,
                        F_ImgUrl = a.F_ImgUrl,
                        F_SeoTitle = a.F_SeoTitle,
                        F_SeoKeywords = a.F_SeoKeywords,
                        F_SeoDescription = a.F_SeoDescription,
                        F_Tags = a.F_Tags,
                        F_Zhaiyao = a.F_Zhaiyao,
                        F_Description = a.F_Description,
                        F_SortCode = a.F_SortCode,
                        F_IsTop = a.F_IsTop,
                        F_IsHot = a.F_IsHot,
                        F_IsRed=a.F_IsRed,
                        F_Click = a.F_Click,
                        F_Source = a.F_Source,
                        F_Author = a.F_Author,
                        F_EnabledMark = a.F_EnabledMark,
                        F_CreatorTime = a.F_CreatorTime,
                        F_CreatorUserId = a.F_CreatorUserId,
                        F_DeleteMark = a.F_DeleteMark,
                        F_DeleteTime = a.F_DeleteTime,
                        F_DeleteUserId = a.F_DeleteUserId,
                        F_LastModifyTime = a.F_LastModifyTime,
                        F_LastModifyUserId = a.F_LastModifyUserId,
                    });
                if (!string.IsNullOrEmpty(keyValue))
                {
                    query = query.Where(a => a.F_Id==keyValue);
                }
                return query;
            }
        }
    }
}
