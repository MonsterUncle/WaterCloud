/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.Repository.SystemManage
{
    public class AreaRepository : RepositoryBase<AreaEntity>, IAreaRepository
    {
        private string ConnectStr;
        private string providerName;
        public AreaRepository()
        {
        }
        public AreaRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            this.ConnectStr = ConnectStr;
            this.providerName = providerName;
        }
    }
}
