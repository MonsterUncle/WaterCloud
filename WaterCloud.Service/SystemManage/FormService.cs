using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Service.SystemManage
{
	/// <summary>
	/// 创 建：超级管理员
	/// 日 期：2020-07-08 14:33
	/// 描 述：表单设计服务类
	/// </summary>
	public class FormService : BaseService<FormEntity>, IDenpendency
	{
		public FormService(ISqlSugarClient context) : base(context)
		{
		}

		#region 获取数据

		public async Task<List<FormEntity>> GetList(string keyword = "")
		{
			var query = repository.IQueryable();
			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(a => a.F_Name.Contains(keyword) || a.F_Description.Contains(keyword));
			}
			return await query.Where(a => a.F_DeleteMark == false).OrderBy(a => a.F_Id, OrderByType.Desc).ToListAsync();
		}

		public async Task<List<FormEntity>> GetLookList(string ItemId = "", string keyword = "")
		{
			var query = GetQuery().Where(a => a.F_DeleteMark == false);
			if (!string.IsNullOrEmpty(ItemId))
			{
				query = query.Where(a => a.F_OrganizeId == ItemId || a.F_OrganizeId == null || a.F_OrganizeId == "");
			}
			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(a => a.F_Name.Contains(keyword) || a.F_Description.Contains(keyword));
			}
			query = GetDataPrivilege("a", "", query);
			return await query.Where(a => a.F_DeleteMark == false).OrderBy(a => a.F_Id, OrderByType.Desc).ToListAsync();
		}

		public async Task<List<FormEntity>> GetLookList(Pagination pagination, string keyword = "")
		{
			//获取数据权限
			var query = GetQuery().Where(a => a.F_DeleteMark == false);
			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(a => a.F_Name.Contains(keyword) || a.F_Description.Contains(keyword));
			}
			query = GetDataPrivilege("a", "", query);
			return await query.ToPageListAsync(pagination);
		}

		private ISugarQueryable<FormEntity> GetQuery()
		{
			var query = repository.Db.Queryable<FormEntity, OrganizeEntity>((a, b) => new JoinQueryInfos(
					JoinType.Left, a.F_OrganizeId == b.F_Id
				))
				.Select((a, b) => new FormEntity
				{
					F_Id = a.F_Id.SelectAll(),
					F_OrganizeName = b.F_FullName,
				}).MergeTable();
			return query;
		}

		public async Task<FormEntity> GetForm(string keyValue)
		{
			var data = await repository.FindEntity(keyValue);
			return data;
		}

		public async Task<FormEntity> GetLookForm(string keyValue)
		{
			var data = await repository.FindEntity(keyValue);
			return GetFieldsFilterData(data);
		}

		#endregion 获取数据

		#region 提交数据

		public async Task SubmitForm(FormEntity entity, string keyValue)
		{
			if (entity.F_FrmType != 1)
			{
				var temp = FormUtil.SetValue(entity.F_Content);
				entity.F_ContentData = string.Join(',', temp.ToArray());
				entity.F_Fields = temp.Count();
			}
			else
			{
				var temp = FormUtil.SetValueByWeb(entity.F_WebId);
				entity.F_ContentData = string.Join(',', temp.ToArray());
				entity.F_Fields = temp.Count();
			}
			if (string.IsNullOrEmpty(keyValue))
			{
				//此处需修改
				entity.F_DeleteMark = false;
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