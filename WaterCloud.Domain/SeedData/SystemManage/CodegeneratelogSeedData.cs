using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：条码生成记录种子
    /// </summary>
    public class CodegeneratelogSeedData : ISqlSugarEntitySeedData<CodegeneratelogEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<CodegeneratelogEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"08daa90a-d690-4c92-8f7d-a7c394d295a8\",\r\n        \"F_Code\": \"0009\",\r\n        \"F_RuleId\": \"08daa908-a7c1-4cc0-84ea-6fab3fa2bea6\",\r\n        \"F_RuleName\": \"测试\",\r\n        \"F_PrintJson\": \"{\\\"barid\\\":\\\"Company\\\",\\\"code\\\":\\\"Company\\\",\\\"name\\\":\\\"上海东鞋贸易有限公司\\\",\\\"rulecode\\\":\\\"0009\\\"}\",\r\n        \"F_PrintCount\": \"0\",\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_Description\": null,\r\n        \"F_CreatorTime\": \"2022-10-08 16:55:24\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_CreatorUserName\": null,\r\n        \"F_LastModifyTime\": null,\r\n        \"F_LastModifyUserId\": null,\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null\r\n    }\r\n]";
            return data.ToObject<List<CodegeneratelogEntity>>();
        }
    }
}