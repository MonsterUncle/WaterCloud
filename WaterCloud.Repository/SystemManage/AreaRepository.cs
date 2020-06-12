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
        private string ConnectStr;
        private string providerName;
        private DbContext dbcontext;
        public AreaRepository()
        {
            dbcontext = GetDbContext();
        }
        public AreaRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            this.ConnectStr = ConnectStr;
            this.providerName = providerName;
            dbcontext = GetDbContext();
        }
    }
}
