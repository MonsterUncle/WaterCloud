/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using Chloe;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemSecurity;

namespace WaterCloud.Repository.SystemSecurity
{
    public class OpenJobRepository : RepositoryBase<OpenJobEntity>, IOpenJobRepository
    {
        private IDbContext dbcontext;
        public OpenJobRepository(IDbContext context) : base(context)
        {
            dbcontext = context;
        }
        public OpenJobRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            dbcontext = GetDbContext();
        }
    }
}
