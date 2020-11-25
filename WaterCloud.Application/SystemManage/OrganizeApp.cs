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
using System.Collections.Generic;
using System.Linq;
using WaterCloud.Code;

namespace WaterCloud.Application.SystemManage
{
    public class OrganizeApp
    {
        private IUserRepository userservice = new UserRepository();
        private IOrganizeRepository service = new OrganizeRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private ICache redisCache = CacheFactory.CaChe();
        private string cacheKey = "watercloud_organizedata_";

        public List<OrganizeEntity> GetList()
        {
            var cachedata = service.CheckCacheList(cacheKey + "list", CacheId.company);
            return cachedata;
        }
        public OrganizeEntity GetForm(string keyValue)
        {
            var cachedata = service.CheckCache(cacheKey, keyValue, CacheId.company);
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
                if (userservice.IQueryable(a=>a.F_OrganizeId==keyValue).Count()>0|| userservice.IQueryable(a => a.F_DepartmentId == keyValue).Count()>0)
                {
                    throw new Exception("组织使用中，无法删除");
                }
                service.Delete(t => t.F_Id == keyValue);
                redisCache.Remove(cacheKey + keyValue, CacheId.company);
                redisCache.Remove(cacheKey + "list", CacheId.company);
            }
        }
        public void SubmitForm(OrganizeEntity organizeEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                organizeEntity.Modify(keyValue);
                service.Update(organizeEntity);
                redisCache.Remove(cacheKey + keyValue, CacheId.company);
                redisCache.Remove(cacheKey + "list", CacheId.company);
            }
            else
            {
                organizeEntity.Create();
                service.Insert(organizeEntity);
                redisCache.Remove(cacheKey + "list", CacheId.company);
            }
        }
    }
}
