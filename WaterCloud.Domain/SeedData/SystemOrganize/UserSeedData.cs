using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：用户种子
    /// </summary>
    public class UserSeedData : ISqlSugarEntitySeedData<UserEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<UserEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_Account\": \"admin\",\r\n        \"F_RealName\": \"超级管理员\",\r\n        \"F_NickName\": \"超级管理员\",\r\n        \"F_HeadIcon\": null,\r\n        \"F_Gender\": 1,\r\n        \"F_Birthday\": \"2020-03-27 00:00:00\",\r\n        \"F_MobilePhone\": \"13600000000\",\r\n        \"F_Email\": \"3333\",\r\n        \"F_WeChat\": null,\r\n        \"F_ManagerId\": null,\r\n        \"F_SecurityLevel\": null,\r\n        \"F_Signature\": null,\r\n        \"F_CompanyId\": \"d69fd66a-6a77-4011-8a25-53a79bdf5001\",\r\n        \"F_OrganizeId\": \"5AB270C0-5D33-4203-A54F-4552699FDA3C\",\r\n        \"F_RoleId\": null,\r\n        \"F_DutyId\": null,\r\n        \"F_IsAdmin\": 1,\r\n        \"F_IsBoss\": 0,\r\n        \"F_IsLeaderInDepts\": 0,\r\n        \"F_IsSenior\": 0,\r\n        \"F_SortCode\": null,\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_Description\": \"系统内置账户\",\r\n        \"F_CreatorTime\": \"2016-07-20 00:00:00\",\r\n        \"F_CreatorUserId\": null,\r\n        \"F_LastModifyTime\": null,\r\n        \"F_LastModifyUserId\": null,\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null,\r\n        \"F_DingTalkUserId\": null,\r\n        \"F_DingTalkUserName\": \"闫志辉\",\r\n        \"F_DingTalkAvatar\": null,\r\n        \"F_WxOpenId\": null,\r\n        \"F_WxNickName\": null,\r\n        \"F_HeadImgUrl\": null\r\n    },\r\n    {\r\n        \"F_Id\": \"08da9c6c-6d08-47d7-83fe-69ad505d845b\",\r\n        \"F_Account\": \"33333\",\r\n        \"F_RealName\": \"33333\",\r\n        \"F_NickName\": null,\r\n        \"F_HeadIcon\": null,\r\n        \"F_Gender\": 1,\r\n        \"F_Birthday\": null,\r\n        \"F_MobilePhone\": \"\",\r\n        \"F_Email\": \"\",\r\n        \"F_WeChat\": \"\",\r\n        \"F_ManagerId\": null,\r\n        \"F_SecurityLevel\": null,\r\n        \"F_Signature\": null,\r\n        \"F_CompanyId\": \"d69fd66a-6a77-4011-8a25-53a79bdf5001\",\r\n        \"F_OrganizeId\": \"5AB270C0-5D33-4203-A54F-4552699FDA3C\",\r\n        \"F_RoleId\": \"08da9c6b-f455-46c0-8ffe-81edad14c7be\",\r\n        \"F_DutyId\": \"08da9b75-9474-4da4-8801-ca3a549a8800\",\r\n        \"F_IsAdmin\": 0,\r\n        \"F_IsBoss\": 0,\r\n        \"F_IsLeaderInDepts\": 0,\r\n        \"F_IsSenior\": 0,\r\n        \"F_SortCode\": null,\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_Description\": null,\r\n        \"F_CreatorTime\": \"2022-09-22 15:31:12.6022639\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_LastModifyTime\": null,\r\n        \"F_LastModifyUserId\": null,\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null,\r\n        \"F_DingTalkUserId\": null,\r\n        \"F_DingTalkUserName\": null,\r\n        \"F_DingTalkAvatar\": null,\r\n        \"F_WxOpenId\": null,\r\n        \"F_WxNickName\": null,\r\n        \"F_HeadImgUrl\": null\r\n    }\r\n]";
            return data.ToObject<List<UserEntity>>();
        }
    }
}