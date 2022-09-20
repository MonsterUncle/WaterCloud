using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.ContentManage;

namespace WaterCloud.Service.ContentManage
{
	/// <summary>
	/// 创 建：超级管理员
	/// 日 期：2020-06-09 19:42
	/// 描 述：新闻管理服务类
	/// </summary>
	public class ArticleNewsService : BaseService<ArticleNewsEntity>, IDenpendency
	{
		public ArticleNewsService(ISqlSugarClient context) : base(context)

		{
		}

		#region 获取数据

		public async Task<List<ArticleNewsEntity>> GetList(string keyword = "")
		{
			var query = repository.IQueryable();
			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(a => a.F_Title.Contains(keyword) || a.F_Tags.Contains(keyword));
			}
			return await query.Where(a => a.F_DeleteMark == false).OrderBy(a => a.F_Id, OrderByType.Desc).ToListAsync();
		}

		public async Task<List<ArticleNewsEntity>> GetLookList(SoulPage<ArticleNewsEntity> pagination, string keyword = "", string CategoryId = "")
		{
			//反格式化显示只能用"等于"，其他不支持
			Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
			Dictionary<string, string> enabledTemp = new Dictionary<string, string>();
			enabledTemp.Add("1", "有效");
			enabledTemp.Add("0", "无效");
			dic.Add("F_EnabledMark", enabledTemp);
			Dictionary<string, string> isTrue = new Dictionary<string, string>();
			isTrue.Add("1", "是");
			isTrue.Add("0", "否");
			dic.Add("F_IsTop", isTrue);
			dic.Add("F_IsHot", isTrue);
			pagination = ChangeSoulData(dic, pagination);
			//获取新闻列表
			var query = repository.Db.Queryable<ArticleNewsEntity, ArticleCategoryEntity>((a, b) => new JoinQueryInfos(
				JoinType.Left, a.F_CategoryId == b.F_Id && b.F_EnabledMark == true
				))
			.Select((a, b) => new ArticleNewsEntity
			{
				F_Id = a.F_Id.SelectAll(),
				F_CategoryName = b.F_FullName,
			}).MergeTable();
			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(a => a.F_Title.Contains(keyword));
			}
			if (!string.IsNullOrEmpty(CategoryId))
			{
				query = query.Where(a => a.F_CategoryId.Contains(CategoryId));
			}
			query = query.Where(a => a.F_DeleteMark == false);
			//权限过滤
			query = GetDataPrivilege<ArticleNewsEntity>("a", "", query);
			return await query.ToPageListAsync(pagination);
		}

		/// <summary>
		/// 获取新闻详情
		/// </summary>
		/// <param name="keyValue">主键值</param>
		/// <returns></returns>
		public async Task<ArticleNewsEntity> GetForm(string keyValue)
		{
			var query = repository.Db.Queryable<ArticleNewsEntity, ArticleCategoryEntity>((a, b) => new JoinQueryInfos(
				JoinType.Left, a.F_CategoryId == b.F_Id && b.F_EnabledMark == true
				))
			.Select((a, b) => new ArticleNewsEntity
			{
				F_Id = a.F_Id.SelectAll(),
				F_CategoryName = b.F_FullName,
			}).MergeTable();
			if (!string.IsNullOrEmpty(keyValue))
			{
				query = query.Where(a => a.F_Id == keyValue);
			}
			//字段权限处理
			return GetFieldsFilterData(await query.FirstAsync());
		}

		#endregion 获取数据

		#region 提交数据

		public async Task SubmitForm(ArticleNewsEntity entity, string keyValue)
		{
			if (string.IsNullOrEmpty(entity.F_Zhaiyao))
			{
				entity.F_Zhaiyao = TextHelper.GetSubString(WebHelper.NoHtml(entity.F_Description), 255);
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
				entity.F_DeleteMark = false;
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
			await repository.Delete(a => ids.Contains(a.F_Id));
		}

		#endregion 提交数据
	}
}