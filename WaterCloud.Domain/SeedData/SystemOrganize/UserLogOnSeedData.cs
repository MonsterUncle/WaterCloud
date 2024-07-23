using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：用户详细种子
    /// </summary>
    public class UserLogOnSeedData : ISqlSugarEntitySeedData<UserLogOnEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<UserLogOnEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_UserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_UserPassword\": \"99e5c8431f64527c0acd80b34a3dce3c\",\r\n        \"F_UserSecretkey\": \"74aeb85d04e4b485\",\r\n        \"F_AllowStartTime\": null,\r\n        \"F_AllowEndTime\": null,\r\n        \"F_LockStartDate\": null,\r\n        \"F_LockEndDate\": null,\r\n        \"F_FirstVisitTime\": null,\r\n        \"F_PreviousVisitTime\": \"2020-04-17 14:47:44\",\r\n        \"F_LastVisitTime\": \"2020-04-17 14:59:58\",\r\n        \"F_ChangePasswordDate\": null,\r\n        \"F_MultiUserLogin\": 0,\r\n        \"F_LogOnCount\": \"360\",\r\n        \"F_UserOnLine\": 0,\r\n        \"F_Question\": null,\r\n        \"F_AnswerQuestion\": null,\r\n        \"F_CheckIPAddress\": 0,\r\n        \"F_Language\": null,\r\n        \"F_Theme\": null,\r\n        \"F_LoginSession\": \"evrcyibdv42f3ykhfy1yz3ur\",\r\n        \"F_ErrorNum\": \"0\"\r\n    },\r\n    {\r\n        \"F_Id\": \"08da9c6c-6d08-47d7-83fe-69ad505d845b\",\r\n        \"F_UserId\": \"08da9c6c-6d08-47d7-83fe-69ad505d845b\",\r\n        \"F_UserPassword\": \"da0240692972ba3d87c3354ce4483331\",\r\n        \"F_UserSecretkey\": \"8d130bda8b26f6dd\",\r\n        \"F_AllowStartTime\": null,\r\n        \"F_AllowEndTime\": null,\r\n        \"F_LockStartDate\": null,\r\n        \"F_LockEndDate\": null,\r\n        \"F_FirstVisitTime\": null,\r\n        \"F_PreviousVisitTime\": null,\r\n        \"F_LastVisitTime\": null,\r\n        \"F_ChangePasswordDate\": null,\r\n        \"F_MultiUserLogin\": null,\r\n        \"F_LogOnCount\": \"0\",\r\n        \"F_UserOnLine\": 0,\r\n        \"F_Question\": null,\r\n        \"F_AnswerQuestion\": null,\r\n        \"F_CheckIPAddress\": null,\r\n        \"F_Language\": null,\r\n        \"F_Theme\": null,\r\n        \"F_LoginSession\": null,\r\n        \"F_ErrorNum\": \"0\"\r\n    }\r\n]";
            return data.ToObject<List<UserLogOnEntity>>();
        }
    }
}