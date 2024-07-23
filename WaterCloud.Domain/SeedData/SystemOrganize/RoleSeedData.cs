using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemSecurity;

namespace WaterCloud.Domain.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：权限种子
    /// </summary>
    public class RoleSeedData : ISqlSugarEntitySeedData<RoleEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<RoleEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"08da9b75-9474-4da4-8801-ca3a549a8800\",\r\n        \"F_CompanyId\": \"d69fd66a-6a77-4011-8a25-53a79bdf5001\",\r\n        \"F_Category\": \"2\",\r\n        \"F_EnCode\": \"11111\",\r\n        \"F_FullName\": \"111111\",\r\n        \"F_Type\": null,\r\n        \"F_AllowEdit\": 0,\r\n        \"F_AllowDelete\": 0,\r\n        \"F_SortCode\": \"11\",\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_Description\": \"\",\r\n        \"F_CreatorTime\": \"2022-09-21 10:04:13.0513223\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_LastModifyTime\": \"2022-09-21 11:22:04.4167701\",\r\n        \"F_LastModifyUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null\r\n    },\r\n    {\r\n        \"F_Id\": \"08da9c6b-f455-46c0-8ffe-81edad14c7be\",\r\n        \"F_CompanyId\": \"d69fd66a-6a77-4011-8a25-53a79bdf5001\",\r\n        \"F_Category\": \"1\",\r\n        \"F_EnCode\": \"2222\",\r\n        \"F_FullName\": \"22222\",\r\n        \"F_Type\": \"1\",\r\n        \"F_AllowEdit\": 0,\r\n        \"F_AllowDelete\": 0,\r\n        \"F_SortCode\": \"2\",\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_Description\": \"\",\r\n        \"F_CreatorTime\": \"2022-09-22 15:27:50.1022554\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_LastModifyTime\": null,\r\n        \"F_LastModifyUserId\": null,\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null\r\n    },\r\n    {\r\n        \"F_Id\": \"08da9c6c-0fd9-4fde-8b5a-8c1bb0c385b5\",\r\n        \"F_CompanyId\": \"d69fd66a-6a77-4011-8a25-53a79bdf5001\",\r\n        \"F_Category\": \"1\",\r\n        \"F_EnCode\": \"33333\",\r\n        \"F_FullName\": \"33333\",\r\n        \"F_Type\": \"1\",\r\n        \"F_AllowEdit\": 0,\r\n        \"F_AllowDelete\": 0,\r\n        \"F_SortCode\": \"3\",\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_Description\": \"\",\r\n        \"F_CreatorTime\": \"2022-09-22 15:28:36.2693743\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_LastModifyTime\": null,\r\n        \"F_LastModifyUserId\": null,\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null\r\n    }\r\n]";
            return data.ToObject<List<RoleEntity>>();
        }
    }
}