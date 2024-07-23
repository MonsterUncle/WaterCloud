using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.ContentManage
{
	/// <summary>
	/// 创 建：超级管理员
	/// 日 期：2024-07-19 19:42
	/// 描 述：新闻管理种子
	/// </summary>
	public class ArticleNewsSeedData : ISqlSugarEntitySeedData<ArticleNewsEntity>
	{
        [IgnoreSeedDataUpdate]
        public IEnumerable<ArticleNewsEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"59966d73-fb8d-448e-9576-12e6a2efd7ed\",\r\n        \"F_CategoryId\": \"c71f577a-8c9b-409b-b21c-bb7081060338\",\r\n        \"F_Title\": \"44444\",\r\n        \"F_LinkUrl\": \"\",\r\n        \"F_ImgUrl\": \"\",\r\n        \"F_SeoTitle\": \"44444\",\r\n        \"F_SeoKeywords\": \"\",\r\n        \"F_SeoDescription\": \"\",\r\n        \"F_Tags\": \"\",\r\n        \"F_Zhaiyao\": \"\",\r\n        \"F_Description\": \"\",\r\n        \"F_SortCode\": \"2\",\r\n        \"F_IsTop\": 0,\r\n        \"F_IsHot\": 0,\r\n        \"F_IsRed\": 0,\r\n        \"F_Click\": \"0\",\r\n        \"F_Source\": \"本站\",\r\n        \"F_Author\": \"超级管理员\",\r\n        \"F_EnabledMark\": 1,\r\n        \"F_DeleteMark\": 0,\r\n        \"F_CreatorTime\": \"2020-07-07 14:00:09\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_LastModifyTime\": \"2020-07-23 10:39:02\",\r\n        \"F_LastModifyUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null\r\n    },\r\n    {\r\n        \"F_Id\": \"7b33eab5-fc0f-471e-9ba9-1eaf34f37cd7\",\r\n        \"F_CategoryId\": \"c71f577a-8c9b-409b-b21c-bb7081060338\",\r\n        \"F_Title\": \"3333\",\r\n        \"F_LinkUrl\": \"\",\r\n        \"F_ImgUrl\": \"/file/local/20210606/202106062145091936.jpg\",\r\n        \"F_SeoTitle\": \"3333\",\r\n        \"F_SeoKeywords\": \"xxx\",\r\n        \"F_SeoDescription\": \"xxx\",\r\n        \"F_Tags\": \"\",\r\n        \"F_Zhaiyao\": \"xxx\",\r\n        \"F_Description\": \"xxx\",\r\n        \"F_SortCode\": \"1\",\r\n        \"F_IsTop\": 0,\r\n        \"F_IsHot\": 0,\r\n        \"F_IsRed\": 0,\r\n        \"F_Click\": \"0\",\r\n        \"F_Source\": \"本站\",\r\n        \"F_Author\": \"超级管理员\",\r\n        \"F_EnabledMark\": 1,\r\n        \"F_DeleteMark\": 0,\r\n        \"F_CreatorTime\": \"2020-07-23 10:24:21\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_LastModifyTime\": \"2021-06-06 21:45:14\",\r\n        \"F_LastModifyUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null\r\n    }\r\n]";
            return data.ToObject<List<ArticleNewsEntity>>();
        }
    }
}