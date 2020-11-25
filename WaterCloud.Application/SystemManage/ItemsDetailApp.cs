/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Entity.SystemManage;
using WaterCloud.Domain.IRepository.SystemManage;
using WaterCloud.Repository.SystemManage;
using System.Collections.Generic;
using System.Linq;

namespace WaterCloud.Application.SystemManage
{
    public class ItemsDetailApp
    {
        private IItemsDetailRepository service = new ItemsDetailRepository();
        private IItemsRepository itemservice = new ItemsRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private ICache redisCache = CacheFactory.CaChe();
        private string cacheKey = "watercloud_itemdetaildata_";
        private string itemcacheKey = "watercloud_itemdata_";

        public List<ItemsDetailEntity> GetList(string itemId = "", string keyword = "")
        {
            var cachedata = service.CheckCacheList(cacheKey + "list", CacheId.itemsData);
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
        public List<ItemsDetailEntity> GetItemList(string enCode)
        {
            var itemcachedata = itemservice.CheckCacheList(itemcacheKey + "list", CacheId.itemsData);
            var item = itemcachedata.Find(a => a.F_EnCode == enCode);
            var cachedata = service.CheckCacheList(cacheKey + "list", CacheId.itemsData);
            cachedata = cachedata.Where(a => a.F_DeleteMark == false && a.F_EnabledMark == true && a.F_ItemId == item.F_Id).OrderBy(a => a.F_SortCode).ToList();
            return cachedata;
        }
        public ItemsDetailEntity GetForm(string keyValue)
        {
            var cachedata = service.CheckCache(cacheKey, keyValue, CacheId.itemsData);
            return cachedata;
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
            redisCache.Remove(cacheKey + keyValue, CacheId.itemsData);
            redisCache.Remove(cacheKey + "list", CacheId.itemsData);
        }
        public void SubmitForm(ItemsDetailEntity itemsDetailEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                itemsDetailEntity.Modify(keyValue);
                service.Update(itemsDetailEntity);
                redisCache.Remove(cacheKey + keyValue, CacheId.itemsData);
                redisCache.Remove(cacheKey + "list", CacheId.itemsData);
            }
            else
            {
                itemsDetailEntity.Create();
                service.Insert(itemsDetailEntity);
                redisCache.Remove(cacheKey + "list", CacheId.itemsData);
            }
        }
    }
}
