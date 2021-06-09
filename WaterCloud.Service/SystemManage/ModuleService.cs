/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Domain.SystemManage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using WaterCloud.Code;
using Chloe;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemManage
{
    public class ModuleService : DataFilterService<ModuleEntity>, IDenpendency
    {
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private string authorizecacheKey = "watercloud_authorizeurldata_";// +权限
        //获取类名

        public ModuleService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<ModuleEntity>> GetList()
        {
            var data =  repository.IQueryable();
            return data.Where(a => a.F_DeleteMark == false).OrderBy(t => t.F_SortCode).ToList();
        }
        public async Task<List<ModuleEntity>> GetBesidesList()
        {
            var moduleList = unitwork.IQueryable<DataPrivilegeRuleEntity>().Select(a => a.F_ModuleId).ToList();
            var query = repository.IQueryable().Where(a => !moduleList.Contains(a.F_Id) && a.F_EnabledMark == true && a.F_Target == "iframe");
            return query.OrderBy(a => a.F_SortCode).ToList();
        }
        public async Task<List<ModuleEntity>> GetLookList()
        {
            var query = repository.IQueryable().Where(u => u.F_DeleteMark == false);
            query = GetDataPrivilege("u", "", query);
            return query.OrderBy(u => u.F_SortCode).ToList();
        }
        public async Task<ModuleEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }
        public async Task<ModuleEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        public async Task DeleteForm(string keyValue)
        {
            if (repository.IQueryable(t => t.F_ParentId.Equals(keyValue)).Count() > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                unitwork.BeginTrans();
                await repository.Delete(a => a.F_Id == keyValue);
                await unitwork.Delete<ModuleButtonEntity>(a => a.F_ModuleId == keyValue);
                await unitwork.Delete<ModuleFieldsEntity>(a => a.F_ModuleId == keyValue);
                unitwork.Commit();
                await CacheHelper.Remove(authorizecacheKey + "list");
            }
        }

        public async Task<List<ModuleEntity>> GetListByRole(string roleid)
        {
            var moduleList = unitwork.IQueryable<RoleAuthorizeEntity>(a => a.F_ObjectId == roleid && a.F_ItemType == 1).Select(a => a.F_ItemId).ToList();
            var query = repository.IQueryable().Where(a => (moduleList.Contains(a.F_Id) || a.F_IsPublic == true) && a.F_DeleteMark == false && a.F_EnabledMark == true);
            return query.OrderBy(a => a.F_SortCode).ToList();
        }

        public async Task SubmitForm(ModuleEntity moduleEntity, string keyValue)
        {
			if (!string.IsNullOrEmpty(moduleEntity.F_Authorize))
			{
                moduleEntity.F_Authorize = moduleEntity.F_Authorize.ToLower();
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                moduleEntity.Modify(keyValue);
                await repository.Update(moduleEntity);
            }
            else
            {
                moduleEntity.Create();
                await repository.Insert(moduleEntity);
            }
            await CacheHelper.Remove(authorizecacheKey + "list");
        }
    }
}
