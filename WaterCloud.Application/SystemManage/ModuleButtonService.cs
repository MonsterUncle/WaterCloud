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

namespace WaterCloud.Service.SystemManage
{
    public class ModuleButtonService: IDenpendency
    {
        private IModuleButtonRepository service = new ModuleButtonRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_modulebuttondata_";
        private string initcacheKey = "watercloud_init_";

        public List<ModuleButtonEntity> GetList(string moduleId = "")
        {
            var cachedata = service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(moduleId))
            {
                cachedata = cachedata.Where(t => t.F_ModuleId == moduleId).ToList();
            }
            return cachedata.OrderBy(t => t.F_SortCode).ToList();
        }
        public ModuleButtonEntity GetForm(string keyValue)
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
            }
            RedisHelper.Del(initcacheKey + "modulebutton_" + "list");
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
                RedisHelper.Del(cacheKey + keyValue);
                RedisHelper.Del(cacheKey + "list");
            }
            else
            {
                moduleButtonEntity.Create();
                service.Insert(moduleButtonEntity);
                RedisHelper.Del(cacheKey + "list");
            }
            RedisHelper.Del(initcacheKey + "modulebutton_" + "list");
        }
        public void SubmitCloneButton(string moduleId, string Ids)
        {
            string[] ArrayId = Ids.Split(',');
            var data = this.GetList();
            List<ModuleButtonEntity> entitys = new List<ModuleButtonEntity>();
            foreach (string item in ArrayId)
            {
                ModuleButtonEntity moduleButtonEntity = data.Find(t => t.F_Id == item);
                moduleButtonEntity.F_Id = Utils.GuId();
                moduleButtonEntity.F_ModuleId = moduleId;
                entitys.Add(moduleButtonEntity);
            }
            service.SubmitCloneButton(entitys);
            RedisHelper.Del(cacheKey + "list");
        }

        public List<ModuleButtonEntity> GetListNew(string moduleId = "")
        {
            return service.GetListNew(moduleId);
        }
    }
}
