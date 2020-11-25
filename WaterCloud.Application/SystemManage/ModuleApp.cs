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
using System;
using System.Collections.Generic;
using System.Linq;

namespace WaterCloud.Application.SystemManage
{
    public class ModuleApp
    {
        private IModuleRepository service = new ModuleRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private ICache redisCache = CacheFactory.CaChe();
        private string cacheKey = "watercloud_moduleldata_";
        private string quickcacheKey = "watercloud_quickmoduledata_";
        private string initcacheKey = "watercloud_init_";
        private string authorizecacheKey = "watercloud_authorizeurldata_";// +权限

        public List<ModuleEntity> GetList()
        {
            var cachedata = service.CheckCacheList(cacheKey + "list", CacheId.module);
            return service.IQueryable().OrderBy(t => t.F_SortCode).ToList();
        }
        public ModuleEntity GetForm(string keyValue)
        {
            var cachedata = service.CheckCache(cacheKey, keyValue, CacheId.module);
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
                redisCache.Remove(cacheKey + keyValue, CacheId.module);
                redisCache.Remove(cacheKey + "list", CacheId.module);
                redisCache.Remove(quickcacheKey + "list", CacheId.module);
                redisCache.Remove(initcacheKey + "list", CacheId.module);
                redisCache.Remove(initcacheKey + "modulebutton_list", CacheId.module);
                redisCache.Remove(authorizecacheKey + "list", CacheId.authorize);
                redisCache.Remove(authorizecacheKey + "authorize_list", CacheId.authorize);
            }
        }

        public List<ModuleEntity> GetListByRole(string roleid)
        {
            return service.GetListByRole(roleid);
        }

        public void SubmitForm(ModuleEntity moduleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                moduleEntity.Modify(keyValue);
                service.Update(moduleEntity);
                redisCache.Remove(cacheKey + keyValue, CacheId.module);
                redisCache.Remove(cacheKey + "list", CacheId.module);
            }
            else
            {
                moduleEntity.Create();
                service.Insert(moduleEntity);
                redisCache.Remove(cacheKey + "list", CacheId.module);
            }
            redisCache.Remove(quickcacheKey + "list", CacheId.module);
            redisCache.Remove(initcacheKey + "list", CacheId.module);
            redisCache.Remove(initcacheKey + "modulebutton_list", CacheId.module);
            redisCache.Remove(authorizecacheKey + "list", CacheId.authorize);
            redisCache.Remove(authorizecacheKey + "authorize_list", CacheId.authorize);

        }
    }
}
