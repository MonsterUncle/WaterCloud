using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.Domain.ContentManage;
using WaterCloud.DataBase;
using SqlSugar;

namespace WaterCloud.Service.ContentManage
{
	/// <summary>
	/// 创 建：超级管理员
	/// 日 期：2020-06-09 19:42
	/// 描 述：新闻管理服务类
	/// </summary>
	public class ArticleNewsService : DataFilterService<ArticleNewsEntity>, IDenpendency
    {
        public ArticleNewsService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        
        #region 获取数据
        public async Task<List<ArticleNewsEntity>> GetList(string keyword = "")
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.Title.Contains(keyword) || a.Tags.Contains(keyword));
            }
            return await query.Where(a => a.DeleteMark == false).OrderBy(a => a.Id,OrderByType.Desc).ToListAsync();
        }

        public async Task<List<ArticleNewsEntity>> GetLookList(SoulPage<ArticleNewsEntity> pagination, string keyword = "", string CategoryId="")
        {
            //反格式化显示只能用"等于"，其他不支持
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> enabledTemp = new Dictionary<string, string>();
            enabledTemp.Add("1", "有效");
            enabledTemp.Add("0", "无效");
            dic.Add("EnabledMark", enabledTemp);
            Dictionary<string, string> isTrue = new Dictionary<string, string>();
            isTrue.Add("1", "是");
            isTrue.Add("0", "否");
            dic.Add("IsTop", isTrue);
            dic.Add("IsHot", isTrue);
            pagination = ChangeSoulData(dic, pagination);
            //获取新闻列表
            var query = repository.Db.Queryable<ArticleNewsEntity, ArticleCategoryEntity>((a,b) => new JoinQueryInfos(
                JoinType.Left,a.CategoryId==b.Id && b.EnabledMark == true
                ))
            .Select((a, b) => new ArticleNewsEntity
            {
                Id = a.Id.SelectAll(),
                CategoryName = b.FullName,
            }).MergeTable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.Title.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(CategoryId))
            {
                query = query.Where(a => a.CategoryId.Contains(CategoryId));
            }
            query = query.Where(a => a.DeleteMark == false);
            //权限过滤
            query = GetDataPrivilege<ArticleNewsEntity>("a", "",query);           
            return await repository.OrderList(query, pagination);
        }
        /// <summary>
        /// 获取新闻详情
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public async Task<ArticleNewsEntity> GetForm(string keyValue)
        {
            var query = repository.Db.Queryable<ArticleNewsEntity, ArticleCategoryEntity>((a, b) => new JoinQueryInfos(
                JoinType.Left, a.CategoryId == b.Id && b.EnabledMark == true
                ))
            .Select((a, b) => new ArticleNewsEntity
            {
                Id = a.Id.SelectAll(),
                CategoryName = b.FullName,
            }).MergeTable();
            if (!string.IsNullOrEmpty(keyValue))
            {
                query = query.Where(a => a.Id == keyValue);
            }
            //字段权限处理
            return GetFieldsFilterData(await query.FirstAsync());
        }
        #endregion

        #region 提交数据
        public async Task SubmitForm(ArticleNewsEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(entity.Zhaiyao))
            {
                entity.Zhaiyao = TextHelper.GetSubString(WebHelper.NoHtml(entity.Description),255);
            }
            if (string.IsNullOrEmpty(entity.SeoTitle))
            {
                entity.SeoTitle = entity.Title;
            }
            if (string.IsNullOrEmpty(entity.SeoKeywords))
            {
                entity.SeoKeywords = entity.Zhaiyao;
            }
            if (string.IsNullOrEmpty(entity.SeoDescription))
            {
                entity.SeoDescription = entity.Zhaiyao;
            }

            if (string.IsNullOrEmpty(keyValue))
            {
                entity.DeleteMark = false;
                //此处需修改
                entity.Create();
                await repository.Insert(entity);
            }
            else
            {
                    //此处需修改
                entity.Modify(keyValue); 
                await repository.Update(entity);
            }
        }

        public async Task DeleteForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            await repository.Delete(a => ids.Contains(a.Id));
        }
        #endregion

    }
}
