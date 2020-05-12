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

namespace WaterCloud.Repository.SystemManage
{
    public class RoleRepository : RepositoryBase<RoleEntity>, IRoleRepository
    {
        private string ConnectStr;
        private string providerName;
        public RoleRepository()
        {
        }
        public RoleRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            this.ConnectStr = ConnectStr;
            this.providerName = providerName;
        }
        public async Task DeleteForm(string keyValue)
        {
            using (var db =new RepositoryBase(ConnectStr, providerName).BeginTrans())
            {
                await db.Delete<RoleEntity>(t => t.F_Id == keyValue);
                await db.Delete<RoleAuthorizeEntity>(t => t.F_ObjectId == keyValue);
                db.Commit();
            }
        }
        public async Task SubmitForm(RoleEntity roleEntity, List<RoleAuthorizeEntity> roleAuthorizeEntitys, string keyValue)
        {
            using (var db =new RepositoryBase(ConnectStr, providerName).BeginTrans())
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
