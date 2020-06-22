/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Domain.SystemManage;
using WaterCloud.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCloud.Code;
using Chloe;

namespace WaterCloud.Service.SystemManage
{
    public class AreaService : DataFilterService<AreaEntity>, IDenpendency
    {
        private IAreaRepository service;
        public AreaService(IDbContext context, string apitoken="") : base(context,apitoken)
        {
            //根据租户选择数据库连接
            var currentuser = OperatorProvider.Provider.GetCurrent(apitoken);
            service = currentuser != null&&!(currentuser.DBProvider == GlobalContext.SystemConfig.DBProvider&&currentuser.DbString == GlobalContext.SystemConfig.DBConnectionString) ? new AreaRepository(currentuser.DbString, currentuser.DBProvider) : new AreaRepository(context);

        }
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private string cacheKey = "watercloud_areadata_";// 区域
        public async Task<List<AreaEntity>> GetList(int layers = 0)
        {
            var list = new List<AreaEntity>();
            list = await service.CheckCacheList(cacheKey + "list");
            if (layers != 0)
            {
                list = list.Where(t => t.F_Layers == layers).ToList();
            }
            return list.Where(t => t.F_DeleteMark == false && t.F_EnabledMark == true).OrderBy(t => t.F_SortCode).ToList();
        }
        public async Task<List<AreaEntity>> GetLookList(int layers=0)
        {
            var list =new List<AreaEntity>();
            if (!CheckDataPrivilege(className.Substring(0, className.Length - 7)))
            {
                list = await service.CheckCacheList(cacheKey + "list");
            }
            else
            {
                var forms = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
                list = forms.ToList();
            }
            if (layers!=0)
            { 
                list = list.Where(t => t.F_Layers == layers).ToList();
            }
            return GetFieldsFilterData(list.Where(t => t.F_DeleteMark == false && t.F_EnabledMark == true).OrderBy(t => t.F_SortCode).ToList(), className.Substring(0, className.Length - 7));
        }
        public async Task<AreaEntity> GetLookForm(string keyValue)
        {
            var cachedata =await service.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata, className.Substring(0, className.Length - 7));
        }
        public async Task<AreaEntity> GetForm(string keyValue)
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
            }
            await CacheHelper.Remove(cacheKey + keyValue);
            await CacheHelper.Remove(cacheKey + "list");
        }
        public async Task SubmitForm(AreaEntity mEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                mEntity.Modify(keyValue);
                await service.Update(mEntity);
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                mEntity.Create();
                await service.Insert(mEntity);
                await CacheHelper.Remove(cacheKey + "list");
            }
        }
    }
}
