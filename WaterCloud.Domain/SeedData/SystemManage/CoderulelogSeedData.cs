using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：条码计数实体log
    /// </summary>
    public class CoderulelogSeedData : ISqlSugarEntitySeedData<CoderulelogEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<CoderulelogEntity> SeedData()
        {
            var data = "";
            return data.ToObject<List<CoderulelogEntity>>();
        }
    }
}