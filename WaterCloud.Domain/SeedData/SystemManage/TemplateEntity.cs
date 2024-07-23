using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：快捷菜单种子
    /// </summary>
    public class TemplateSeedData : ISqlSugarEntitySeedData<TemplateEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<TemplateEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"08daa778-f508-4fd6-8f60-e60cb414b464\",\r\n        \"F_TemplateName\": \"测试模板\",\r\n        \"F_TemplateFile\": \"/file/local/20221006/202210061658345649.frx\",\r\n        \"F_TemplateDBProvider\": \"0\",\r\n        \"F_TemplateSql\": \"SELECT F_EnCode barId,F_EnCode code,F_FullName name FROM `sys_organize` where F_Id='5AB270C0-5D33-4203-A54F-4552699FDA3C'\",\r\n        \"F_TemplateSqlParm\": \"\",\r\n        \"F_PrintType\": \"1\",\r\n        \"F_Batch\": 0,\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_Description\": null,\r\n        \"F_CreatorTime\": \"2022-10-06 16:58:38\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_CreatorUserName\": null,\r\n        \"F_LastModifyTime\": null,\r\n        \"F_LastModifyUserId\": null,\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null\r\n    }\r\n]";
            return data.ToObject<List<TemplateEntity>>();
        }
    }
}