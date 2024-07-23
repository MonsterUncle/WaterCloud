using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：条码种子
    /// </summary>
    public class CoderuleSeedData : ISqlSugarEntitySeedData<CoderuleEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<CoderuleEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"08daa908-a7c1-4cc0-84ea-6fab3fa2bea6\",\r\n        \"F_RuleName\": \"测试\",\r\n        \"F_RuleJson\": \"[{\\\"Id\\\":\\\"1668a472-e5d2-412e-8879-393b1cbc59a4\\\",\\\"FormatType\\\":11,\\\"FormatString\\\":\\\"0000\\\",\\\"ToBase\\\":10,\\\"InitValue\\\":1,\\\"MaxValue\\\":-1,\\\"Increment\\\":1,\\\"DiyDate\\\":null,\\\"LAY_TABLE_INDEX\\\":0}]\",\r\n        \"F_Reset\": \"yyyy-MM-dd\",\r\n        \"F_TemplateId\": \"08daa778-f508-4fd6-8f60-e60cb414b464\",\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_Description\": null,\r\n        \"F_CreatorTime\": \"2022-10-08 16:39:47\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_CreatorUserName\": null,\r\n        \"F_LastModifyTime\": null,\r\n        \"F_LastModifyUserId\": null,\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null\r\n    }\r\n]";
            return data.ToObject<List<CoderuleEntity>>();
        }
    }
}