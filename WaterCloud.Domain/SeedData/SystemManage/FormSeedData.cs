using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：表单种子
    /// </summary>
    public class FormSeedData : ISqlSugarEntitySeedData<FormEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<FormEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"08d92a58-531c-43ec-8546-d359e0f81347\",\r\n        \"F_Name\": \"333\",\r\n        \"F_FrmType\": \"0\",\r\n        \"F_WebId\": \"\",\r\n        \"F_Fields\": \"2\",\r\n        \"F_ContentData\": \"F_UserName,F_RequestType\",\r\n        \"F_ContentParse\": null,\r\n        \"F_Content\": \"[{\\\"id\\\":\\\"F_UserName\\\",\\\"index\\\":0,\\\"label\\\":\\\"用户\\\",\\\"tag\\\":\\\"input\\\",\\\"tagIcon\\\":\\\"input\\\",\\\"placeholder\\\":\\\"请输入\\\",\\\"defaultValue\\\":null,\\\"labelWidth\\\":110,\\\"width\\\":\\\"100%\\\",\\\"clearable\\\":true,\\\"maxlength\\\":null,\\\"showWordLimit\\\":false,\\\"readonly\\\":false,\\\"disabled\\\":false,\\\"required\\\":true,\\\"expression\\\":\\\"\\\",\\\"document\\\":\\\"\\\"},{\\\"id\\\":\\\"F_RequestType\\\",\\\"index\\\":1,\\\"label\\\":\\\"请假类型\\\",\\\"tag\\\":\\\"radio\\\",\\\"tagIcon\\\":\\\"radio\\\",\\\"labelWidth\\\":110,\\\"disabled\\\":false,\\\"document\\\":\\\"\\\",\\\"datasourceType\\\":\\\"local\\\",\\\"remoteUrl\\\":\\\"http://\\\",\\\"remoteMethod\\\":\\\"post\\\",\\\"remoteOptionText\\\":\\\"options.data.dictName\\\",\\\"remoteOptionValue\\\":\\\"options.data.dictId\\\",\\\"options\\\":[{\\\"text\\\":\\\"事假\\\",\\\"value\\\":\\\"0\\\",\\\"checked\\\":true},{\\\"text\\\":\\\"病假\\\",\\\"value\\\":\\\"1\\\",\\\"checked\\\":false}]}]\",\r\n        \"F_SortCode\": \"33\",\r\n        \"F_EnabledMark\": 1,\r\n        \"F_DeleteMark\": 0,\r\n        \"F_CreatorTime\": \"2021-06-08 16:35:09\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_LastModifyTime\": \"2023-01-13 10:33:07.2480064\",\r\n        \"F_LastModifyUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null,\r\n        \"F_Description\": \"\",\r\n        \"F_OrganizeId\": \"\",\r\n        \"F_DbName\": \"oms_formtest\"\r\n    },\r\n    {\r\n        \"F_Id\": \"8faff4e5-b729-44d2-ac26-e899a228f63d\",\r\n        \"F_Name\": \"系统内置的复杂请假条表单\",\r\n        \"F_FrmType\": \"1\",\r\n        \"F_WebId\": \"FormTest\",\r\n        \"F_Fields\": \"11\",\r\n        \"F_ContentData\": \"F_Id,F_UserName,F_RequestType,F_StartTime,F_EndTime,F_RequestComment,F_Attachment,F_FlowInstanceId,F_CreatorTime,F_CreatorUserId,F_CreatorUserName\",\r\n        \"F_ContentParse\": \"\",\r\n        \"F_Content\": \"\",\r\n        \"F_SortCode\": \"0\",\r\n        \"F_EnabledMark\": 1,\r\n        \"F_DeleteMark\": 0,\r\n        \"F_CreatorTime\": \"2019-07-29 01:03:36\",\r\n        \"F_CreatorUserId\": \"6ba79766-faa0-4259-8139-a4a6d35784e0\",\r\n        \"F_LastModifyTime\": \"2022-07-19 03:08:11.9246435\",\r\n        \"F_LastModifyUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_DeleteTime\": \"2019-07-29 01:03:36\",\r\n        \"F_DeleteUserId\": \"6ba79766-faa0-4259-8139-a4a6d35784e0\",\r\n        \"F_Description\": \"企业版内置的复杂请假条表单\",\r\n        \"F_OrganizeId\": \"\",\r\n        \"F_DbName\": null\r\n    }\r\n]";
            return data.ToObject<List<FormEntity>>();
        }
    }
}