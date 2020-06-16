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
    public class OrganizeRepository : RepositoryBase<OrganizeEntity>, IOrganizeRepository
    {
        private string ConnectStr;
        private string providerName;
        private DbContext dbcontext;
        public OrganizeRepository()
        {
            dbcontext = GetDbContext();
        }
        public OrganizeRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            this.ConnectStr = ConnectStr;
            this.providerName = providerName;
            dbcontext = GetDbContext();
        }
    }
}
