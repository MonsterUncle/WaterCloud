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
    public class DutyService : DataFilterService<RoleEntity>, IDenpendency
    {
        private IUserRepository userservice = new UserRepository();
        private IRoleRepository service = new RoleRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private string cacheKey = "watercloud_dutydata_";// 岗位
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];

        public async Task<List<RoleEntity>> GetList(string keyword = "")
        {
            var cachedata =await service.CheckCacheList(cacheKey + "list");
            cachedata = cachedata.Where(t => t.F_Category == 2&&t.F_DeleteMark==false).ToList();
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_FullName.Contains(keyword) || t.F_EnCode.Contains(keyword)).ToList();
            }
            return cachedata.OrderBy(t => t.F_SortCode).ToList();
        }
        public async Task<List<RoleEntity>> GetLookList(Pagination pagination, string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(u => u.F_FullName.Contains(keyword) || u.F_EnCode.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark == false&& u.F_Category == 2);
            return GetFieldsFilterData(await service.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }
        public async Task<RoleEntity> GetLookForm(string keyValue)
        {
            var cachedata = await service.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata, className.Substring(0, className.Length - 7));
        }
        public async Task<RoleEntity> GetForm(string keyValue)
        {
            var cachedata = await service.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        public async Task DeleteForm(string keyValue)
        {
            if (userservice.IQueryable(a => a.F_DutyId == keyValue).Count() > 0)
            {
                throw new Exception("岗位使用中，无法删除");
            }
            await service.Delete(t => t.F_Id == keyValue);
            await CacheHelper.Remove(cacheKey + keyValue);
            await CacheHelper.Remove(cacheKey + "list");
        }
        public async Task SubmitForm(RoleEntity roleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                roleEntity.Modify(keyValue);
                await service.Update(roleEntity);
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                roleEntity.Create();
                roleEntity.F_Category = 2;
                await service.Insert(roleEntity);
                await CacheHelper.Remove(cacheKey + "list");
            }
        }
    }
}
