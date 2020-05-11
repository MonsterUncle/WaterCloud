using Chloe;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.DataBase.Extensions;
using WaterCloud.Domain;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Domain.SystemSecurity;

namespace WaterCloud.Service.CommonService
{
    public class DatabaseTableMySqlService : RepositoryBase, IDatabaseTableService
    {
        #region 获取数据
        public List<TableInfo> GetTableList(string tableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT table_name TableName FROM information_schema.tables WHERE table_schema='" + GetDatabase() + "' AND table_type='base table'");
            IEnumerable<TableInfo> list = FindList<TableInfo>(strSql.ToString());
            if (!string.IsNullOrEmpty(tableName))
            {
                list = list.Where(p => p.TableName.Contains(tableName)).ToList();
            }
            SetTableDetail(list);
            return list.ToList();
        }

        public List<TableInfo> GetTablePageList(string tableName, Pagination pagination)
        {
            StringBuilder strSql = new StringBuilder();
            var parameter = new List<DbParam>();
            strSql.Append(@"SELECT table_name TableName FROM information_schema.tables where table_schema='" + GetDatabase() + "' and (table_type='base table' or table_type='BASE TABLE')");

            if (!string.IsNullOrEmpty(tableName))
            {
                strSql.Append(" AND table_name like @TableName ");
                parameter.Add(new DbParam("@TableName", '%' + tableName + '%'));
            }

            IEnumerable<TableInfo> list = FindList<TableInfo>(strSql.ToString(), parameter.ToArray());
            pagination.records = list.Count();
            var tempData = list.Skip(pagination.rows * (pagination.page - 1)).Take(pagination.rows).AsQueryable().ToList();
            SetTableDetail(tempData);
            return tempData;
        }

        public List<TableFieldInfo> GetTableFieldList(string tableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT COLUMN_NAME TableColumn, 
		                           DATA_TYPE Datatype,
		                           (CASE COLUMN_KEY WHEN 'PRI' THEN 'Y' ELSE '' END) TableIdentity,
		                           REPLACE(REPLACE(SUBSTRING(COLUMN_TYPE,LOCATE('(',COLUMN_TYPE)),'(',''),')','') FieldLength,
	                               (CASE IS_NULLABLE WHEN 'NO' THEN 'N' ELSE 'Y' END) IsNullable,
                                   IFNULL(COLUMN_DEFAULT,'') FieldDefault,
                                   COLUMN_COMMENT Remark
                             FROM information_schema.columns WHERE table_schema='" + GetDatabase() + "' AND table_name=@TableName");
            var parameter = new List<DbParam>();
            parameter.Add(new DbParam("@TableName", tableName));
            var list = FindList<TableFieldInfo>(strSql.ToString(), parameter.ToArray());
            return list;
        }
        #endregion

        #region 公有方法
        public bool DatabaseBackup(string database, string backupPath)
        {
            string backupFile = string.Format("{0}\\{1}_{2}.bak", backupPath, database, DateTime.Now.ToString("yyyyMMddHHmmss"));
            string strSql = string.Format(" backup database [{0}] to disk = '{1}'", database, backupFile);
            var result = DbHelper.ExecuteSqlCommand(strSql);
            return result > 0 ? true : false;
        }

        /// <summary>
        /// 仅用在WaterCloud框架里面，同步不同数据库之间的数据，以 MySql 为主库，同步 MySql 的数据到SqlServer和Oracle，保证各个数据库的数据是一样的
        /// </summary>
        /// <returns></returns>
        public void SyncDatabase()
        {
            #region 同步SqlServer数据库
           SyncSqlServerTable<AreaEntity>();
           SyncSqlServerTable<ModuleEntity>();
           SyncSqlServerTable<ModuleButtonEntity>();
           SyncSqlServerTable<ItemsEntity>();
           SyncSqlServerTable<ItemsDetailEntity>();
           SyncSqlServerTable<NoticeEntity>();
           SyncSqlServerTable<OrganizeEntity>();
           SyncSqlServerTable<QuickModuleEntity>();
           SyncSqlServerTable<RoleAuthorizeEntity>();
           SyncSqlServerTable<RoleEntity>();
           SyncSqlServerTable<LogEntity>();
           SyncSqlServerTable<RoleEntity>();
           SyncSqlServerTable<UserEntity>();
           SyncSqlServerTable<UserLogOnEntity>();
           SyncSqlServerTable<ServerStateEntity>();
           SyncSqlServerTable<FilterIPEntity>();
           SyncSqlServerTable<DbBackupEntity>();
            #endregion
        }
        private void SyncSqlServerTable<T>() where T : class, new()
        {
            string sqlServerConnectionString = "192.168.1.17;Initial Catalog = WaterCloudNetDb;User ID=sa;Password=admin@12345;MultipleActiveResultSets=true";
            var list = new RepositoryBase().IQueryable<T>().ToList();
            var context=new RepositoryBase(sqlServerConnectionString, "System.Data.SqlClient");
            context.Delete<T>(p => true);
            foreach (var item in list)
            {
                context.Insert<T>(item);
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取所有表的主键、主键名称、记录数
        /// </summary>
        /// <returns></returns>
        private List<TableInfo> GetTableDetailList()
        {
            string strSql = @"SELECT t1.TABLE_NAME TableName,t1.TABLE_COMMENT Remark,t1.TABLE_ROWS TableCount,t2.CONSTRAINT_NAME TableKeyName,t2.column_name TableKey
                                     FROM information_schema.TABLES as t1 
	                                 LEFT JOIN INFORMATION_SCHEMA.`KEY_COLUMN_USAGE` as t2 on t1.TABLE_NAME = t2.TABLE_NAME
                                     WHERE t1.TABLE_SCHEMA='" + GetDatabase() + "' AND t2.TABLE_SCHEMA='" + GetDatabase() + "'";

            IEnumerable<TableInfo> list = FindList<TableInfo>(strSql.ToString());
            return list.ToList();
        }

        /// <summary>
        /// 赋值表的主键、主键名称、记录数
        /// </summary>
        /// <param name="list"></param>
        private void SetTableDetail(IEnumerable<TableInfo> list)
        {
            List<TableInfo> detailList = GetTableDetailList();
            foreach (TableInfo table in list)
            {
                table.TableKey = string.Join(",", detailList.Where(p => p.TableName == table.TableName).Select(p => p.TableKey));
                table.TableKeyName = detailList.Where(p => p.TableName == table.TableName).Select(p => p.TableKeyName).FirstOrDefault();
                table.TableCount = detailList.Where(p => p.TableName == table.TableName).Select(p => p.TableCount).FirstOrDefault();
            }
        }
        private string GetDatabase()
        {
            string database = HtmlHelper.Resove(GlobalContext.SystemConfig.DBConnectionString.ToLower(), "database=", ";");
            return database;
        }
        #endregion
    }
}
