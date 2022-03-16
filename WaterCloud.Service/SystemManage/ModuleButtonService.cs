/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemManage
{
    public class ModuleButtonService : DataFilterService<ModuleButtonEntity>, IDenpendency
    {
        private string authorizecacheKey = GlobalContext.SystemConfig.ProjectPrefix + "_authorizeurldata_";// +权限
        public ModuleButtonService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<List<ModuleButtonEntity>> GetList(string moduleId = "")
        {
            var list = repository.IQueryable();
            if (!string.IsNullOrEmpty(moduleId))
            {
                list = list.Where(a => a.ModuleId == moduleId);
            }
            return await list.Where(a => a.DeleteMark == false).OrderBy(a => a.SortCode).ToListAsync();
        }
        public async Task<List<ModuleButtonEntity>> GetLookList(string moduleId = "", string keyword = "")
        {
            var query = repository.IQueryable().Where(a => a.DeleteMark == false);
            if (!string.IsNullOrEmpty(moduleId))
            {
                query = query.Where(a => a.ModuleId == moduleId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(a => a.FullName.Contains(keyword) || a.EnCode.Contains(keyword));
            }
            query = GetDataPrivilege("a", "", query);
            return await query.OrderBy(a => a.SortCode).ToListAsync();
        }
        public async Task<ModuleButtonEntity> GetLookForm(string keyValue)
        {
            var data =await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }
        public async Task<ModuleButtonEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        public async Task DeleteForm(string keyValue)
        {
            if (await repository.IQueryable(a => a.ParentId.Equals(keyValue)).AnyAsync())
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                await repository.Delete(a => a.Id == keyValue);
            }
            await CacheHelper.RemoveAsync(authorizecacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
        }

        public async Task<List<ModuleButtonEntity>> GetListByRole(string roleid)
        {
            var moduleList = repository.Db.Queryable<RoleAuthorizeEntity>().Where(a => a.ObjectId == roleid && a.ItemType == 2).Select(a => a.ItemId).ToList();
            var query = repository.IQueryable().Where(a => (moduleList.Contains(a.Id) || a.IsPublic == true) && a.DeleteMark == false && a.EnabledMark == true);
            return await query.OrderBy(a => a.SortCode).ToListAsync();
        }

        public async Task SubmitForm(ModuleButtonEntity moduleButtonEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(moduleButtonEntity.Authorize))
            {
                moduleButtonEntity.Authorize = moduleButtonEntity.Authorize.ToLower();
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                moduleButtonEntity.Modify(keyValue);
                await repository.Update(moduleButtonEntity);
            }
            else
            {
                moduleButtonEntity.DeleteMark = false;
                moduleButtonEntity.AllowEdit = false;
                moduleButtonEntity.AllowDelete = false;
                var module = await repository.Db.Queryable<ModuleEntity>().Where(a => a.Id == moduleButtonEntity.ModuleId).FirstAsync();
                if (module.Target != "iframe" && module.Target != "expand")
                {
                    throw new Exception("菜单不能创建按钮");
                }
                moduleButtonEntity.Create();
                await repository.Insert(moduleButtonEntity);
            }
            await CacheHelper.RemoveAsync(authorizecacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
        }
        public async Task SubmitCloneButton(string moduleId, string Ids)
        {
            string[] ArrayId = Ids.Split(',');
            var data =await this.GetList();
            List<ModuleButtonEntity> entitys = new List<ModuleButtonEntity>();
            var module = await repository.Db.Queryable<ModuleEntity>().Where(a => a.Id == moduleId).FirstAsync();
            if (module.Target != "iframe" && module.Target != "expand")
            {
                throw new Exception("菜单不能创建按钮");
            }
            foreach (string item in ArrayId)
            {
                ModuleButtonEntity moduleButtonEntity = data.Find(a => a.Id == item);
                moduleButtonEntity.Create();
                moduleButtonEntity.ModuleId = moduleId;
                entitys.Add(moduleButtonEntity);
            }
            await repository.Insert(entitys);
            await CacheHelper.RemoveAsync(authorizecacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
        }

        public async Task<List<ModuleButtonEntity>> GetListNew(string moduleId = "")
        {
            var query = repository.Db.Queryable<ModuleButtonEntity, ModuleEntity>((a,b)=>new JoinQueryInfos(
                JoinType.Inner, a.ModuleId == b.Id && b.EnabledMark == true && b.DeleteMark == false

                )).Where(a => a.EnabledMark == true && a.DeleteMark == false)
            .Select((a, b) => new ModuleButtonEntity
            {
                Id = a.Id,
                AllowDelete = a.AllowDelete,
                AllowEdit = a.AllowEdit,
                UrlAddress = a.UrlAddress,
                CreatorTime = a.CreatorTime,
                CreatorUserId = a.CreatorUserId,
                DeleteMark = a.DeleteMark,
                DeleteTime = a.DeleteTime,
                DeleteUserId = a.DeleteUserId,
                Description = a.Description,
                EnabledMark = a.EnabledMark,
                EnCode = a.EnCode,
                FullName = a.FullName,
                Icon = a.Icon,
                IsPublic = a.IsPublic,
                JsEvent = a.JsEvent,
                LastModifyTime = a.LastModifyTime,
                LastModifyUserId = a.LastModifyUserId,
                Layers = a.Layers,
                Location = a.Location,
                ModuleId = b.UrlAddress,
                ParentId = a.ParentId,
                SortCode = a.SortCode,
                Split = a.Split,
            }).MergeTable();
            if (!string.IsNullOrEmpty(moduleId))
            {
                query = query.Where(a => a.ModuleId == moduleId);
            }
            return await query.OrderBy(a => a.SortCode).ToListAsync();
        }
    }
}
