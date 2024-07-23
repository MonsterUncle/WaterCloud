using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemSecurity
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：IP限制种子
    /// </summary>
    public class FilterIPSeedData : ISqlSugarEntitySeedData<FilterIPEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<FilterIPEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"08da9c6c-ee80-4e93-884f-61a310860a89\",\r\n        \"F_Type\": 0,\r\n        \"F_StartIP\": \"128.0.0.2\",\r\n        \"F_EndIP\": \"\",\r\n        \"F_SortCode\": null,\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_Description\": \"\",\r\n        \"F_CreatorTime\": \"2022-09-22 15:34:49.8178932\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_LastModifyTime\": null,\r\n        \"F_LastModifyUserId\": null,\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null,\r\n        \"F_EndTime\": \"2022-09-22 00:00:00\"\r\n    }\r\n]";
            return data.ToObject<List<FilterIPEntity>>();
        }
    }
}