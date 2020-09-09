/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using WaterCloud.Service.SystemManage;
using Chloe;

namespace WaterCloud.Service.SystemOrganize
{
    public class RoleService : DataFilterService<RoleEntity>, IDenpendency
    {
        private ModuleService moduleApp;
        private ModuleButtonService moduleButtonApp;
        private ModuleFieldsService moduleFieldsApp;
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_roledata_";
        private string authorizecacheKey = "watercloud_authorizeurldata_";// +权限
        private string initcacheKey = "watercloud_init_";
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public RoleService(IDbContext context) : base(context)
        {
            moduleApp = new ModuleService(context);
            moduleButtonApp = new ModuleButtonService(context);
            moduleFieldsApp = new ModuleFieldsService(context);
        }

        public async Task<List<RoleEntity>> GetList( string keyword = "")
        {
            var cachedata =await repository.CheckCacheList(cacheKey + "list");
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
            return GetFieldsFilterData(await repository.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }
        public async Task<RoleEntity> GetForm(string keyValue)
        {
            var cachedata =await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        public async Task<RoleEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata, className.Substring(0, className.Length - 7));
        }
        public async Task DeleteForm(string keyValue)
        {
            if (uniwork.IQueryable<UserEntity>(a => a.F_RoleId.Contains(keyValue)).Count() > 0 )
            {
                throw new Exception("角色使用中，无法删除");
            }
            uniwork.BeginTrans();
            await repository.Delete(t => t.F_Id == keyValue);
            await uniwork.Delete<RoleAuthorizeEntity>(t => t.F_ObjectId == keyValue);
            uniwork.Commit();
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
                roleEntity.F_DeleteMark = false;
                roleEntity.F_AllowEdit = false;
                roleEntity.F_AllowDelete = false;
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
            uniwork.BeginTrans();
            if (!string.IsNullOrEmpty(keyValue))
            {
                await repository.Update(roleEntity);
            }
            else
            {
                roleEntity.F_Category = 1;
                await repository.Insert(roleEntity);
            }
            await uniwork.Delete<RoleAuthorizeEntity>(t => t.F_ObjectId == roleEntity.F_Id);
            await uniwork.Insert(roleAuthorizeEntitys);
            uniwork.Commit();
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
