﻿/*******************************************************************************
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
using SqlSugar;
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
            var query = repository.IQueryable();
            return await query.Where(a => a.DeleteMark == false).OrderBy(a => a.SortCode).ToListAsync();
        }
        public async Task<List<ItemsEntity>> GetLookList()
        {
            var query = repository.IQueryable().Where(a => a.DeleteMark == false);
            query = GetDataPrivilege("a","",query);
            return await query.OrderBy(a => a.SortCode).ToListAsync();
        }
        public async Task<ItemsEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);

        }
        public async Task<ItemsEntity> GetForm(string keyValue)
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
                itemsEntity.DeleteMark = false;
                itemsEntity.IsTree = false;
                itemsEntity.Create();
                await repository.Insert(itemsEntity);
            }
        }
    }
}
