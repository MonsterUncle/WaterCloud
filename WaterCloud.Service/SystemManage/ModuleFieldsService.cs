using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Repository.SystemManage;

namespace WaterCloud.Service.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-05-21 14:38
    /// 描 述：字段管理服务类
    /// </summary>
    public class ModuleFieldsService :  IDenpendency
    {
        private IModuleFieldsRepository service = new ModuleFieldsRepository();
        private IModuleRepository moduleservice = new ModuleRepository();
        private string cacheKey = "watercloud_ modulefieldsdata_";
        private string initcacheKey = "watercloud_init_";
        private string authorizecacheKey = "watercloud_authorizeurldata_";// +权限
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

        public async Task<List<ModuleFieldsEntity>> GetList(Pagination pagination, string moduleId, string keyword = "")
        {
            var expression = ExtLinq.True<ModuleFieldsEntity>();
            expression = expression.And(t => t.F_ModuleId.Equals(moduleId));
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                expression = expression.And(t => t.F_FullName.Contains(keyword));
                expression = expression.Or(t => t.F_EnCode.Contains(keyword));
            }
            expression = expression.And(t => t.F_DeleteMark == false);
            return await service.FindList(expression, pagination);
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
                await RedisHelper.DelAsync(cacheKey + "list");
            }
            else
            {
                entity.Modify(keyValue); 
                await service.Update(entity);
                await RedisHelper.DelAsync(cacheKey + keyValue);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
            await RedisHelper.DelAsync(initcacheKey + "modulefields_" + "list");
        }

        public async Task DeleteForm(string keyValue)
        {
            await service.Delete(t => t.F_Id == keyValue);
            await RedisHelper.DelAsync(cacheKey + keyValue);
            await RedisHelper.DelAsync(cacheKey + "list");
            await RedisHelper.DelAsync(initcacheKey + "modulefields_" + "list");
            await RedisHelper.DelAsync(authorizecacheKey + "list");
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
            await RedisHelper.DelAsync(cacheKey + "list");
            await RedisHelper.DelAsync(initcacheKey + "modulefields_" + "list");
            await RedisHelper.DelAsync(authorizecacheKey + "list");
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
