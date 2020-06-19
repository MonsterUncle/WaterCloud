/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using Chloe;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.Repository.SystemManage
{
    public class AreaRepository : RepositoryBase<AreaEntity>, IAreaRepository
    {
        private IDbContext dbcontext;
        public AreaRepository(IDbContext context) : base(context)
        {
            dbcontext = context;
        }
        public AreaRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            dbcontext = GetDbContext();
        }
    }
}
