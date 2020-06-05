﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Repository.SystemManage;

namespace WaterCloud.Service.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-01 09:44
    /// 描 述：数据权限服务类
    /// </summary>
    public class DataPrivilegeRuleService : DataFilterService<DataPrivilegeRuleEntity>,IDenpendency
    {
        private IDataPrivilegeRuleRepository service = new DataPrivilegeRuleRepository();
        private IModuleRepository moduleservice = new ModuleRepository();
        private string cacheKey = "watercloud_ dataprivilegeruledata_";
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        #region 获取数据
        public async Task<List<DataPrivilegeRuleEntity>> GetList(string keyword = "")
        {
            var list = new List<DataPrivilegeRuleEntity>();
            list = await service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(t => t.F_ModuleCode.Contains(keyword) || t.F_Description.Contains(keyword)).ToList();
            }
            return list.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public async Task<List<DataPrivilegeRuleEntity>> GetLookList(Pagination pagination,string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(u => u.F_ModuleCode.Contains(keyword) || u.F_Description.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark == false);
            return GetFieldsFilterData(await service.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }

        public async Task<DataPrivilegeRuleEntity> GetForm(string keyValue)
        {
            var cachedata = await service.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata, className.Substring(0, className.Length - 7));
        }
        #endregion

        #region 提交数据
        public async Task SubmitForm(DataPrivilegeRuleEntity entity, string keyValue)
        {
            entity.F_ModuleCode = (await moduleservice.FindEntity(entity.F_ModuleId)).F_EnCode;
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.Create();
                await service.Insert(entity);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
            else
            {
                entity.Modify(keyValue); 
                await service.Update(entity);
                await RedisHelper.DelAsync(cacheKey + keyValue);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
        }

        public async Task DeleteForm(string keyValue)
        {
            await service.Delete(t => t.F_Id == keyValue);
            await RedisHelper.DelAsync(cacheKey + keyValue);
            await RedisHelper.DelAsync(cacheKey + "list");
        }
        #endregion

    }
}
