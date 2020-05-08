/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemSecurity;

namespace WaterCloud.Repository.SystemSecurity
{
    public class LogRepository : RepositoryBase<LogEntity>, ILogRepository
    {
        private string ConnectStr;
        private string providerName;
        public LogRepository()
        {
        }
        public LogRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            this.ConnectStr = ConnectStr;
            this.providerName = providerName;
        }
    }
}
