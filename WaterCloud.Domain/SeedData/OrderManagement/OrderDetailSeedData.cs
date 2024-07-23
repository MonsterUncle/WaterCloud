using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.OrderManagement
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：订单明细种子
    /// </summary>
    public class OrderDetailSeedData : ISqlSugarEntitySeedData<OrderDetailEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<OrderDetailEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"08d9459d-b222-4ad5-8e4e-c5153e69a752\",\r\n        \"F_OrderId\": \"08d9459c-5cd7-453f-8240-d566f1fe058c\",\r\n        \"F_OrderState\": \"1\",\r\n        \"F_ProductName\": \"222\",\r\n        \"F_ProductDescription\": \"\",\r\n        \"F_ProductUnit\": \"\",\r\n        \"F_NeedNum\": \"3\",\r\n        \"F_ActualNum\": \"2\",\r\n        \"F_Description\": \"\",\r\n        \"F_CreatorTime\": null,\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_CreatorUserName\": \"超级管理员\",\r\n        \"F_NeedTime\": \"2021-07-13 00:00:00\",\r\n        \"F_ActualTime\": \"2021-07-13 00:00:00\"\r\n    },\r\n    {\r\n        \"F_Id\": \"08da83f4-06f1-40f6-8af1-c756eb6c7b8d\",\r\n        \"F_OrderId\": \"08da83f4-06f1-409f-8069-cb99fbebed5a\",\r\n        \"F_OrderState\": \"0\",\r\n        \"F_ProductName\": \"11222\",\r\n        \"F_ProductDescription\": \"889465-6546\",\r\n        \"F_ProductUnit\": \"PC\",\r\n        \"F_NeedNum\": \"0\",\r\n        \"F_ActualNum\": \"0\",\r\n        \"F_Description\": \"\",\r\n        \"F_CreatorTime\": \"2022-08-22 04:08:53.8066641\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_CreatorUserName\": \"超级管理员\",\r\n        \"F_NeedTime\": \"2022-08-23 00:00:00\",\r\n        \"F_ActualTime\": \"2022-08-22 00:00:00\"\r\n    }\r\n]";
            return data.ToObject<List<OrderDetailEntity>>();
        }
    }
}