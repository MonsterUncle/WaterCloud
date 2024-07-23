using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：新闻管理分类种子
    /// </summary>
    public class ArticleCategorySeedData : ISqlSugarEntitySeedData<ArticleCategoryEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<ArticleCategoryEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"b2be2ea5-819e-485b-a277-26be5396db65\",\r\n        \"F_FullName\": \"222\",\r\n        \"F_ParentId\": \"0\",\r\n        \"F_SortCode\": \"9933\",\r\n        \"F_Description\": \"222\",\r\n        \"F_LinkUrl\": \"222\",\r\n        \"F_ImgUrl\": \"22\",\r\n        \"F_SeoTitle\": \"222\",\r\n        \"F_SeoKeywords\": \"22\",\r\n        \"F_SeoDescription\": \"22\",\r\n        \"F_IsHot\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_DeleteMark\": 0,\r\n        \"F_CreatorTime\": \"2020-06-23 16:29:31\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_LastModifyTime\": \"2020-06-29 17:25:28\",\r\n        \"F_LastModifyUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null\r\n    },\r\n    {\r\n        \"F_Id\": \"c71f577a-8c9b-409b-b21c-bb7081060338\",\r\n        \"F_FullName\": \"啊倒萨大大苏打大苏打撒大苏打啊实打实的阿德飒飒啊是\",\r\n        \"F_ParentId\": \"0\",\r\n        \"F_SortCode\": \"22222222\",\r\n        \"F_Description\": \"222\",\r\n        \"F_LinkUrl\": \"http://www.baidu.com\",\r\n        \"F_ImgUrl\": \"http://www.baidu.com\",\r\n        \"F_SeoTitle\": \"\",\r\n        \"F_SeoKeywords\": \"\",\r\n        \"F_SeoDescription\": \"\",\r\n        \"F_IsHot\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_DeleteMark\": 0,\r\n        \"F_CreatorTime\": \"2020-06-30 11:56:16\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_LastModifyTime\": \"2020-08-11 13:22:20\",\r\n        \"F_LastModifyUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null\r\n    }\r\n]";
            return data.ToObject<List<ArticleCategoryEntity>>();
        }
    }
}