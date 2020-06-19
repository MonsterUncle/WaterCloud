using Chloe;
using System;
using System.IO;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Service.CommonService;

namespace WaterCloud.Service.AutoJob
{
    public class DatabasesBackupJob : IJobTask
    {
        #region  构造函数
        private IDatabaseTableService databaseTableService;

        public DatabasesBackupJob(IDbContext context)
        {
            string dbType = GlobalContext.SystemConfig.DBProvider;
            switch (dbType)
            {
                case "MySql.Data.MySqlClient":
                    databaseTableService = new DatabaseTableMySqlService (context);
                    break;
                case "System.Data.SqlClient":
                    databaseTableService = new DatabaseTableSqlServerService(context);
                    break;
                default:
                    throw new Exception("未找到数据库配置");
            }
        }
        #endregion
        public async Task<AjaxResult> Start()
        {
            AjaxResult obj = new AjaxResult();
            string backupPath = GlobalContext.SystemConfig.DBBackup;
            if (string.IsNullOrEmpty(backupPath))
            {
                backupPath = Path.Combine(GlobalContext.HostingEnvironment.ContentRootPath, "Database");
            }
            else
            {
                backupPath = Path.Combine(GlobalContext.HostingEnvironment.ContentRootPath, backupPath);
            }

            if (!Directory.Exists(backupPath))
            {
                Directory.CreateDirectory(backupPath);
            }
            if(await databaseTableService.DatabaseBackup(backupPath))
            {
                obj.state = ResultType.success.ToString();
                obj.message = "备份路径：" + backupPath;
            }
            else
            {
                obj.state = ResultType.error.ToString();
                obj.message = "数据库备份错误";
            }
            return obj;
        }
    }
}
