/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Domain.SystemManage;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaterCloud.Code;
using Chloe;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemManage
{
    public class ItemsTypeService : DataFilterService<ItemsEntity>,IDenpendency
    {  
        public ItemsTypeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<List<ItemsEntity>> GetList()
        {
            var data = repository.IQueryable();
            return await data.Where(a=>a.F_DeleteMark==false).OrderBy(t => t.F_SortCode).ToListAsync();
        }
        public async Task<List<ItemsEntity>> GetLookList()
        {
            var query = repository.IQueryable().Where(a => a.F_DeleteMark == false);
            query = GetDataPrivilege("u","",query);
            return await query.OrderBy(t => t.F_SortCode).ToListAsync();
        }
        public async Task<ItemsEntity> GetLookForm(string keyValue)
        {
            var data =await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);

        }
        public async Task<ItemsEntity> GetForm(string keyValue)
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
                await repository.Delete(t => t.F_Id == keyValue);
            }
        }
        public async Task SubmitForm(ItemsEntity itemsEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                itemsEntity.Modify(keyValue);
                await repository.Update(itemsEntity);
            }
            else
            {
                itemsEntity.F_DeleteMark = false;
                itemsEntity.F_IsTree = false;
                itemsEntity.Create();
                await repository.Insert(itemsEntity);
            }
        }
    }
}
