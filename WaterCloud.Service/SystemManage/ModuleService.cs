/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Domain.SystemManage;
using WaterCloud.Repository.SystemManage;
using System;
using System.Collections.Generic;

namespace WaterCloud.Service.SystemManage
{
    public class ModuleService: IDenpendency
    {
        private IModuleRepository service = new ModuleRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_moduleldata_";
        private string quickcacheKey = "watercloud_quickmoduledata_";
        private string initcacheKey = "watercloud_init_";

        public List<ModuleEntity> GetList()
        {
            var cachedata = service.CheckCacheList(cacheKey + "list");
            return service.IQueryable().OrderBy(t => t.F_SortCode).ToList();
        }
        public ModuleEntity GetForm(string keyValue)
        {
            var cachedata = service.CheckCache(cacheKey, keyValue);
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
                RedisHelper.Del(cacheKey + keyValue);
                RedisHelper.Del(cacheKey + "list");
                RedisHelper.Del(quickcacheKey + "list");
                RedisHelper.Del(initcacheKey + "list");
                RedisHelper.Del(initcacheKey + "modulebutton_list");
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
                RedisHelper.Del(cacheKey + keyValue);
                RedisHelper.Del(cacheKey + "list");
            }
            else
            {
                moduleEntity.Create();
                service.Insert(moduleEntity);
                RedisHelper.Del(cacheKey + "list");
            }
            RedisHelper.Del(quickcacheKey + "list");
            RedisHelper.Del(initcacheKey + "list");
            RedisHelper.Del(initcacheKey + "modulebutton_list");

        }
    }
}
