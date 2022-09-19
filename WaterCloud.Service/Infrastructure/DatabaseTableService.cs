using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemSecurity;

namespace WaterCloud.Service
{
    public class DatabaseTableService:IDenpendency
    {
        private RepositoryBase<LogEntity> repository;
		private static string cacheKey = GlobalContext.SystemConfig.ProjectPrefix + "_dblist";// 数据库键
        public DatabaseTableService(ISqlSugarClient context)
		{
            repository = new RepositoryBase<LogEntity>(context);
        }
        public List<DbTableInfo> GetTableList(string tableName, string dbNumber)
		{
			if (string.IsNullOrEmpty(dbNumber))
			{
                dbNumber = GlobalContext.SystemConfig.MainDbNumber;
            }
			repository.ChangeEntityDb(dbNumber);
            var data = repository.Db.DbMaintenance.GetTableInfoList(false);
            if (!string.IsNullOrEmpty(tableName))
                data = data.Where(a => a.Name.Contains(tableName)).ToList();
            return  data;

        }
        public List<DbTableInfo> GetTablePageList(string tableName, string dbNumber, Pagination pagination)
		{
            if (string.IsNullOrEmpty(dbNumber))
            {
                dbNumber = GlobalContext.SystemConfig.MainDbNumber;
            }
			repository.ChangeEntityDb(dbNumber);
            var data = repository.Db.DbMaintenance.GetTableInfoList(false);
			if (!string.IsNullOrEmpty(tableName))
                data = data.Where(a => a.Name.Contains(tableName)).ToList();
            pagination.records = data.Count();
            return data.Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows).ToList();
        }
        public List<dynamic> GetDbNumberListJson()
        {
            var data = DBInitialize.GetConnectionConfigs();
            return data.Select(a=>a.ConfigId).ToList();
        }
        
        public List<DbColumnInfo> GetTableFieldList(string tableName, string dbNumber)
		{
            if (string.IsNullOrEmpty(dbNumber))
            {
                dbNumber = GlobalContext.SystemConfig.MainDbNumber;
            }
			repository.ChangeEntityDb(dbNumber);
            var data= repository.Db.DbMaintenance.GetColumnInfosByTableName(tableName,false);
            return data;
        }
    }
}
