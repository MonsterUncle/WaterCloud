using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：快捷菜单种子
    /// </summary>
    public class QuickModuleSeedData : ISqlSugarEntitySeedData<QuickModuleEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<QuickModuleEntity> SeedData()
        {
            var data = "";
            return data.ToObject<List<QuickModuleEntity>>();
        }
    }
}