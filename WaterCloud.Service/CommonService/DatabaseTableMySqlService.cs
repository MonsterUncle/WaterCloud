using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.DataBase.Extensions;
using WaterCloud.Domain;
using WaterCloud.CodeGenerator;

namespace WaterCloud.Service.CommonService
{
	public class DatabaseTableMySqlService : IDatabaseTableService
    {
        private IUnitOfWork _unitOfWork;
        public DatabaseTableMySqlService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #region 获取数据
        public async Task<List<TableInfo>> GetTableList(string tableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT table_name TableName FROM information_schema.tables WHERE table_schema='" + GetDatabase() + "' AND (table_type='base table' or table_type='BASE TABLE' or table_type='view')");
            IEnumerable<TableInfo> list =await _unitOfWork.GetDbClient().SqlQueryable<TableInfo>(strSql.ToString()).ToListAsync();
            if (!string.IsNullOrEmpty(tableName))
            {
                list = list.Where(p => p.TableName.Contains(tableName)).ToList();
            }
            await SetTableDetail(list);
            return list.ToList();
        }

        public async Task<List<TableInfo>> GetTablePageList(string tableName, Pagination pagination)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT table_name TableName,CREATE_TIME CreateTime FROM information_schema.tables where table_schema='" + GetDatabase() + "' and (table_type='base table' or table_type='BASE TABLE' or table_type='view')");
            var query = _unitOfWork.GetDbClient().SqlQueryable<TableInfo>(strSql.ToString());
            if (!tableName.IsEmpty())
            {
                query = query.Where(a => a.TableName.Contains(tableName));
            }
            IEnumerable<TableInfo> list = await query.ToListAsync();
            pagination.records = list.Count();
            var tempData = list.OrderByDescending(a=>a.CreateTime).Skip(pagination.rows * (pagination.page - 1)).Take(pagination.rows).AsQueryable().ToList();
            await SetTableDetail(tempData);
            return tempData;
        }

        public async Task<List<TableFieldInfo>> GetTableFieldList(string tableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT COLUMN_NAME TableColumn, 
		                           DATA_TYPE Datatype,
		                           (CASE COLUMN_KEY WHEN 'PRI' THEN 'Y' ELSE '' END) TableIdentity,
		                           REPLACE(REPLACE(SUBSTRING(COLUMN_TYPE,LOCATE('(',COLUMN_TYPE)),'(',''),')','') FieldLength,
	                               (CASE IS_NULLABLE WHEN 'NO' THEN 'N' ELSE 'Y' END) IsNullable,
                                   IFNULL(COLUMN_DEFAULT,'') FieldDefault,
                                   COLUMN_COMMENT Remark
                             FROM information_schema.columns WHERE table_schema='" + GetDatabase() + $"' AND table_name='{tableName}'");
            var list = await _unitOfWork.GetDbClient().SqlQueryable<TableFieldInfo>(strSql.ToString()).ToListAsync();
            return list;
        }
        #endregion

        #region 公有方法
        public async Task<bool> DatabaseBackup(string backupPath)
        {
            string database = HtmlHelper.Resove(GlobalContext.SystemConfig.DBConnectionString.ToLower(), "database=", ";");
            //不能备份
            var result = DbHelper.ExecuteSqlCommand(database, backupPath);
            return result > 0 ? true : false;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取所有表的主键、主键名称、记录数
        /// </summary>
        /// <returns></returns>
        private async Task<List<TableInfo>> GetTableDetailList()
        {
            string strSql = @"SELECT t1.TABLE_NAME TableName,t1.TABLE_COMMENT Remark,t1.TABLE_ROWS TableCount,t2.CONSTRAINT_NAME TableKeyName,t2.column_name TableKey,t1.CREATE_TIME CreateTime
                                     FROM information_schema.TABLES as t1 
	                                 LEFT JOIN INFORMATION_SCHEMA.`KEY_COLUMN_USAGE` as t2 on t1.TABLE_NAME = t2.TABLE_NAME
                                     WHERE t1.TABLE_SCHEMA='" + GetDatabase() + "' AND t2.TABLE_SCHEMA='" + GetDatabase() + "'";

            IEnumerable<TableInfo> list = await _unitOfWork.GetDbClient().SqlQueryable<TableInfo>(strSql.ToString()).ToListAsync();
            return list.ToList();
        }

        /// <summary>
        /// 赋值表的主键、主键名称、记录数
        /// </summary>
        /// <param name="list"></param>
        private async Task SetTableDetail(IEnumerable<TableInfo> list)
        {
            List<TableInfo> detailList =await GetTableDetailList();
            foreach (TableInfo table in list)
            {
                table.TableKey = string.Join(",", detailList.Where(p => p.TableName == table.TableName).Select(p => p.TableKey));
                table.TableKeyName = detailList.Where(p => p.TableName == table.TableName).Select(p => p.TableKeyName).First();
                table.TableCount = detailList.Where(p => p.TableName == table.TableName).Select(p => p.TableCount).First();
                table.CreateTime = detailList.Where(p => p.TableName == table.TableName).Select(p => p.CreateTime).First();
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
