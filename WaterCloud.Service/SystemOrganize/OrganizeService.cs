/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Domain.SystemOrganize;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using WaterCloud.Code;
using Chloe;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemOrganize
{
    public class OrganizeService : DataFilterService<OrganizeEntity>, IDenpendency
    {
        public OrganizeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<List<OrganizeEntity>> GetList()
        {
            var data = repository.IQueryable();
            return data.Where(a=>a.F_DeleteMark==false).ToList();
        }
        public async Task<List<OrganizeEntity>> GetLookList()
        {
            var query = repository.IQueryable().Where(a => a.F_DeleteMark == false);
            query = GetDataPrivilege("u","",query);
            return query.OrderBy(t => t.F_SortCode).ToList();
        }
        public async Task<OrganizeEntity> GetLookForm(string keyValue)
        {
            var data =await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }
        public async Task<OrganizeEntity> GetForm(string keyValue)
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
                if (unitwork.IQueryable<UserEntity>(a=>a.F_OrganizeId==keyValue).Count()>0|| unitwork.IQueryable<UserEntity>(a => a.F_DepartmentId == keyValue).Count()>0)
                {
                    throw new Exception("组织使用中，无法删除");
                }
                await repository.Delete(t => t.F_Id == keyValue);
            }
        }
        public async Task SubmitForm(OrganizeEntity organizeEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                organizeEntity.Modify(keyValue);
                await repository.Update(organizeEntity);
            }
            else
            {
                organizeEntity.F_AllowDelete = false;
                organizeEntity.F_AllowEdit = false;
                organizeEntity.F_DeleteMark = false;
                organizeEntity.Create();
                await repository.Insert(organizeEntity);
            }
        }
    }
}
