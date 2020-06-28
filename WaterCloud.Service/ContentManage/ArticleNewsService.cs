using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.Domain.ContentManage;
using WaterCloud.Repository.ContentManage;
using Chloe;

namespace WaterCloud.Service.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-09 19:42
    /// 描 述：新闻管理服务类
    /// </summary>
    public class ArticleNewsService : DataFilterService<ArticleNewsEntity>, IDenpendency
    {
        private IArticleNewsRepository service;
        
        public ArticleNewsService(IDbContext context) : base(context)
        {
            var currentuser = OperatorProvider.Provider.GetCurrent();
            service = currentuser != null && !(currentuser.DBProvider == GlobalContext.SystemConfig.DBProvider && currentuser.DbString == GlobalContext.SystemConfig.DBConnectionString) ? new ArticleNewsRepository(currentuser.DbString, currentuser.DBProvider) : new ArticleNewsRepository(context);

        }
        private string cacheKey = "watercloud_cms_articlenewsdata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        #region 获取数据
        public async Task<List<ArticleNewsEntity>> GetList(string keyword = "")
        {
            var cachedata = await service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_Title.Contains(keyword) || t.F_Tags.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public async Task<List<ArticleNewsEntity>> GetLookList(Pagination pagination,string keyword = "", string CategoryId="")
        {
            //获取新闻列表
            var query = service.GetList(keyword, CategoryId);

            //获取数据权限
            var list = GetDataPrivilege<ArticleNewsEntity>("u", className.Substring(0, className.Length - 7), query);
            
            list = list.Where(u => u.F_DeleteMark==false);
            return GetFieldsFilterData(await service.OrderList(list, pagination),className.Substring(0, className.Length - 7));
        }
        /// <summary>
        /// 获取新闻详情
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public async Task<ArticleNewsEntity> GetForm(string keyValue)
        {
            //获取新闻详情
            var query = service.GetForm(keyValue);
            //字段权限处理
            return GetFieldsFilterData(query.FirstOrDefault(), className.Substring(0, className.Length - 7));
        }
        #endregion

        #region 提交数据
        public async Task SubmitForm(ArticleNewsEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(entity.F_Zhaiyao))
            {
                entity.F_Zhaiyao = WaterCloud.Code.TextHelper.GetSubString(WaterCloud.Code.WebHelper.NoHtml(entity.F_Description),255);
            }
            if (string.IsNullOrEmpty(entity.F_SeoTitle))
            {
                entity.F_SeoTitle = entity.F_Title;
            }
            if (string.IsNullOrEmpty(entity.F_SeoKeywords))
            {
                entity.F_SeoKeywords = entity.F_Zhaiyao;
            }
            if (string.IsNullOrEmpty(entity.F_SeoDescription))
            {
                entity.F_SeoDescription = entity.F_Zhaiyao;
            }

            if (string.IsNullOrEmpty(keyValue))
            {
                    //此处需修改
                entity.Create();
                await service.Insert(entity);
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                    //此处需修改
                entity.Modify(keyValue); 
                await service.Update(entity);
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
        }

        public async Task DeleteForm(string keyValue)
        {
            await service.Delete(t => t.F_Id == keyValue);
            await CacheHelper.Remove(cacheKey + keyValue);
            await CacheHelper.Remove(cacheKey + "list");
        }
        #endregion

    }
}
