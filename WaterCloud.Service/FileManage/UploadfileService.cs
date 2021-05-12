using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.Domain.FileManage;
using WaterCloud.Domain.SystemOrganize;
using SqlSugar;
using WaterCloud.DataBase;

namespace WaterCloud.Service.FileManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-07-22 12:04
    /// 描 述：文件管理服务类
    /// </summary>
    public class UploadfileService : DataFilterService<UploadfileEntity>, IDenpendency
    {
        private string cacheKey = "watercloud_uploadfiledata_";
        
        public UploadfileService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #region 获取数据
        public async Task<List<UploadfileEntity>> GetList(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_FileName.Contains(keyword) || t.F_Description.Contains(keyword)).ToList();
            }
            return cachedata.OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public async Task<List<UploadfileEntity>> GetLookList(string keyword = "")
        {
            var query = GetQuery();
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(u => u.F_FileName.Contains(keyword) || u.F_Description.Contains(keyword));
            }
            query = GetDataPrivilege("u", "", query);
            var data = query.OrderBy(t => t.F_CreatorTime,OrderByType.Desc).ToList();
            foreach (var item in data)
            {
                string[] departments = item.F_OrganizeId.Split(',');
                item.F_OrganizeName = string.Join(',', repository.Db.Queryable<OrganizeEntity>().Where(a => departments.Contains(a.F_Id)).Select(a => a.F_FullName).ToList());
            }
            return data;
        }

        public async Task<List<UploadfileEntity>> GetLookList(SoulPage<UploadfileEntity> pagination, string keyword = "")
        {
            //反格式化显示只能用"等于"，其他不支持
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> enabledTemp = new Dictionary<string, string>();
            enabledTemp.Add("有效", "1");
            enabledTemp.Add("无效", "0");
            dic.Add("F_EnabledMark", enabledTemp);
            Dictionary<string, string> fileTypeTemp = new Dictionary<string, string>();
            fileTypeTemp.Add("图片", "1");
            fileTypeTemp.Add("文件", "0");
            dic.Add("F_FileType", fileTypeTemp);
            pagination = ChangeSoulData(dic, pagination);
            var query = GetQuery();
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(u => u.F_FileName.Contains(keyword) || u.F_Description.Contains(keyword));
            }
            //权限过滤
            query = GetDataPrivilege("u", "", query);
            var data = await repository.OrderList(query, pagination);
            var orgs = repository.Db.Queryable<OrganizeEntity>().ToList();
            foreach (var item in data)
            {
                string[] departments = item.F_OrganizeId.Split(',');
                item.F_OrganizeName = string.Join(',', orgs.Where(a => departments.Contains(a.F_Id)).Select(a => a.F_FullName).ToList());
            }
            return data;
        }
        private ISugarQueryable<UploadfileEntity> GetQuery()
        {
            var query = repository.Db.Queryable<UploadfileEntity, UserEntity>((a,b)=>new JoinQueryInfos(
                    JoinType.Left, a.F_CreatorUserId == b.F_Id))
                .Select((a, b) => new UploadfileEntity
                {
                    F_Id = a.F_Id.SelectAll(),
                    F_CreatorUserName =b.F_RealName,   
                }).MergeTable();
            return query;
        }
        public async Task<UploadfileEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        public async Task<UploadfileEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata);
        }
        #endregion

        #region 提交数据
        public async Task SubmitForm(UploadfileEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                    //此处需修改
                entity.Create();
                await repository.Insert(entity);
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                    //此处需修改
                entity.Modify(keyValue); 
                await repository.Update(entity);
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
        }

        public async Task DeleteForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            await repository.Delete(t => ids.Contains(t.F_Id));
            foreach (var item in ids)
            {
            await CacheHelper.Remove(cacheKey + item);
            }
            await CacheHelper.Remove(cacheKey + "list");
        }
        #endregion

    }
}
