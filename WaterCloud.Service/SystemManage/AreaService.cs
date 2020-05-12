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
using System.Linq;
using System.Threading.Tasks;

namespace WaterCloud.Service.SystemManage
{
    public class AreaService: IDenpendency
    {
        private IAreaRepository service = new AreaRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private string cacheKey = "watercloud_areadata_";// 区域

        public async Task<List<AreaEntity>> GetList()
        {
            var cachedata =await service.CheckCacheList(cacheKey + "list");
            cachedata = cachedata.Where(t => t.F_DeleteMark == false && t.F_EnabledMark == true && t.F_Layers == 1).ToList();
            return cachedata.OrderBy(t => t.F_SortCode).ToList();
        }
        public async Task<AreaEntity> GetForm(string keyValue)
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
               await service.Delete(t => t.F_Id == keyValue);
            }
            await RedisHelper.DelAsync(cacheKey + keyValue);
            await RedisHelper.DelAsync(cacheKey + "list");
        }
        public async Task SubmitForm(AreaEntity mEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                mEntity.Modify(keyValue);
                await service.Update(mEntity);
                await RedisHelper.DelAsync(cacheKey + keyValue);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
            else
            {
                mEntity.Create();
                await service.Insert(mEntity);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
        }
    }
}
