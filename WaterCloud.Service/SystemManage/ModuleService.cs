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
            return await query.Where(a => a.DeleteMark == false).OrderBy(a => a.SortCode).ToListAsync();
        }
        public async Task<List<ModuleEntity>> GetBesidesList()
        {
            var moduleList = repository.Db.Queryable<DataPrivilegeRuleEntity>().Select(a => a.ModuleId).ToList();
            var query = repository.IQueryable().Where(a => !moduleList.Contains(a.Id) && a.EnabledMark == true && a.Target == "iframe");
            return await query.OrderBy(a => a.SortCode).ToListAsync();
        }
        public async Task<List<ModuleEntity>> GetLookList()
        {
            var query = repository.IQueryable().Where(a => a.DeleteMark == false);
            query = GetDataPrivilege("a", "", query);
            return await query.OrderBy(a => a.SortCode).ToListAsync();
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
        public async Task<string> GetMaxSortCode(string ParentId)
        {
			try
			{
                int SortCode = (int)await repository.Db.Queryable<ModuleEntity>().Where(t => t.ParentId == ParentId).MaxAsync(a => a.SortCode);

                return (SortCode + 1).ToString();
            }
			catch (Exception)
			{
                return "0";
			}
        }
        public async Task DeleteForm(string keyValue)
        {
            if (await repository.IQueryable(a => a.ParentId.Equals(keyValue)).AnyAsync())
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                unitofwork.CurrentBeginTrans();
                await repository.Delete(a => a.Id == keyValue);
                await repository.Db.Deleteable<ModuleButtonEntity>().Where(a => a.ModuleId == keyValue).ExecuteCommandAsync();
                await repository.Db.Deleteable<ModuleFieldsEntity>().Where(a => a.ModuleId == keyValue).ExecuteCommandAsync();
                unitofwork.CurrentCommit();
                await CacheHelper.RemoveAsync(authorizecacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
            }
        }

        public async Task<List<ModuleEntity>> GetListByRole(string roleid)
        {
            var moduleList = repository.Db.Queryable<RoleAuthorizeEntity>().Where(a => a.ObjectId == roleid && a.ItemType == 1).Select(a => a.ItemId).ToList();
            var query = repository.IQueryable().Where(a => (moduleList.Contains(a.Id) || a.IsPublic == true) && a.DeleteMark == false && a.EnabledMark == true);
            return await query.OrderBy(a => a.SortCode).ToListAsync();
        }

        public async Task SubmitForm(ModuleEntity moduleEntity, string keyValue)
        {
			if (!string.IsNullOrEmpty(moduleEntity.Authorize))
			{
                moduleEntity.Authorize = moduleEntity.Authorize.ToLower();
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
        /// <param name="Id">内码</param>
        /// <param name="SortCode">排序数字</param>
        /// <returns></returns>
        public async Task SubmitUpdateForm(string Id, int SortCode)
        {
            //更新
            await repository.Update(a => a.Id == Id, a => new ModuleEntity()
            {
                SortCode = SortCode
            });
        }
    }
}
