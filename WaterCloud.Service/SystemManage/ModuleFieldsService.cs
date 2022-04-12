using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;
using SqlSugar;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-05-21 14:38
    /// 描 述：字段管理服务类
    /// </summary>
    public class ModuleFieldsService : DataFilterService<ModuleFieldsEntity>, IDenpendency
    {
        public ModuleFieldsService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #region 获取数据
        public async Task<List<ModuleFieldsEntity>> GetList(string keyword = "")
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(a => a.FullName.Contains(keyword) || a.EnCode.Contains(keyword));
            }
            return await query.Where(a => a.DeleteMark == false).OrderBy(a => a.Id,OrderByType.Desc).ToListAsync();
        }

        public async Task<List<ModuleFieldsEntity>> GetLookList(Pagination pagination, string moduleId, string keyword = "")
        {
            //获取数据权限
            var query = repository.IQueryable().Where(a => a.DeleteMark == false && a.ModuleId == moduleId);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.FullName.Contains(keyword) || a.EnCode.Contains(keyword));
            }
            query = GetDataPrivilege("a","", query);
			return await query.ToPageListAsync(pagination);

        }

        public async Task<ModuleFieldsEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }
        public async Task<ModuleFieldsEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        #endregion

        #region 提交数据
        public async Task SubmitForm(ModuleFieldsEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.DeleteMark = false;
                entity.Create();
                await repository.Insert(entity);
            }
            else
            {
                entity.Modify(keyValue);
                await repository.Update(entity);
            }
        }

        public async Task DeleteForm(string keyValue)
        {
            await repository.Delete(a => a.Id == keyValue);
        }

        public async Task SubmitCloneFields(string moduleId, string ids)
        {
            string[] ArrayId = ids.Split(',');
            var data = await this.GetList();
            List<ModuleFieldsEntity> entitys = new List<ModuleFieldsEntity>();
            var module = await repository.Db.Queryable<ModuleEntity>().Where(a => a.Id == moduleId).FirstAsync();
            if (string.IsNullOrEmpty(module.UrlAddress) || module.Target != "iframe")
            {
                throw new Exception("框架页才能创建字段");
            }
            foreach (string item in ArrayId)
            {
                ModuleFieldsEntity moduleFieldsEntity = data.Find(a => a.Id == item);
                moduleFieldsEntity.Create();
                moduleFieldsEntity.ModuleId = moduleId;
                entitys.Add(moduleFieldsEntity);
            }
            await repository.Insert(entitys);
        }

        public async Task<List<ModuleFieldsEntity>> GetListByRole(string roleid)
        {
            var moduleList = repository.Db.Queryable<RoleAuthorizeEntity>().Where(a => a.ObjectId == roleid && a.ItemType == 3).Select(a => a.ItemId).ToList();
            var query = repository.IQueryable().Where(a => (moduleList.Contains(a.Id) || a.IsPublic == true) && a.DeleteMark == false && a.EnabledMark == true);
            return await query.OrderBy(a => a.CreatorTime,OrderByType.Desc).ToListAsync();
        }

        internal async Task<List<ModuleFieldsEntity>> GetListNew(string moduleId = "")
        {
            var query = repository.Db.Queryable<ModuleFieldsEntity, ModuleEntity>((a,b) => new JoinQueryInfos (
                JoinType.Inner,a.ModuleId==b.Id && b.EnabledMark == true
                ))
            .Select((a, b) => new ModuleFieldsEntity
            {
                Id = a.Id,
                CreatorTime = a.CreatorTime,
                CreatorUserId = a.CreatorUserId,
                DeleteMark = a.DeleteMark,
                DeleteTime = a.DeleteTime,
                DeleteUserId = a.DeleteUserId,
                Description = a.Description,
                EnabledMark = a.EnabledMark,
                EnCode = a.EnCode,
                FullName = a.FullName,
                LastModifyTime = a.LastModifyTime,
                LastModifyUserId = a.LastModifyUserId,
                ModuleId = b.UrlAddress,
                IsPublic = a.IsPublic
            }).MergeTable();
            if (!string.IsNullOrEmpty(moduleId))
            {
                query = query.Where(a => a.ModuleId == moduleId);
            }
            return await query.OrderBy(a => a.CreatorTime,OrderByType.Desc).ToListAsync();
        }
        #endregion

    }
}
