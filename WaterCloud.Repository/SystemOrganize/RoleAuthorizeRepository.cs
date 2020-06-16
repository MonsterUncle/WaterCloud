/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using Chloe;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Repository.SystemOrganize
{
    public class RoleAuthorizeRepository : RepositoryBase<RoleAuthorizeEntity>, IRoleAuthorizeRepository
    {
        private string ConnectStr;
        private string providerName;
        private DbContext dbcontext;
        public RoleAuthorizeRepository()
        {
            dbcontext = GetDbContext();
        }
        public RoleAuthorizeRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            this.ConnectStr = ConnectStr;
            this.providerName = providerName;
            dbcontext = GetDbContext();
        }
    }
}
