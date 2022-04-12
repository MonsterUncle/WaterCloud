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
                query = query.Where(a => a.FileName.Contains(keyword) || a.Description.Contains(keyword));
            }
            return await query.OrderBy(a => a.Id,OrderByType.Desc).ToListAsync();
        }

        public async Task<List<UploadfileEntity>> GetLookList(string keyword = "")
        {
            var query = GetQuery();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.FileName.Contains(keyword) || a.Description.Contains(keyword));
            }
            query = GetDataPrivilege("a", "", query);
            var data = await query.OrderBy(a => a.Id,OrderByType.Desc).ToListAsync();
            foreach (var item in data)
            {
                string[] departments = item.OrganizeId.Split(',');
                item.OrganizeName = string.Join(',', repository.Db.Queryable<OrganizeEntity>().Where(a => departments.Contains(a.Id)).Select(a => a.FullName).ToList());
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
            dic.Add("EnabledMark", enabledTemp);
            Dictionary<string, string> fileTypeTemp = new Dictionary<string, string>();
            fileTypeTemp.Add("1", "图片");
            fileTypeTemp.Add("0", "文件");
            dic.Add("FileType", fileTypeTemp);
            pagination = ChangeSoulData(dic, pagination);
            var query = GetQuery();
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(a => a.FileName.Contains(keyword) || a.Description.Contains(keyword));
            }
            //权限过滤
            query = GetDataPrivilege("a", "", query);
            var data = await query.ToPageListAsync(pagination); ;
            var orgs = repository.Db.Queryable<OrganizeEntity>().ToList();
            foreach (var item in data)
            {
                string[] departments = item.OrganizeId.Split(',');
                item.OrganizeName = string.Join(',', orgs.Where(a => departments.Contains(a.Id)).Select(a => a.FullName).ToList());
            }
            return data;
        }
        private ISugarQueryable<UploadfileEntity> GetQuery()
        {
            var query = repository.Db.Queryable<UploadfileEntity, UserEntity>((a,b)=>new JoinQueryInfos(
                    JoinType.Left, a.CreatorUserId == b.Id))
                .Select((a, b) => new UploadfileEntity
                {
                    Id = a.Id,
                    CreatorUserName = b.RealName,
                    CreatorTime = a.CreatorTime,
                    CreatorUserId = a.CreatorUserId,
                    Description = a.Description,
                    EnabledMark = a.EnabledMark,
                    FileExtension = a.FileExtension,
                    FileBy = a.FileBy,
                    FileName = a.FileName,
                    FilePath = a.FilePath,
                    FileSize = a.FileSize,
                    FileType = a.FileType,
                    OrganizeId = a.OrganizeId,
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
            await repository.Delete(a => ids.Contains(a.Id));
        }
        #endregion

    }
}
