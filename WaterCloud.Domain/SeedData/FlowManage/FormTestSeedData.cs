using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.FlowManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：工作流测试数据种子
    /// </summary>
    public class FormTestSeedData : ISqlSugarEntitySeedData<FormTestEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<FormTestEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"39fcc2eb-95c4-ce62-2429-46be05704627\",\r\n        \"F_UserName\": \"222233\",\r\n        \"F_RequestType\": \"事假\",\r\n        \"F_StartTime\": \"2021-05-28 00:00:00\",\r\n        \"F_EndTime\": \"2021-05-28 00:00:00\",\r\n        \"F_RequestComment\": \"3333\",\r\n        \"F_Attachment\": \"\",\r\n        \"F_CreatorTime\": \"2021-05-28 10:12:41\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_CreatorUserName\": \"超级管理员\",\r\n        \"F_FlowInstanceId\": \"39fcc2eb-9367-b566-9078-9f411bc4f050\"\r\n    },\r\n    {\r\n        \"F_Id\": \"08daf4c8-f927-401a-855c-b3dc7d0c0526\",\r\n        \"F_UserName\": \"3333\",\r\n        \"F_RequestType\": \"事假\",\r\n        \"F_StartTime\": \"2023-01-13 02:14:00\",\r\n        \"F_EndTime\": \"2023-01-13 23:59:00\",\r\n        \"F_RequestComment\": \"333\",\r\n        \"F_Attachment\": \"\",\r\n        \"F_CreatorTime\": \"2023-01-13 02:15:23.7069704\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_CreatorUserName\": \"超级管理员\",\r\n        \"F_FlowInstanceId\": \"08daf4c8-f13b-42cc-83b8-dd3835c6c1a0\"\r\n    },\r\n    {\r\n        \"F_Id\": \"08daf511-391e-49f1-8735-45afaa232f75\",\r\n        \"F_UserName\": \"QWH\",\r\n        \"F_RequestType\": \"0\",\r\n        \"F_StartTime\": null,\r\n        \"F_EndTime\": null,\r\n        \"F_RequestComment\": null,\r\n        \"F_Attachment\": null,\r\n        \"F_CreatorTime\": \"2023-01-13 10:52:34.7903575\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_CreatorUserName\": null,\r\n        \"F_FlowInstanceId\": null\r\n    },\r\n    {\r\n        \"F_Id\": \"08daf513-e9fc-45fc-871a-b3c19d7cb39f\",\r\n        \"F_UserName\": \"KKKK\",\r\n        \"F_RequestType\": \"0\",\r\n        \"F_StartTime\": null,\r\n        \"F_EndTime\": null,\r\n        \"F_RequestComment\": null,\r\n        \"F_Attachment\": null,\r\n        \"F_CreatorTime\": \"2023/01/13 11:11:50\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_CreatorUserName\": \"超级管理员\",\r\n        \"F_FlowInstanceId\": null\r\n    }\r\n]";
            return data.ToObject<List<FormTestEntity>>();
        }
    }
}