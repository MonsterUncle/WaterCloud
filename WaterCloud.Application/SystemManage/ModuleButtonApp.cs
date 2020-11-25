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
    public class ModuleButtonApp
    {
        private IModuleButtonRepository service = new ModuleButtonRepository();
        private IModuleRepository moduleservice = new ModuleRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private ICache redisCache = CacheFactory.CaChe();
        private string cacheKey = "watercloud_modulebuttondata_";
        private string initcacheKey = "watercloud_init_";
        private string authorizecacheKey = "watercloud_authorizeurldata_";// +权限


        public List<ModuleButtonEntity> GetList(string moduleId = "")
        {
            var cachedata = service.CheckCacheList(cacheKey + "list", CacheId.module);
            if (!string.IsNullOrEmpty(moduleId))
            {
                cachedata = cachedata.Where(t => t.F_ModuleId == moduleId).ToList();
            }
            return cachedata.OrderBy(t => t.F_SortCode).ToList();
        }
        public ModuleButtonEntity GetForm(string keyValue)
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
            }
            redisCache.Remove(initcacheKey + "modulebutton_" + "list", CacheId.module);
            redisCache.Remove(authorizecacheKey + "list", CacheId.authorize);
            redisCache.Remove(authorizecacheKey + "authorize_list", CacheId.authorize);
        }

        public List<ModuleButtonEntity> GetListByRole(string roleid)
        {
            return service.GetListByRole(roleid);
        }

        public void SubmitForm(ModuleButtonEntity moduleButtonEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                moduleButtonEntity.Modify(keyValue);
                service.Update(moduleButtonEntity);
                redisCache.Remove(cacheKey + keyValue, CacheId.module);
                redisCache.Remove(cacheKey + "list", CacheId.module);
            }
            else
            {
                var module = moduleservice.FindEntity(a => a.F_Id == moduleButtonEntity.F_ModuleId);
                if (string.IsNullOrEmpty(module.F_UrlAddress) || module.F_Target != "iframe")
                {
                    throw new Exception("菜单不能创建按钮");
                }
                moduleButtonEntity.Create();
                service.Insert(moduleButtonEntity);
                redisCache.Remove(cacheKey + "list", CacheId.module);
            }
            redisCache.Remove(initcacheKey + "modulebutton_" + "list", CacheId.module);
            redisCache.Remove(authorizecacheKey + "list", CacheId.authorize);
            redisCache.Remove(authorizecacheKey + "authorize_list", CacheId.authorize);
        }
        public void SubmitCloneButton(string moduleId, string Ids)
        {
            string[] ArrayId = Ids.Split(',');
            var data = this.GetList();
            List<ModuleButtonEntity> entitys = new List<ModuleButtonEntity>();
            var module = moduleservice.FindEntity(a => a.F_Id == moduleId);
            if (string.IsNullOrEmpty(module.F_UrlAddress) || module.F_Target != "iframe")
            {
                throw new Exception("菜单不能创建按钮");
            }
            foreach (string item in ArrayId)
            {
                ModuleButtonEntity moduleButtonEntity = data.Find(t => t.F_Id == item);
                moduleButtonEntity.F_Id = Utils.GuId();
                moduleButtonEntity.F_ModuleId = moduleId;
                entitys.Add(moduleButtonEntity);
            }
            service.SubmitCloneButton(entitys);
            redisCache.Remove(cacheKey + "list", CacheId.module);
            redisCache.Remove(authorizecacheKey + "list", CacheId.authorize);
            redisCache.Remove(authorizecacheKey + "authorize_list", CacheId.authorize);
        }

        public List<ModuleButtonEntity> GetListNew(string moduleId = "")
        {
            return service.GetListNew(moduleId);
        }
    }
}
