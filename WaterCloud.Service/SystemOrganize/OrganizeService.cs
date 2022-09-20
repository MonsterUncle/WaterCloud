/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Service.SystemOrganize
{
	public class OrganizeService : BaseService<OrganizeEntity>, IDenpendency
	{
		public OrganizeService(ISqlSugarClient context) : base(context)
		{
		}

		public async Task<List<OrganizeEntity>> GetList()
		{
			var query = repository.IQueryable();
			return await query.Where(a => a.F_DeleteMark == false).ToListAsync();
		}

		public async Task<List<OrganizeEntity>> GetLookList()
		{
			var query = repository.IQueryable().Where(a => a.F_DeleteMark == false);
			query = GetDataPrivilege("a", "", query);
			return await query.OrderBy(a => a.F_SortCode).ToListAsync();
		}

		public async Task<OrganizeEntity> GetLookForm(string keyValue)
		{
			var data = await repository.FindEntity(keyValue);
			return GetFieldsFilterData(data);
		}

		public async Task<OrganizeEntity> GetForm(string keyValue)
		{
			var data = await repository.FindEntity(keyValue);
			return data;
		}

		public async Task DeleteForm(string keyValue)
		{
			if (await repository.IQueryable(a => a.F_ParentId.Equals(keyValue)).AnyAsync())
			{
				throw new Exception("删除失败！操作的对象包含了下级数据。");
			}
			else
			{
				if (await repository.Db.Queryable<UserEntity>().Where(a => a.F_OrganizeId == keyValue).AnyAsync() || await repository.Db.Queryable<UserEntity>().Where(a => a.F_DepartmentId == keyValue).AnyAsync())
				{
					throw new Exception("组织使用中，无法删除");
				}
				await repository.Delete(a => a.F_Id == keyValue);
			}
		}

		public async Task SubmitForm(OrganizeEntity organizeEntity, string keyValue)
		{
			if (!string.IsNullOrEmpty(keyValue))
			{
				organizeEntity.Modify(keyValue);
				await repository.Update(organizeEntity);
			}
			else
			{
				organizeEntity.F_AllowDelete = false;
				organizeEntity.F_AllowEdit = false;
				organizeEntity.F_DeleteMark = false;
				organizeEntity.Create();
				await repository.Insert(organizeEntity);
			}
		}
	}
}