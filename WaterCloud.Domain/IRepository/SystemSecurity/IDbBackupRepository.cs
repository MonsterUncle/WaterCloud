/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System.Threading.Tasks;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemSecurity
{
    public interface IDbBackupRepository : IRepositoryBase<DbBackupEntity>
    {
        Task DeleteForm(string keyValue);
        Task ExecuteDbBackup(DbBackupEntity dbBackupEntity);
    }
}
