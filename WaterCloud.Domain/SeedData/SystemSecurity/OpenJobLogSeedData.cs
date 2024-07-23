using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemSecurity
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2024-07-19 19:42
    /// 描 述：定时任务记录种子
    /// </summary>
    public class OpenJobLogSeedData : ISqlSugarEntitySeedData<OpenJobLogEntity>
    {
        [IgnoreSeedDataUpdate]
        public IEnumerable<OpenJobLogEntity> SeedData()
        {
            var data = "[\r\n    {\r\n        \"F_Id\": \"08da9c6c-fcfa-456c-856b-e5d29ab7d906\",\r\n        \"F_FileName\": \"WaterCloud.Service.AutoJob.SaveServerStateJob\",\r\n        \"F_JobName\": \"更新服务时间\",\r\n        \"F_JobGroup\": \"watercloud\",\r\n        \"F_StarRunTime\": \"2023-01-13 17:03:01.7372776\",\r\n        \"F_EndRunTime\": \"2023-01-13 17:03:00.7372792\",\r\n        \"F_CronExpress\": \"0/1 * * * * ?\",\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 1,\r\n        \"F_Description\": \"\",\r\n        \"F_CreatorTime\": \"2022-09-22 15:35:14.1014342\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_LastModifyTime\": \"2023-01-13 17:03:01.7654604\",\r\n        \"F_LastModifyUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null,\r\n        \"F_LastRunTime\": \"2023-01-13 17:08:21.0012127\",\r\n        \"F_LastRunMark\": \"0\",\r\n        \"F_LastRunErrTime\": \"2023-01-13 17:08:21.0012127\",\r\n        \"F_LastRunErrMsg\": \"执行失败，服务器状态更新失败！The transaction object is not associated with the same connection object as this command.\",\r\n        \"F_JobType\": \"0\",\r\n        \"F_IsLog\": \"是\",\r\n        \"F_RequestHeaders\": \"\",\r\n        \"F_RequestString\": \"\",\r\n        \"F_RequestUrl\": \"\",\r\n        \"F_DbNumber\": \"0\",\r\n        \"F_JobDBProvider\": \"\",\r\n        \"F_JobSql\": \"\",\r\n        \"F_JobSqlParm\": \"\"\r\n    },\r\n    {\r\n        \"F_Id\": \"08dae8b2-2f71-49f8-892c-b95f39e0ca84\",\r\n        \"F_FileName\": \"\",\r\n        \"F_JobName\": \"百度\",\r\n        \"F_JobGroup\": \"watercloud\",\r\n        \"F_StarRunTime\": \"2023-01-13 17:03:01.8428589\",\r\n        \"F_EndRunTime\": \"2023-01-13 17:03:37.7810888\",\r\n        \"F_CronExpress\": \"0/1 * * * * ?\",\r\n        \"F_DeleteMark\": 0,\r\n        \"F_EnabledMark\": 0,\r\n        \"F_Description\": \"\",\r\n        \"F_CreatorTime\": \"2022-12-28 17:02:02.4145795\",\r\n        \"F_CreatorUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_LastModifyTime\": \"2023-01-13 17:03:37.7815455\",\r\n        \"F_LastModifyUserId\": \"9f2ec079-7d0f-4fe2-90ab-8b09a8302aba\",\r\n        \"F_DeleteTime\": null,\r\n        \"F_DeleteUserId\": null,\r\n        \"F_LastRunTime\": \"2023-01-13 17:03:12.0032829\",\r\n        \"F_LastRunMark\": \"1\",\r\n        \"F_LastRunErrTime\": \"2023-01-13 11:27:47.2781789\",\r\n        \"F_LastRunErrMsg\": \"执行失败，An error occurred while sending the request.\",\r\n        \"F_JobType\": \"1\",\r\n        \"F_IsLog\": \"是\",\r\n        \"F_RequestHeaders\": \"\",\r\n        \"F_RequestString\": \"\",\r\n        \"F_RequestUrl\": \"https://www.baidu.com/s?wd=%E6%B3%95%E6%8B%89%E7%AC%AC%E6%9C%AA%E6%9D%A5\",\r\n        \"F_DbNumber\": \"0\",\r\n        \"F_JobDBProvider\": \"\",\r\n        \"F_JobSql\": \"\",\r\n        \"F_JobSqlParm\": \"\"\r\n    }\r\n]";
            return data.ToObject<List<OpenJobLogEntity>>();
        }
    }
}