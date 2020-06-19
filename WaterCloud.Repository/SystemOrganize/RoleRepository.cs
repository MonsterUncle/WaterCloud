/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemOrganize;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chloe;

namespace WaterCloud.Repository.SystemOrganize
{
    public class RoleRepository : RepositoryBase<RoleEntity>, IRoleRepository
    {
        private IDbContext dbcontext;
        public RoleRepository(IDbContext context) : base(context)
        {
            dbcontext = context;
        }
        public RoleRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            dbcontext = GetDbContext();
        }
        public async Task DeleteForm(string keyValue)
        {
            using (var db =new RepositoryBase(dbcontext).BeginTrans())
            {
                await db.Delete<RoleEntity>(t => t.F_Id == keyValue);
                await db.Delete<RoleAuthorizeEntity>(t => t.F_ObjectId == keyValue);
                db.Commit();
            }
        }
        public async Task SubmitForm(RoleEntity roleEntity, List<RoleAuthorizeEntity> roleAuthorizeEntitys, string keyValue)
        {
            using (var db =new RepositoryBase(dbcontext).BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    await db.Update(roleEntity);
                }
                else
                {
                    roleEntity.F_Category = 1;
                    await db.Insert(roleEntity);
                }
                await db.Delete<RoleAuthorizeEntity>(t => t.F_ObjectId == roleEntity.F_Id);
                await db.Insert(roleAuthorizeEntitys);
                db.Commit();
            }
        }
    }
}
