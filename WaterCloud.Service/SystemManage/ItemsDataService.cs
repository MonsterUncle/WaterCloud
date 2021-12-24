/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Domain.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCloud.Code;
using Chloe;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemManage
{
    public class ItemsDataService : DataFilterService<ItemsDetailEntity>,IDenpendency
    {
        /// <summary>
        /// 缓存操作类
        /// </summary>
        public ItemsDataService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        //获取类名
        
        public async Task<List<ItemsDetailEntity>> GetList(string itemId = "", string keyword = "")
        {
            var list = repository.IQueryable();
            if (!string.IsNullOrEmpty(itemId))
            {
                list = list.Where(t => t.F_ItemId == itemId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(t => t.F_ItemName.Contains(keyword) || t.F_ItemCode.Contains(keyword));
            }
            return await list.Where(a => a.F_DeleteMark == false).OrderBy(t => t.F_SortCode).ToListAsync();
        }
        public async Task<List<ItemsDetailEntity>> GetLookList(string itemId = "", string keyword = "")
        {
            var query = repository.IQueryable().Where(a => a.F_DeleteMark == false);
            if (!string.IsNullOrEmpty(itemId))
            {
                query = query.Where(t => t.F_ItemId == itemId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(t => t.F_ItemName.Contains(keyword) || t.F_ItemCode.Contains(keyword));
            }
            query = GetDataPrivilege("u","", query);
            return await query.OrderBy(t => t.F_SortCode).ToListAsync();
        }
        public async Task<List<ItemsDetailEntity>> GetItemList(string enCode)
        {
            var itemdata = unitwork.IQueryable<ItemsEntity>().ToList();
            var item = itemdata.Find(a => a.F_EnCode == enCode);
            var data = repository.IQueryable();
            return await data.Where(a => a.F_DeleteMark == false && a.F_EnabledMark == true && a.F_ItemId == item.F_Id).OrderBy(a => a.F_SortCode).ToListAsync();
        }
        public async Task<ItemsDetailEntity> GetLookForm(string keyValue)
        {
            var data =await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }
        public async Task<ItemsDetailEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        public async Task DeleteForm(string keyValue)
        {
            await repository.Delete(t => t.F_Id == keyValue);
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
