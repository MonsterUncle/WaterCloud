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

namespace WaterCloud.Service.SystemManage
{
    public class AreaService: IDenpendency
    {
        private IAreaRepository service = new AreaRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private string cacheKey = "watercloud_areadata_";// 区域

        public List<AreaEntity> GetList()
        {
            var cachedata = service.CheckCacheList(cacheKey + "list");
            cachedata = cachedata.Where(t => t.F_DeleteMark == false && t.F_EnabledMark == true && t.F_Layers == 1).ToList();
            return cachedata.OrderBy(t => t.F_SortCode).ToList();
        }
        public AreaEntity GetForm(string keyValue)
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
            }
            RedisHelper.Del(cacheKey + keyValue);
            RedisHelper.Del(cacheKey + "list");
        }
        public void SubmitForm(AreaEntity mEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                mEntity.Modify(keyValue);
                service.Update(mEntity);
                RedisHelper.Del(cacheKey + keyValue);
                RedisHelper.Del(cacheKey + "list");
            }
            else
            {
                mEntity.Create();
                service.Insert(mEntity);
                RedisHelper.Del(cacheKey + "list");
            }
        }
    }
}
