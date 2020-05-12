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
using System.Threading.Tasks;

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
        private string modulebuttoncacheKey = "watercloud_modulebuttondata_";

        public async Task<List<ModuleEntity>> GetList()
        {
            var cachedata =await service.CheckCacheList(cacheKey + "list");
            return service.IQueryable().OrderBy(t => t.F_SortCode).ToList();
        }
        public async Task<ModuleEntity> GetForm(string keyValue)
        {
            var cachedata =await service.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        public async Task DeleteForm(string keyValue)
        {
            if (service.IQueryable(t => t.F_ParentId.Equals(keyValue)).Count() > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                await service.DeleteForm(keyValue);
                await RedisHelper.DelAsync(cacheKey + keyValue);
                await RedisHelper.DelAsync(cacheKey + "list");
                await RedisHelper.DelAsync(quickcacheKey + "list");
                await RedisHelper.DelAsync(initcacheKey + "list");
                await RedisHelper.DelAsync(initcacheKey + "modulebutton_list");
                await RedisHelper.DelAsync(modulebuttoncacheKey + "list");
            }
        }

        public async Task<List<ModuleEntity>> GetListByRole(string roleid)
        {
            return await service.GetListByRole(roleid);
        }

        public async Task SubmitForm(ModuleEntity moduleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                moduleEntity.Modify(keyValue);
                await service.Update(moduleEntity);
                await RedisHelper.DelAsync(cacheKey + keyValue);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
            else
            {
                moduleEntity.Create();
                await service.Insert(moduleEntity);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
            await RedisHelper.DelAsync(quickcacheKey + "list");
            await RedisHelper.DelAsync(initcacheKey + "list");
            await RedisHelper.DelAsync(initcacheKey + "modulebutton_list");
            await RedisHelper.DelAsync(modulebuttoncacheKey + "list");
        }
    }
}
