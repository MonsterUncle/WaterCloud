using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Repository.SystemOrganize;
using Chloe;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-12 13:50
    /// 描 述：系统设置服务类
    /// </summary>
    public class SystemSetService : DataFilterService<SystemSetEntity>, IDenpendency
    {
        private ISystemSetRepository service;
        private string cacheKey = "watercloud_systemsetdata_";
        private string cacheKeyOperator = "watercloud_operator_";// +登录者token
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public SystemSetService(IDbContext context, string apitoken = "") : base(context, apitoken)
        {
            service = new SystemSetRepository(context);
        }
        #region 获取数据
        public async Task<List<SystemSetEntity>> GetList(string keyword = "")
        {
            var cachedata = await service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_CompanyName.Contains(keyword) || t.F_ProjectName.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public async Task<List<SystemSetEntity>> GetLookList(string keyword = "")
        {
            var list =new List<SystemSetEntity>();
            if (!CheckDataPrivilege(className.Substring(0, className.Length - 7)))
            {
                list = await service.CheckCacheList(cacheKey + "list");
            }
            else
            {
                var forms = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
                list = forms.ToList();
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.F_CompanyName.Contains(keyword) || u.F_ProjectName.Contains(keyword)).ToList();
            }
            return GetFieldsFilterData(list.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList(),className.Substring(0, className.Length - 7));
        }

        public async Task<SystemSetEntity> GetFormByHost(string host)
        {
            var cachedata = await service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(host))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_HostUrl.Contains(host)).ToList();
            }
            else
            {
                cachedata = cachedata.Where(t => t.F_Id==Define.SYSTEM_MASTERPROJECT).ToList();
            }
            if (cachedata.Count==0)
            {
                cachedata = await service.CheckCacheList(cacheKey + "list");
                cachedata = cachedata.Where(t => t.F_Id == Define.SYSTEM_MASTERPROJECT).ToList();
            }
            return cachedata.Where(t => t.F_DeleteMark == false).FirstOrDefault();
        }

        public async Task<List<SystemSetEntity>> GetLookList(Pagination pagination,string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.F_CompanyName.Contains(keyword) || u.F_ProjectName.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark==false);
            return GetFieldsFilterData(await service.OrderList(list, pagination),className.Substring(0, className.Length - 7));
        }

        public async Task<SystemSetEntity> GetForm(string keyValue)
        {
            var cachedata = await service.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        #endregion

        public async Task<SystemSetEntity> GetLookForm(string keyValue)
        {
            var cachedata = await service.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata,className.Substring(0, className.Length - 7));
        }

        #region 提交数据
        public async Task SubmitForm(SystemSetEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                    //此处需修改
                entity.Create();
                await service.Insert(entity);
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                    //此处需修改
                entity.Modify(keyValue);
                await service.UpdateForm(entity);
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
            var set=await service.FindEntity(entity.F_Id);
            var tempkey=new UserRepository(DBContexHelper.Contex(set.F_DbString, set.F_DBProvider)).IQueryable().Where(a => a.F_IsAdmin == true && a.F_OrganizeId == keyValue).FirstOrDefault().F_Id;
            await CacheHelper.Remove(cacheKeyOperator + "info_" + tempkey);
        }

        public async Task DeleteForm(string keyValue)
        {
            await service.Delete(t => t.F_Id == keyValue);
            await CacheHelper.Remove(cacheKey + keyValue);
            await CacheHelper.Remove(cacheKey + "list");
        }
        #endregion

    }
}
