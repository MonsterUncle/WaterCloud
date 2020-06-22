using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Repository.SystemManage;
using Chloe;

namespace WaterCloud.Service.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-05-21 14:38
    /// 描 述：字段管理服务类
    /// </summary>
    public class ModuleFieldsService : DataFilterService<ModuleFieldsEntity>, IDenpendency
    {
        private IModuleFieldsRepository service;
        private IModuleRepository moduleservice;
        private string cacheKey = "watercloud_ modulefieldsdata_";
        private string initcacheKey = "watercloud_init_";
        private string authorizecacheKey = "watercloud_authorizeurldata_";// +权限
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public ModuleFieldsService(IDbContext context, string apitoken = "") : base(context, apitoken)
        {
            var currentuser = OperatorProvider.Provider.GetCurrent(apitoken);
            service = currentuser != null&&!(currentuser.DBProvider == GlobalContext.SystemConfig.DBProvider&&currentuser.DbString == GlobalContext.SystemConfig.DBConnectionString) ? new ModuleFieldsRepository(currentuser.DbString,currentuser.DBProvider) : new ModuleFieldsRepository(context);
            moduleservice = currentuser != null&&!(currentuser.DBProvider == GlobalContext.SystemConfig.DBProvider&&currentuser.DbString == GlobalContext.SystemConfig.DBConnectionString) ? new ModuleRepository(currentuser.DbString,currentuser.DBProvider) : new ModuleRepository(context);
        }
        #region 获取数据
        public async Task<List<ModuleFieldsEntity>> GetList(string keyword = "")
        {
            var cachedata = await service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_FullName.Contains(keyword) || t.F_EnCode.Contains(keyword)).ToList();
            }
            return cachedata.Where(a=>a.F_DeleteMark==false).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public async Task<List<ModuleFieldsEntity>> GetLookList(Pagination pagination, string moduleId, string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(u => u.F_FullName.Contains(keyword) || u.F_EnCode.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark == false&&u.F_ModuleId== moduleId);
            return GetFieldsFilterData(await service.OrderList(list, pagination), className.Substring(0, className.Length - 7));

        }

        public async Task<ModuleFieldsEntity> GetLookForm(string keyValue)
        {
            var cachedata = await service.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata, className.Substring(0, className.Length - 7));
        }
        public async Task<ModuleFieldsEntity> GetForm(string keyValue)
        {
            var cachedata = await service.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        #endregion

        #region 提交数据
        public async Task SubmitForm(ModuleFieldsEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.Create();
                await service.Insert(entity);
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                entity.Modify(keyValue); 
                await service.Update(entity);
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
            await CacheHelper.Remove(initcacheKey + "modulefields_" + "list");
        }

        public async Task DeleteForm(string keyValue)
        {
            await service.Delete(t => t.F_Id == keyValue);
            await CacheHelper.Remove(cacheKey + keyValue);
            await CacheHelper.Remove(cacheKey + "list");
            await CacheHelper.Remove(initcacheKey + "modulefields_" + "list");
            await CacheHelper.Remove(authorizecacheKey + "list");
        }

        public async Task SubmitCloneFields(string moduleId, string ids)
        {
            string[] ArrayId = ids.Split(',');
            var data = await this.GetList();
            List<ModuleFieldsEntity> entitys = new List<ModuleFieldsEntity>();
            var module = await moduleservice.FindEntity(a => a.F_Id == moduleId);
            if (string.IsNullOrEmpty(module.F_UrlAddress) || module.F_Target != "iframe")
            {
                throw new Exception("框架页才能创建按钮");
            }
            foreach (string item in ArrayId)
            {
                ModuleFieldsEntity moduleFieldsEntity = data.Find(t => t.F_Id == item);
                moduleFieldsEntity.Create();
                moduleFieldsEntity.F_ModuleId = moduleId;
                entitys.Add(moduleFieldsEntity);
            }
            await service.Insert(entitys);
            await CacheHelper.Remove(cacheKey + "list");
            await CacheHelper.Remove(initcacheKey + "modulefields_" + "list");
            await CacheHelper.Remove(authorizecacheKey + "list");
        }

        public async Task<List<ModuleFieldsEntity>> GetListByRole(string roleid)
        {
            return await service.GetListByRole(roleid);
        }

        internal async Task<List<ModuleFieldsEntity>> GetListNew(string moduleId="")
        {
            return await service.GetListNew(moduleId);
        }
        #endregion

    }
}
