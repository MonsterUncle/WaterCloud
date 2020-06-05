/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;

namespace WaterCloud.Service.SystemManage
{
    public class ModuleButtonService : DataFilterService<ModuleButtonEntity>, IDenpendency
    {
        private IModuleButtonRepository service = new ModuleButtonRepository();
        private IModuleRepository moduleservice = new ModuleRepository();
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_modulebuttondata_";
        private string initcacheKey = "watercloud_init_";
        private string authorizecacheKey = "watercloud_authorizeurldata_";// +权限
        public async Task<List<ModuleButtonEntity>> GetList(string moduleId = "")
        {
            var list = new List<ModuleButtonEntity>();
            list = await service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(moduleId))
            {
                list = list.Where(t => t.F_ModuleId == moduleId).ToList();
            }
            return list.Where(a => a.F_DeleteMark == false).OrderBy(t => t.F_SortCode).ToList();
        }
        public async Task<List<ModuleButtonEntity>> GetLookList(string moduleId = "")
        {
            var list = new List<ModuleButtonEntity>();
            if (!CheckDataPrivilege(className.Substring(0, className.Length - 7)))
            {
                list = await service.CheckCacheList(cacheKey + "list");
            }
            else
            {
                var forms = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
                list = forms.ToList();
            }
            if (!string.IsNullOrEmpty(moduleId))
            {
                list = list.Where(t => t.F_ModuleId == moduleId).ToList();
            }
            return GetFieldsFilterData(list.Where(a => a.F_DeleteMark == false).OrderBy(t => t.F_SortCode).ToList(), className.Substring(0, className.Length - 7));
        }
        public async Task<ModuleButtonEntity> GetForm(string keyValue)
        {
            var cachedata =await service.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata, className.Substring(0, className.Length - 7));
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
                await RedisHelper.DelAsync(cacheKey + keyValue);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
            await RedisHelper.DelAsync(initcacheKey + "modulebutton_" + "list");
            await RedisHelper.DelAsync(authorizecacheKey + "list");
            await RedisHelper.DelAsync(authorizecacheKey + "authorize_list");
        }

        public async Task<List<ModuleButtonEntity>> GetListByRole(string roleid)
        {
            return await service.GetListByRole(roleid);
        }

        public async Task SubmitForm(ModuleButtonEntity moduleButtonEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                moduleButtonEntity.Modify(keyValue);
                await service.Update(moduleButtonEntity);
                await RedisHelper.DelAsync(cacheKey + keyValue);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
            else
            {
                var module = await moduleservice.FindEntity(a => a.F_Id == moduleButtonEntity.F_ModuleId);
                if (string.IsNullOrEmpty(module.F_UrlAddress) || module.F_Target != "iframe")
                {
                    throw new Exception("菜单不能创建按钮");
                }
                moduleButtonEntity.Create();
                await service.Insert(moduleButtonEntity);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
            await RedisHelper.DelAsync(initcacheKey + "modulebutton_" + "list");
            await RedisHelper.DelAsync(authorizecacheKey + "list");
            await RedisHelper.DelAsync(authorizecacheKey + "authorize_list");
        }
        public async Task SubmitCloneButton(string moduleId, string Ids)
        {
            string[] ArrayId = Ids.Split(',');
            var data =await this.GetList();
            List<ModuleButtonEntity> entitys = new List<ModuleButtonEntity>();
            var module = await moduleservice.FindEntity(a => a.F_Id == moduleId);
            if (string.IsNullOrEmpty(module.F_UrlAddress)||module.F_Target!= "iframe")
            {
                throw new Exception("菜单不能创建按钮");
            }
            foreach (string item in ArrayId)
            {
                ModuleButtonEntity moduleButtonEntity = data.Find(t => t.F_Id == item);
                moduleButtonEntity.Create();
                moduleButtonEntity.F_ModuleId = moduleId;
                entitys.Add(moduleButtonEntity);
            }
            await service.Insert(entitys);
            await RedisHelper.DelAsync(cacheKey + "list");
            await RedisHelper.DelAsync(initcacheKey + "modulebutton_" + "list");
            await RedisHelper.DelAsync(authorizecacheKey + "list");
            await RedisHelper.DelAsync(authorizecacheKey + "authorize_list");
        }

        public async Task<List<ModuleButtonEntity>> GetListNew(string moduleId = "")
        {
            return await service.GetListNew(moduleId);
        }
    }
}
