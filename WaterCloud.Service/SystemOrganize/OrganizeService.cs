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

		public async Task<List<OrganizeExtend>> GetList()
		{
			var query = GetQuery();
			return await query.Where(a => a.F_DeleteMark == false).ToListAsync();
		}

		public async Task<List<OrganizeExtend>> GetLookList()
		{
			var query = GetQuery().Where(a => a.F_DeleteMark == false);
			query = GetDataPrivilege("a", "", query);
			return await query.OrderBy(a => a.F_SortCode).ToListAsync();
		}

		public async Task<OrganizeExtend> GetLookForm(string keyValue)
		{
			var data = await GetQuery().FirstAsync(a => a.F_Id == keyValue);
            return GetFieldsFilterData(data);
		}

		public async Task<OrganizeExtend> GetForm(string keyValue)
		{
			var data = await GetQuery().FirstAsync(a => a.F_Id == keyValue);
			return data;
		}


        private ISugarQueryable<OrganizeExtend> GetQuery()
        {
            var query = repository.Db.Queryable<OrganizeEntity, UserEntity>((a, b) => new JoinQueryInfos(
                JoinType.Left, a.F_ManagerId == b.F_Id
                )).Where(a => a.F_DeleteMark == false)
                .Select((a, b) => new OrganizeExtend
                {
                    F_Id = a.F_Id.SelectAll(),
                    F_ManagerName = b.F_RealName
                }).MergeTable();
            return query;
        }

        public async Task DeleteForm(string keyValue)
		{
			if (await repository.IQueryable(a => a.F_ParentId.Equals(keyValue)).AnyAsync())
			{
				throw new Exception("删除失败！操作的对象包含了下级数据。");
			}
			else
			{
				if (await repository.Db.Queryable<UserEntity>().Where(a => a.F_CompanyId == keyValue).AnyAsync() || await repository.Db.Queryable<UserEntity>().Where(a => a.F_OrganizeId == keyValue).AnyAsync())
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