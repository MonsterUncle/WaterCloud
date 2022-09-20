/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/

using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.Service.SystemManage
{
	public class ItemsDataService : BaseService<ItemsDetailEntity>, IDenpendency
	{
		public ItemsTypeService itemApp { get; set; }

		/// <summary>
		/// 缓存操作类
		/// </summary>
		public ItemsDataService(ISqlSugarClient context) : base(context)
		{
		}

		//获取类名

		public async Task<List<ItemsDetailEntity>> GetList(string itemId = "", string keyword = "")
		{
			var list = repository.IQueryable();
			if (!string.IsNullOrEmpty(itemId))
			{
				list = list.Where(a => a.F_ItemId == itemId);
			}
			if (!string.IsNullOrEmpty(keyword))
			{
				list = list.Where(a => a.F_ItemName.Contains(keyword) || a.F_ItemCode.Contains(keyword));
			}
			return await list.Where(a => a.F_DeleteMark == false).OrderBy(a => a.F_SortCode).ToListAsync();
		}

		public async Task<List<ItemsDetailEntity>> GetLookList(string itemId = "", string keyword = "")
		{
			var query = repository.IQueryable().Where(a => a.F_DeleteMark == false);
			if (!string.IsNullOrEmpty(itemId))
			{
				query = query.Where(a => a.F_ItemId == itemId);
			}
			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(a => a.F_ItemName.Contains(keyword) || a.F_ItemCode.Contains(keyword));
			}
			query = GetDataPrivilege("a", "", query);
			return await query.OrderBy(a => a.F_SortCode).ToListAsync();
		}

		public async Task<List<ItemsDetailEntity>> GetItemList(string enCode)
		{
			var itemcachedata = await itemApp.GetList();
			var item = itemcachedata.Find(a => a.F_EnCode == enCode);
			var data = repository.IQueryable();
			return data.Where(a => a.F_DeleteMark == false && a.F_EnabledMark == true && a.F_ItemId == item.F_Id).OrderBy(a => a.F_SortCode).ToList();
		}

		public async Task<ItemsDetailEntity> GetLookForm(string keyValue)
		{
			var data = await repository.FindEntity(keyValue);
			return GetFieldsFilterData(data);
		}

		public async Task<ItemsDetailEntity> GetForm(string keyValue)
		{
			var data = await repository.FindEntity(keyValue);
			return data;
		}

		public async Task DeleteForm(string keyValue)
		{
			await repository.Delete(a => a.F_Id == keyValue);
		}

		public async Task SubmitForm(ItemsDetailEntity itemsDetailEntity, string keyValue)
		{
			if (!string.IsNullOrEmpty(keyValue))
			{
				itemsDetailEntity.Modify(keyValue);
				await repository.Update(itemsDetailEntity);
			}
			else
			{
				itemsDetailEntity.F_DeleteMark = false;
				itemsDetailEntity.Create();
				await repository.Insert(itemsDetailEntity);
			}
		}
	}
}