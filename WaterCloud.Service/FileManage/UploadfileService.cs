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
        public UploadfileService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #region 获取数据
        public async Task<List<UploadfileEntity>> GetList(string keyword = "")
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_FileName.Contains(keyword) || a.F_Description.Contains(keyword));
            }
            return await query.OrderBy(a => a.F_Id,OrderByType.Desc).ToListAsync();
        }

        public async Task<List<UploadfileEntity>> GetLookList(string keyword = "")
        {
            var query = GetQuery();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_FileName.Contains(keyword) || a.F_Description.Contains(keyword));
            }
            query = GetDataPrivilege("a", "", query);
            var data = await query.OrderBy(a => a.F_Id,OrderByType.Desc).ToListAsync();
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
            enabledTemp.Add("1", "有效");
            enabledTemp.Add("0", "无效");
            dic.Add("F_EnabledMark", enabledTemp);
            Dictionary<string, string> fileTypeTemp = new Dictionary<string, string>();
            fileTypeTemp.Add("1", "图片");
            fileTypeTemp.Add("0", "文件");
            dic.Add("F_FileType", fileTypeTemp);
            pagination = ChangeSoulData(dic, pagination);
            var query = GetQuery();
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(a => a.F_FileName.Contains(keyword) || a.F_Description.Contains(keyword));
            }
            //权限过滤
            query = GetDataPrivilege("a", "", query);
            var data = await query.ToPageListAsync(pagination); ;
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
                    F_Id = a.F_Id,
                    F_CreatorUserName = b.F_RealName,
                    F_CreatorTime = a.F_CreatorTime,
                    F_CreatorUserId = a.F_CreatorUserId,
                    F_Description = a.F_Description,
                    F_EnabledMark = a.F_EnabledMark,
                    F_FileExtension = a.F_FileExtension,
                    F_FileBy = a.F_FileBy,
                    F_FileName = a.F_FileName,
                    F_FilePath = a.F_FilePath,
                    F_FileSize = a.F_FileSize,
                    F_FileType = a.F_FileType,
                    F_OrganizeId = a.F_OrganizeId,
                }).MergeTable();
            return query;
        }
        public async Task<UploadfileEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        public async Task<UploadfileEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
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
            }
            else
            {
                    //此处需修改
                entity.Modify(keyValue); 
                await repository.Update(entity);
            }
        }

        public async Task DeleteForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            await repository.Delete(a => ids.Contains(a.F_Id));
        }
        #endregion

    }
}
