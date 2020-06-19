/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chloe;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Repository.SystemManage
{
    public class ModuleButtonRepository : RepositoryBase<ModuleButtonEntity>, IModuleButtonRepository
    {
        private IDbContext dbcontext;
        public ModuleButtonRepository(IDbContext context) : base(context)
        {
            dbcontext = context;
        }
        public ModuleButtonRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            dbcontext = GetDbContext();
        }
        public async Task<List<ModuleButtonEntity>> GetListByRole(string roleid)
        {
            //1、使用框架方法
            //using (var db =new RepositoryBase(ConnectStr, providerName))
            //{
            //    var moduleList = db.IQueryable<RoleAuthorizeEntity>(a => a.F_ObjectId == roleid && a.F_ItemType == 2).Select(a => a.F_ItemId).ToList();
            //    var query = db.IQueryable<ModuleButtonEntity>().Where(a => (moduleList.Contains(a.F_Id)||a.F_IsPublic==true) &&a.F_DeleteMark==false&& a.F_EnabledMark == true);
            //    var result = query.OrderBy(a => a.F_SortCode).ToList();
            //    return result;
            //}
            //2、使用chloe自身方法
            var moduleList = dbcontext.Query<RoleAuthorizeEntity>(a => a.F_ObjectId == roleid && a.F_ItemType == 2).Select(a => a.F_ItemId).ToList();
            var query = dbcontext.Query<ModuleButtonEntity>().Where(a => (moduleList.Contains(a.F_Id) || a.F_IsPublic == true) && a.F_DeleteMark == false && a.F_EnabledMark == true);
            var result = query.OrderBy(a => a.F_SortCode).ToList();
            return result;

        }

        public async Task<List<ModuleButtonEntity>> GetListNew(string moduleId)
        {
            using (var db = new RepositoryBase(dbcontext))
            {
                var query = db.IQueryable<ModuleButtonEntity>(a=>a.F_EnabledMark==true && a.F_DeleteMark == false)
                    .InnerJoin<ModuleEntity>((a, b) => a.F_ModuleId == b.F_Id&& b.F_EnabledMark == true && a.F_DeleteMark == false)
                    .Select((a, b) => new ModuleButtonEntity { 
                    F_Id=a.F_Id,
                    F_AllowDelete=a.F_AllowDelete,
                    F_AllowEdit=a.F_AllowEdit,
                    F_UrlAddress=a.F_UrlAddress,
                    F_CreatorTime=a.F_CreatorTime,
                    F_CreatorUserId=a.F_CreatorUserId,
                    F_DeleteMark=a.F_DeleteMark,
                    F_DeleteTime=a.F_DeleteTime,
                    F_DeleteUserId=a.F_DeleteUserId,
                    F_Description=a.F_Description,
                    F_EnabledMark=a.F_EnabledMark,
                    F_EnCode=a.F_EnCode,
                    F_FullName=a.F_FullName,
                    F_Icon=a.F_Icon,
                    F_IsPublic=a.F_IsPublic,
                    F_JsEvent=a.F_JsEvent,
                    F_LastModifyTime=a.F_LastModifyTime,
                    F_LastModifyUserId=a.F_LastModifyUserId,
                    F_Layers=a.F_Layers,
                    F_Location=a.F_Location,
                    F_ModuleId=b.F_UrlAddress,
                    F_ParentId=a.F_ParentId,
                    F_SortCode=a.F_SortCode,
                    F_Split=a.F_Split,                                        
                    });
                if (!string.IsNullOrEmpty(moduleId))
                {
                    query = query.Where(a => a.F_ModuleId == moduleId);
                }
                return query.OrderBy(a=>a.F_SortCode).ToList();
            }
        }
    }
}
