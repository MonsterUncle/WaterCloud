using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.OrderManagement
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：订单种子
    /// </summary>
    public class OrderSeedData : ISqlSugarEntitySeedData<OrderEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<OrderEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"08d9459c-5cd7-453f-8240-d566f1fe058c\",\r\n        \"F_OrderCode\": \"OR-20210713091957\",\r\n        \"F_OrderState\": \"1\",\r\n        \"F_NeedTime\": \"2021-07-13 00:00:00\",\r\n        \"F_ActualTime\": \"2021-07-13 00:00:00\",\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_Description\": \"\",\r\n        \"F_CreatorTime\": \"2021-07-13 09:20:12\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_CreatorUserName\": \"超级管理员\",\r\n        \"F_LastModifyTime\": \"2021-07-13 09:29:45\",\r\n        \"F_LastModifyUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null\r\n    },\r\n    {\r\n        \"F_Id\": \"08da83f4-06f1-409f-8069-cb99fbebed5a\",\r\n        \"F_OrderCode\": \"OR-20220822120803\",\r\n        \"F_OrderState\": \"0\",\r\n        \"F_NeedTime\": \"2022-08-23 00:00:00\",\r\n        \"F_ActualTime\": null,\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_Description\": \"\",\r\n        \"F_CreatorTime\": \"2022-08-22 04:08:53.8066641\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_CreatorUserName\": \"超级管理员\",\r\n        \"F_LastModifyTime\": null,\r\n        \"F_LastModifyUserId\": null,\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null\r\n    }\r\n]";
            return data.ToObject<List<OrderEntity>>();
        }
    }
}