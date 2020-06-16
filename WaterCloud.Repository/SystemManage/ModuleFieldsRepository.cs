using Chloe;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Repository.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-05-21 14:38
    /// 描 述：字段管理数据实现类
    /// </summary>
    public class ModuleFieldsRepository : RepositoryBase<ModuleFieldsEntity>,IModuleFieldsRepository
    {
        private string ConnectStr;
        private string providerName;
        private DbContext dbcontext;
        public ModuleFieldsRepository()
        {
            dbcontext = GetDbContext();
        }
        public ModuleFieldsRepository(string ConnectStr, string providerName)
             : base(ConnectStr, providerName)
        {
            this.ConnectStr = ConnectStr;
            this.providerName = providerName;
            dbcontext = GetDbContext();
        }

        public async Task<List<ModuleFieldsEntity>> GetListByRole(string roleid)
        {
            using (var db = new RepositoryBase(ConnectStr, providerName))
            {
                var moduleList = db.IQueryable<RoleAuthorizeEntity>(a => a.F_ObjectId == roleid && a.F_ItemType == 3).Select(a => a.F_ItemId).ToList();
                var query = db.IQueryable<ModuleFieldsEntity>().Where(a => (moduleList.Contains(a.F_Id)) && a.F_EnabledMark == true);
                var result = query.OrderByDesc(a => a.F_CreatorTime).ToList();
                return result;
            }
        }

        public async Task<List<ModuleFieldsEntity>> GetListNew(string moduleId)
        {
            using (var db = new RepositoryBase(ConnectStr, providerName))
            {
                var query = db.IQueryable<ModuleFieldsEntity>(a => a.F_EnabledMark == true)
                    .InnerJoin<ModuleEntity>((a, b) => a.F_ModuleId == b.F_Id && b.F_EnabledMark == true)
                    .Select((a, b) => new ModuleFieldsEntity
                    {
                        F_Id = a.F_Id,
                        F_CreatorTime = a.F_CreatorTime,
                        F_CreatorUserId = a.F_CreatorUserId,
                        F_DeleteMark = a.F_DeleteMark,
                        F_DeleteTime = a.F_DeleteTime,
                        F_DeleteUserId = a.F_DeleteUserId,
                        F_Description = a.F_Description,
                        F_EnabledMark = a.F_EnabledMark,
                        F_EnCode = a.F_EnCode,
                        F_FullName = a.F_FullName,
                        F_LastModifyTime = a.F_LastModifyTime,
                        F_LastModifyUserId = a.F_LastModifyUserId,
                        F_ModuleId = b.F_UrlAddress,
                        F_IsPublic=a.F_IsPublic
                    });
                if (!string.IsNullOrEmpty(moduleId))
                {
                    query = query.Where(a => a.F_ModuleId == moduleId);
                }
                return query.OrderByDesc(a => a.F_CreatorTime).ToList();
            }
        }
    }
}
