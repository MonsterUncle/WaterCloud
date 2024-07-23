using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemSecurity
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：操作log种子
    /// </summary>
    public class LogSeedData : ISqlSugarEntitySeedData<LogEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<LogEntity> SeedData()
        {
            var data = "";
            return data.ToObject<List<LogEntity>>();
        }
    }
}