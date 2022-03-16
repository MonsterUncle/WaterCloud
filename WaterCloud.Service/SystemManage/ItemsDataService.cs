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
using SqlSugar;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemManage
{
    public class ItemsDataService : DataFilterService<ItemsDetailEntity>,IDenpendency
    {
        public ItemsTypeService itemApp { get; set; }
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
                list = list.Where(a => a.ItemId == itemId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(a => a.ItemName.Contains(keyword) || a.ItemCode.Contains(keyword));
            }
            return await list.Where(a => a.DeleteMark == false).OrderBy(a => a.SortCode).ToListAsync();
        }
        public async Task<List<ItemsDetailEntity>> GetLookList(string itemId = "", string keyword = "")
        {
            var query = repository.IQueryable().Where(a => a.DeleteMark == false);
            if (!string.IsNullOrEmpty(itemId))
            {
                query = query.Where(a => a.ItemId == itemId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.ItemName.Contains(keyword) || a.ItemCode.Contains(keyword));
            }
            query = GetDataPrivilege("a","", query);
            return await query.OrderBy(a => a.SortCode).ToListAsync();
        }
        public async Task<List<ItemsDetailEntity>> GetItemList(string enCode)
        {
            var itemcachedata = await itemApp.GetList();
            var item = itemcachedata.Find(a => a.EnCode == enCode);
            var data = repository.IQueryable();
            return data.Where(a => a.DeleteMark == false && a.EnabledMark == true && a.ItemId == item.Id).OrderBy(a => a.SortCode).ToList();
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
            await repository.Delete(a => a.Id == keyValue);
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
                itemsDetailEntity.DeleteMark = false;
                itemsDetailEntity.Create();
                await repository.Insert(itemsDetailEntity);
            }
        }
    }
}
