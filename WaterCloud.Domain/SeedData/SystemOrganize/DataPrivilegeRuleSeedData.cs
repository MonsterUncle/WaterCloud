using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：数据权限种子
    /// </summary>
    public class DataPrivilegeRuleSeedData : ISqlSugarEntitySeedData<DataPrivilegeRuleEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<DataPrivilegeRuleEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"07999ec1-9fbb-46b5-bcea-d343bf49e6d8\",\r\n        \"F_ModuleId\": \"7e4e4a48-4d51-4159-a113-2a211186f13a\",\r\n        \"F_ModuleCode\": \"Notice\",\r\n        \"F_PrivilegeRules\": \"[{\\\"Operation\\\":\\\"and\\\",\\\"Filters\\\":\\\"[{\\\\\\\"Key\\\\\\\":\\\\\\\"{loginRole}\\\\\\\",\\\\\\\"Contrast\\\\\\\":\\\\\\\"contains\\\\\\\",\\\\\\\"Text\\\\\\\":\\\\\\\"0003\\\\\\\",\\\\\\\"Value\\\\\\\":\\\\\\\"d71324b7-e7eb-47b2-bdea-f0293d36bb7f\\\\\\\"},{\\\\\\\"Key\\\\\\\":\\\\\\\"F_CreatorUserId\\\\\\\",\\\\\\\"Contrast\\\\\\\":\\\\\\\"==\\\\\\\",\\\\\\\"Text\\\\\\\":\\\\\\\"{loginUser}\\\\\\\",\\\\\\\"Value\\\\\\\":\\\\\\\"{loginUser}\\\\\\\"}]\\\",\\\"Description\\\":\\\"0003角色只能查看自己的\\\"},{\\\"Operation\\\":\\\"and\\\",\\\"Filters\\\":\\\"[{\\\\\\\"Key\\\\\\\":\\\\\\\"{loginRole}\\\\\\\",\\\\\\\"Contrast\\\\\\\":\\\\\\\"contains\\\\\\\",\\\\\\\"Text\\\\\\\":\\\\\\\"0002\\\\\\\",\\\\\\\"Value\\\\\\\":\\\\\\\"8c119bce-0d70-4a56-8389-214d8e14e107\\\\\\\"},{\\\\\\\"Key\\\\\\\":\\\\\\\"F_EnabledMark\\\\\\\",\\\\\\\"Contrast\\\\\\\":\\\\\\\"==\\\\\\\",\\\\\\\"Text\\\\\\\":\\\\\\\"0\\\\\\\",\\\\\\\"Value\\\\\\\":\\\\\\\"0\\\\\\\"}]\\\",\\\"Description\\\":\\\"0002查看全部的\\\"}]\",\r\n        \"F_SortCode\": \"0\",\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_Description\": \"0003角色只能查看自己的,0002查看全部的\",\r\n        \"F_CreatorTime\": \"2020-06-03 11:21:11\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_LastModifyTime\": \"2020-08-31 10:40:11\",\r\n        \"F_LastModifyUserId\": \"ac7610db-b66e-4f57-916c-c7ea0a4b84c9\",\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null\r\n    }\r\n]";
            return data.ToObject<List<DataPrivilegeRuleEntity>>();
        }
    }
}