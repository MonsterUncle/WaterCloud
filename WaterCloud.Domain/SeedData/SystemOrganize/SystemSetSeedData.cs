using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：租户种子
    /// </summary>
    public class SystemSetSeedData : ISqlSugarEntitySeedData<SystemSetEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<SystemSetEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"d69fd66a-6a77-4011-8a25-53a79bdf5001\",\r\n        \"F_Logo\": \"/icon/favicon.ico\",\r\n        \"F_LogoCode\": \"WaterCloud\",\r\n        \"F_ProjectName\": \"水之云信息系统\",\r\n        \"F_CompanyName\": \"水之云\",\r\n        \"F_AdminAccount\": \"admin\",\r\n        \"F_AdminPassword\": \"0000\",\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_Description\": \"\",\r\n        \"F_CreatorTime\": \"2020-06-12 16:30:00\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_LastModifyTime\": \"2021-06-09 13:22:12\",\r\n        \"F_LastModifyUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null,\r\n        \"F_MobilePhone\": \"13621551864\",\r\n        \"F_PrincipalMan\": \"MonsterUncle\",\r\n        \"F_EndTime\": \"2032-06-26 00:00:00\",\r\n        \"F_DbString\": \"data source=localhost;database=watercloudnetdb;uid=root;pwd=root;\",\r\n        \"F_DBProvider\": \"MySql.Data.MySqlClient\",\r\n        \"F_HostUrl\": \"localhost\",\r\n        \"F_DbNumber\": \"0\"\r\n    },\r\n    {\r\n        \"F_Id\": \"08dab0c3-6b99-4654-8684-705994c1d32e\",\r\n        \"F_Logo\": \"/icon/local/20221018/202210181244189323.jpg\",\r\n        \"F_LogoCode\": \"test\",\r\n        \"F_ProjectName\": \"test\",\r\n        \"F_CompanyName\": \"tst\",\r\n        \"F_AdminAccount\": \"test\",\r\n        \"F_AdminPassword\": \"0000\",\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_Description\": null,\r\n        \"F_CreatorTime\": \"2022-10-18 12:44:19.6674095\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_LastModifyTime\": null,\r\n        \"F_LastModifyUserId\": null,\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null,\r\n        \"F_MobilePhone\": \"13295893319\",\r\n        \"F_PrincipalMan\": \"test\",\r\n        \"F_EndTime\": \"2022-11-18 00:00:00\",\r\n        \"F_DbString\": \"data source=localhost;database=test;uid=root;pwd=123456;\",\r\n        \"F_DBProvider\": \"MySql\",\r\n        \"F_HostUrl\": \"128.0.0.1\",\r\n        \"F_DbNumber\": \"t-1\"\r\n    }\r\n]";
            return data.ToObject<List<SystemSetEntity>>();
        }
    }
}