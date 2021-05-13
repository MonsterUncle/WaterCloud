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
using SqlSugar;
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
            var query = repository.IQueryable();
            return await query.Where(a => a.F_DeleteMark == false).OrderBy(t => t.F_SortCode).ToListAsync();
        }
        public async Task<List<ModuleEntity>> GetBesidesList()
        {
            var moduleList = repository.Db.Queryable<DataPrivilegeRuleEntity>().Select(a => a.F_ModuleId).ToList();
            var query = repository.IQueryable().Where(a => !moduleList.Contains(a.F_Id) && a.F_EnabledMark == true && a.F_Target == "iframe");
            return await query.OrderBy(a => a.F_SortCode).ToListAsync();
        }
        public async Task<List<ModuleEntity>> GetLookList()
        {
            var query = repository.IQueryable().Where(u => u.F_DeleteMark == false);
            query = GetDataPrivilege("u", "", query);
            return await query.OrderBy(u => u.F_SortCode).ToListAsync();
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
                unitofwork.BeginTrans();
                await repository.Delete(a => a.F_Id == keyValue);
                await repository.Db.Deleteable<ModuleButtonEntity>().Where(a => a.F_ModuleId == keyValue).ExecuteCommandAsync();
                await repository.Db.Deleteable<ModuleFieldsEntity>().Where(a => a.F_ModuleId == keyValue).ExecuteCommandAsync();
                unitofwork.Commit();
                await CacheHelper.Remove(authorizecacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
            }
        }

        public async Task<List<ModuleEntity>> GetListByRole(string roleid)
        {
            var moduleList = repository.Db.Queryable<RoleAuthorizeEntity>().Where(a => a.F_ObjectId == roleid && a.F_ItemType == 1).Select(a => a.F_ItemId).ToList();
            var query = repository.IQueryable().Where(a => (moduleList.Contains(a.F_Id) || a.F_IsPublic == true) && a.F_DeleteMark == false && a.F_EnabledMark == true);
            return await query.OrderBy(a => a.F_SortCode).ToListAsync();
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
            await CacheHelper.Remove(authorizecacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
        }
    }
}
