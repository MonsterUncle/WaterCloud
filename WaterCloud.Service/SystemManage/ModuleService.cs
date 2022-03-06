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
        private string authorizecacheKey = GlobalContext.SystemConfig.ProjectPrefix + "_authorizeurldata_";// +权限
        //获取类名

        public ModuleService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<ModuleEntity>> GetList()
        {
            var query = repository.IQueryable();
            return await query.Where(a => a.F_DeleteMark == false).OrderBy(a => a.F_SortCode).ToListAsync();
        }
        public async Task<List<ModuleEntity>> GetBesidesList()
        {
            var moduleList = repository.Db.Queryable<DataPrivilegeRuleEntity>().Select(a => a.F_ModuleId).ToList();
            var query = repository.IQueryable().Where(a => !moduleList.Contains(a.F_Id) && a.F_EnabledMark == true && a.F_Target == "iframe");
            return await query.OrderBy(a => a.F_SortCode).ToListAsync();
        }
        public async Task<List<ModuleEntity>> GetLookList()
        {
            var query = repository.IQueryable().Where(a => a.F_DeleteMark == false);
            query = GetDataPrivilege("a", "", query);
            return await query.OrderBy(a => a.F_SortCode).ToListAsync();
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
        public async Task<string> GetMaxSortCode(string F_ParentId)
        {
			try
			{
                int F_SortCode = (int)await repository.Db.Queryable<ModuleEntity>().Where(t => t.F_ParentId == F_ParentId).MaxAsync(a => a.F_SortCode);

                return (F_SortCode + 1).ToString();
            }
			catch (Exception)
			{
                return "0";
			}
        }
        public async Task DeleteForm(string keyValue)
        {
            if (await repository.IQueryable(a => a.F_ParentId.Equals(keyValue)).AnyAsync())
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                unitofwork.CurrentBeginTrans();
                await repository.Delete(a => a.F_Id == keyValue);
                await repository.Db.Deleteable<ModuleButtonEntity>().Where(a => a.F_ModuleId == keyValue).ExecuteCommandAsync();
                await repository.Db.Deleteable<ModuleFieldsEntity>().Where(a => a.F_ModuleId == keyValue).ExecuteCommandAsync();
                unitofwork.CurrentCommit();
                await CacheHelper.RemoveAsync(authorizecacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
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
            await CacheHelper.RemoveAsync(authorizecacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
        }
        /// <summary>
        /// 更新菜单排序
        /// </summary>
        /// <param name="F_Id">内码</param>
        /// <param name="SortCode">排序数字</param>
        /// <returns></returns>
        public async Task SubmitUpdateForm(string F_Id, int SortCode)
        {
            //更新
            await repository.Update(a => a.F_Id == F_Id, a => new ModuleEntity()
            {
                F_SortCode = SortCode
            });
        }
    }
}
