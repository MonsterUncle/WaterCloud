/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Repository.SystemOrganize;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using WaterCloud.Service.SystemManage;

namespace WaterCloud.Service.SystemOrganize
{
    public class RoleService : DataFilterService<RoleEntity>, IDenpendency
    {
        private IUserRepository userservice;
        private IRoleRepository service;
        private ModuleService moduleApp = new ModuleService();
        private ModuleButtonService moduleButtonApp = new ModuleButtonService();
        private ModuleFieldsService moduleFieldsApp = new ModuleFieldsService();
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_roledata_";
        private string authorizecacheKey = "watercloud_authorizeurldata_";// +权限
        private string initcacheKey = "watercloud_init_";
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public RoleService()
        {
            var currentuser = OperatorProvider.Provider.GetCurrent();
            service = currentuser != null ? new RoleRepository(currentuser.DbString, currentuser.DBProvider) : new RoleRepository();
            userservice = currentuser != null ? new UserRepository(currentuser.DbString, currentuser.DBProvider) : new UserRepository();
        }

        public async Task<List<RoleEntity>> GetList( string keyword = "")
        {
            var cachedata =await service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_FullName.Contains(keyword) || t.F_EnCode.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_Category == 1&&t.F_DeleteMark==false).ToList();
        }
        public async Task<List<RoleEntity>> GetLookList(Pagination pagination, string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(u => u.F_FullName.Contains(keyword) || u.F_EnCode.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark == false && u.F_Category == 1);
            return GetFieldsFilterData(await service.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }
        public async Task<RoleEntity> GetForm(string keyValue)
        {
            var cachedata =await service.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        public async Task<RoleEntity> GetLookForm(string keyValue)
        {
            var cachedata = await service.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata, className.Substring(0, className.Length - 7));
        }
        public async Task DeleteForm(string keyValue)
        {
            if (userservice.IQueryable(a => a.F_RoleId.Contains(keyValue)).Count() > 0 )
            {
                throw new Exception("角色使用中，无法删除");
            }
            await service.DeleteForm(keyValue);
            await CacheHelper.Remove(cacheKey + keyValue);
            await CacheHelper.Remove(cacheKey + "list");
            await CacheHelper.Remove(authorizecacheKey + "list");
            await CacheHelper.Remove(authorizecacheKey + "authorize_list");
            await CacheHelper.Remove(initcacheKey + "modulebutton_list");
            await CacheHelper.Remove(initcacheKey + "modulefields_list");
            await CacheHelper.Remove(initcacheKey + "list");
        }
        public async Task SubmitForm(RoleEntity roleEntity, string[] permissionIds,string[] permissionfieldsIds, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                roleEntity.F_Id = keyValue;

            }
            else
            {
                roleEntity.Create();

            }
            var moduledata =await moduleApp.GetList();
            var buttondata =await moduleButtonApp.GetList();
            var fieldsdata = await moduleFieldsApp.GetList();
            List<RoleAuthorizeEntity> roleAuthorizeEntitys = new List<RoleAuthorizeEntity>();
            foreach (var itemId in permissionIds)
            {
                RoleAuthorizeEntity roleAuthorizeEntity = new RoleAuthorizeEntity();
                roleAuthorizeEntity.F_Id = Utils.GuId();
                roleAuthorizeEntity.F_ObjectType = 1;
                roleAuthorizeEntity.F_ObjectId = roleEntity.F_Id;
                roleAuthorizeEntity.F_ItemId = itemId;
                if (moduledata.Find(t => t.F_Id == itemId) != null)
                {
                    roleAuthorizeEntity.F_ItemType = 1;
                    roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                }
                if (buttondata.Find(t => t.F_Id == itemId) != null)
                {
                    roleAuthorizeEntity.F_ItemType = 2;
                    roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                }
            }
            foreach (var itemId in permissionfieldsIds)
            {
                RoleAuthorizeEntity roleAuthorizeEntity = new RoleAuthorizeEntity();
                roleAuthorizeEntity.F_Id = Utils.GuId();
                roleAuthorizeEntity.F_ObjectType = 1;
                roleAuthorizeEntity.F_ObjectId = roleEntity.F_Id;
                roleAuthorizeEntity.F_ItemId = itemId;
                if (fieldsdata.Find(t => t.F_Id == itemId) != null)
                {
                    roleAuthorizeEntity.F_ItemType = 3;
                    roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                }
            }
            await service.SubmitForm(roleEntity, roleAuthorizeEntitys, keyValue);
            await CacheHelper.Remove(cacheKey + keyValue);
            await CacheHelper.Remove(cacheKey + "list");
            await CacheHelper.Remove(authorizecacheKey + "list");
            await CacheHelper.Remove(authorizecacheKey + "authorize_list");
            await CacheHelper.Remove(initcacheKey + "modulebutton_list");
            await CacheHelper.Remove(initcacheKey + "modulefields_list");
            await CacheHelper.Remove(initcacheKey + "list");
        }
    }
}
