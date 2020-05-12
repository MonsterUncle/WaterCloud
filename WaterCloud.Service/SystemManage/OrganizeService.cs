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
    public class OrganizeService: IDenpendency
    {
        private IUserRepository userservice = new UserRepository();
        private IOrganizeRepository service = new OrganizeRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_organizedata_";

        public async Task<List<OrganizeEntity>> GetList()
        {
            var cachedata =await service.CheckCacheList(cacheKey + "list");
            return cachedata;
        }
        public async Task<OrganizeEntity> GetForm(string keyValue)
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
                if (userservice.IQueryable(a=>a.F_OrganizeId==keyValue).Count()>0|| userservice.IQueryable(a => a.F_DepartmentId == keyValue).Count()>0)
                {
                    throw new Exception("组织使用中，无法删除");
                }
                await service.Delete(t => t.F_Id == keyValue);
                await RedisHelper.DelAsync(cacheKey + keyValue);
                await  RedisHelper.DelAsync(cacheKey + "list");
            }
        }
        public async Task SubmitForm(OrganizeEntity organizeEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                organizeEntity.Modify(keyValue);
                await service.Update(organizeEntity);
                await RedisHelper.DelAsync(cacheKey + keyValue);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
            else
            {
                organizeEntity.Create();
                await service.Insert(organizeEntity);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
        }
    }
}
