/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Domain.SystemManage;
using WaterCloud.Repository.SystemManage;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WaterCloud.Service.SystemManage
{
    public class ItemsTypeService : DataFilterService<ItemsEntity>,IDenpendency
    {
        private IItemsRepository service = new ItemsRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private string cacheKey = "watercloud_itemsdata_";// 字典分类
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];

        public async Task<List<ItemsEntity>> GetList()
        {
            var cachedata =await service.CheckCacheList(cacheKey + "list");
            return cachedata.Where(a=>a.F_DeleteMark==false).OrderBy(t => t.F_SortCode).ToList();
        }
        public async Task<List<ItemsEntity>> GetLookList()
        {
            var list = new List<ItemsEntity>();
            if (!CheckDataPrivilege(className.Substring(0, className.Length - 7)))
            {
                list = await service.CheckCacheList(cacheKey + "list");
            }
            else
            {
                var forms = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
                list = forms.ToList();
            }
            return GetFieldsFilterData(list.Where(a => a.F_DeleteMark == false).OrderBy(t => t.F_SortCode).ToList(), className.Substring(0, className.Length - 7));
        }
        public async Task<ItemsEntity> GetLookForm(string keyValue)
        {
            var cachedata =await service.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata, className.Substring(0, className.Length - 7));

        }
        public async Task<ItemsEntity> GetForm(string keyValue)
        {
            var cachedata = await service.CheckCache(cacheKey, keyValue);
            return cachedata;

        }
        public async Task DeleteForm(string keyValue)
        {
            if (service.IQueryable(t => t.F_ParentId.Equals(keyValue)).Count() > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                await service.Delete(t => t.F_Id == keyValue);
                await RedisHelper.DelAsync(cacheKey + keyValue);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
        }
        public async Task SubmitForm(ItemsEntity itemsEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                itemsEntity.Modify(keyValue);
                await service.Update(itemsEntity);
                await RedisHelper.DelAsync(cacheKey + keyValue);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
            else
            {
                itemsEntity.Create();
                await service.Insert(itemsEntity);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
        }
    }
}
