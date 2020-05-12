/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Repository.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace WaterCloud.Service.SystemManage
{
    public class RoleService: IDenpendency
    {
        private IUserRepository userservice = new UserRepository();
        private IRoleRepository service = new RoleRepository();
        private ModuleService moduleApp = new ModuleService();
        private ModuleButtonService moduleButtonApp = new ModuleButtonService();
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_roledata_";
        private string authorizecacheKey = "watercloud_authorizeurldata_";// +权限

        public async Task<List<RoleEntity>> GetList( string keyword = "")
        {
            var cachedata =await service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_FullName.Contains(keyword) || t.F_EnCode.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_Category == 1).ToList();
        }
        public async Task<List<RoleEntity>> GetList(Pagination pagination, string keyword = "")
        {
            var expression = ExtLinq.True<RoleEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_FullName.Contains(keyword));
                expression = expression.Or(t => t.F_EnCode.Contains(keyword));
            }
            expression = expression.And(t => t.F_Category == 1);
            return await service.FindList(expression, pagination);
        }
        public async Task<RoleEntity> GetForm(string keyValue)
        {
            var cachedata =await service.CheckCache(cacheKey, keyValue);
            return cachedata;
        }

        public async Task DeleteForm(string keyValue)
        {
            if (userservice.IQueryable(a => a.F_RoleId == keyValue).Count() > 0 )
            {
                throw new Exception("角色使用中，无法删除");
            }
            await service.DeleteForm(keyValue);
            await  RedisHelper.DelAsync(cacheKey + keyValue);
            await  RedisHelper.DelAsync(cacheKey + "list");
            await  RedisHelper.DelAsync(authorizecacheKey + "list");
            await  RedisHelper.DelAsync(authorizecacheKey + keyValue);
        }
        public async Task SubmitForm(RoleEntity roleEntity, string[] permissionIds, string keyValue)
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
                }
                if (buttondata.Find(t => t.F_Id == itemId) != null)
                {
                    roleAuthorizeEntity.F_ItemType = 2;
                }
                roleAuthorizeEntitys.Add(roleAuthorizeEntity);
            }
            await service.SubmitForm(roleEntity, roleAuthorizeEntitys, keyValue);
            await  RedisHelper.DelAsync(cacheKey + keyValue);
            await  RedisHelper.DelAsync(cacheKey + "list");
            await  RedisHelper.DelAsync(authorizecacheKey + "list");
            await  RedisHelper.DelAsync(authorizecacheKey + keyValue);
        }
    }
}
