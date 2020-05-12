/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Domain.SystemManage;
using WaterCloud.Repository.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaterCloud.Service.SystemManage
{
    public class ItemsDetailService: IDenpendency
    {
        private IItemsDetailRepository service = new ItemsDetailRepository();
        private IItemsRepository itemservice = new ItemsRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_itemdetaildata_";
        private string itemcacheKey = "watercloud_itemdata_";

        public async Task<List<ItemsDetailEntity>> GetList(string itemId = "", string keyword = "")
        {
            var cachedata =await service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(itemId))
            {
                cachedata = cachedata.Where(t => t.F_ItemId == itemId).ToList();
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_ItemName.Contains(keyword) || t.F_ItemCode.Contains(keyword)).ToList();
            }
            return cachedata.OrderBy(t => t.F_SortCode).ToList();
        }
        public async Task<List<ItemsDetailEntity>> GetItemList(string enCode)
        {
            var itemcachedata =await itemservice.CheckCacheList(itemcacheKey + "list");
            var item = itemcachedata.Find(a => a.F_EnCode == enCode);
            var cachedata =await service.CheckCacheList(cacheKey + "list");
            cachedata = cachedata.Where(a => a.F_DeleteMark == false && a.F_EnabledMark == true && a.F_ItemId == item.F_Id).OrderBy(a => a.F_SortCode).ToList();
            return cachedata;
        }
        public async Task<ItemsDetailEntity> GetForm(string keyValue)
        {
            var cachedata =await service.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        public async Task DeleteForm(string keyValue)
        {
            await service.Delete(t => t.F_Id == keyValue);
            await RedisHelper.DelAsync(cacheKey + keyValue);
            await RedisHelper.DelAsync(cacheKey + "list");
        }
        public async Task SubmitForm(ItemsDetailEntity itemsDetailEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                itemsDetailEntity.Modify(keyValue);
                await service.Update(itemsDetailEntity);
                await RedisHelper.DelAsync(cacheKey + keyValue);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
            else
            {
                itemsDetailEntity.Create();
                await service.Insert(itemsDetailEntity);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
        }
    }
}
