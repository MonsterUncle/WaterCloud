/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Domain.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCloud.Code;
using SqlSugar;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemManage
{
    public class AreaService : DataFilterService<AreaEntity>, IDenpendency
    {
        public AreaService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<List<AreaEntity>> GetList(int layers = 0)
        {
            var query = repository.IQueryable();
            if (layers != 0)
            {
                query = query.Where(a => a.F_Layers == layers);
            }
            return await query.Where(a => a.F_DeleteMark == false && a.F_EnabledMark == true).OrderBy(a => a.F_SortCode).ToListAsync();
        }
        public async Task<List<AreaEntity>> GetLookList(int layers=0)
        {
            var query =repository .IQueryable ().Where(a => a.F_DeleteMark == false && a.F_EnabledMark == true);
            if (layers!=0)
            { 
                query = query.Where(a => a.F_Layers == layers);
            }
            query = GetDataPrivilege("a","", query);
            return await query.OrderBy(a => a.F_SortCode).ToListAsync();
        }
        public async Task<AreaEntity> GetLookForm(string keyValue)
        {
            var data =await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }
        public async Task<AreaEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        public async Task DeleteForm(string keyValue)
        {
            if (repository.IQueryable(a => a.F_ParentId.Equals(keyValue)).Count() > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
               await repository.Delete(a => a.F_Id == keyValue);
            }
        }
        public async Task SubmitForm(AreaEntity mEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                mEntity.Modify(keyValue);
                await repository.Update(mEntity);
            }
            else
            {
                mEntity.F_DeleteMark = false;
                mEntity.Create();
                await repository.Insert(mEntity);
            }
        }
    }
}
