/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Repository.SystemSecurity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WaterCloud.Service.SystemSecurity
{
    public class DbBackupService: IDenpendency
    {
        private IDbBackupRepository service = new DbBackupRepository();

        public async Task<List<DbBackupEntity>> GetList(string keyword)
        {
            var expression = ExtLinq.True<DbBackupEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_DbName.Contains(keyword));

            }
            return service.IQueryable(expression).OrderByDesc(t => t.F_BackupTime).ToList();
        }
        public async Task<DbBackupEntity> GetForm(string keyValue)
        {
            return await service.FindEntity(keyValue);
        }
        public async Task DeleteForm(string keyValue)
        {
            await service.DeleteForm(keyValue);
        }
        public async Task SubmitForm(DbBackupEntity dbBackupEntity)
        {
            dbBackupEntity.F_Id = Utils.GuId();
            dbBackupEntity.F_EnabledMark = true;
            dbBackupEntity.F_BackupTime = DateTime.Now;
            await service.ExecuteDbBackup(dbBackupEntity);
        }
    }
}
