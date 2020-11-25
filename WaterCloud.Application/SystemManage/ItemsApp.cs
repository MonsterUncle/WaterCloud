/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Entity.SystemManage;
using WaterCloud.Domain.IRepository.SystemManage;
using WaterCloud.Repository.SystemManage;
using System;
using System.Linq;
using System.Collections.Generic;

using WaterCloud.Code;

namespace WaterCloud.Application.SystemManage
{
    public class ItemsApp
    {
        private IItemsRepository service = new ItemsRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private ICache redisCache = CacheFactory.CaChe();
        private string cacheKey = "watercloud_itemsdata_";// 字典分类

        public List<ItemsEntity> GetList()
        {
            var cachedata = service.CheckCacheList(cacheKey + "list", CacheId.itemsData);
            return cachedata.OrderBy(t => t.F_SortCode).ToList();
        }
        public ItemsEntity GetForm(string keyValue)
        {
            var cachedata = service.CheckCache(cacheKey, keyValue, CacheId.itemsData);
            return cachedata;
        }
        public void DeleteForm(string keyValue)
        {
            if (service.IQueryable(t => t.F_ParentId.Equals(keyValue)).Count() > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                service.Delete(t => t.F_Id == keyValue);
                redisCache.Remove(cacheKey + keyValue, CacheId.itemsData);
                redisCache.Remove(cacheKey + "list", CacheId.itemsData);
            }
        }
        public void SubmitForm(ItemsEntity itemsEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                itemsEntity.Modify(keyValue);
                service.Update(itemsEntity);
                redisCache.Remove(cacheKey + keyValue, CacheId.itemsData);
                redisCache.Remove(cacheKey + "list", CacheId.itemsData);
            }
            else
            {
                itemsEntity.Create();
                service.Insert(itemsEntity);
                redisCache.Remove(cacheKey + "list", CacheId.itemsData);
            }
        }
    }
}
