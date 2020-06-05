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
using System.Linq;

namespace WaterCloud.Service.SystemManage
{
    public class ModuleService : DataFilterService<ModuleEntity>, IDenpendency
    {
        private IModuleRepository service = new ModuleRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_moduleldata_";
        private string quickcacheKey = "watercloud_quickmoduledata_";
        private string initcacheKey = "watercloud_init_";
        private string modulebuttoncacheKey = "watercloud_modulebuttondata_";
        private string modulefieldscacheKey = "watercloud_modulefieldsdata_";
        private string authorizecacheKey = "watercloud_authorizeurldata_";// +权限
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];

        public async Task<List<ModuleEntity>> GetList()
        {
            var cachedata =await service.CheckCacheList(cacheKey + "list");
            return cachedata.Where(a=>a.F_DeleteMark==false).OrderBy(t => t.F_SortCode).ToList();
        }
        public async Task<List<ModuleEntity>> GetLookList()
        {
            var list = new List<ModuleEntity>();
            if (!CheckDataPrivilege(className.Substring(0, className.Length - 7)))
            {
                list = await service.CheckCacheList(cacheKey + "list");
            }
            else
            {
                var forms = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
                list = forms.ToList();
            }
            return list.Where(a => a.F_DeleteMark == false).OrderBy(t => t.F_SortCode).ToList();
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
                await RedisHelper.DelAsync(initcacheKey + "modulefields_list");
                await RedisHelper.DelAsync(authorizecacheKey + "list");
                await RedisHelper.DelAsync(authorizecacheKey + "authorize_list");
                await RedisHelper.DelAsync(modulebuttoncacheKey + "list");
                await RedisHelper.DelAsync(modulefieldscacheKey + "list");
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
            await RedisHelper.DelAsync(initcacheKey + "modulefields_list");
            await RedisHelper.DelAsync(authorizecacheKey + "list");
            await RedisHelper.DelAsync(authorizecacheKey + "authorize_list");
            await RedisHelper.DelAsync(modulebuttoncacheKey + "list");
            await RedisHelper.DelAsync(modulefieldscacheKey + "list");
        }
    }
}
