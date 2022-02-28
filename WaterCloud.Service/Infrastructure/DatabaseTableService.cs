using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.CodeGenerator;
using WaterCloud.DataBase;
using WaterCloud.Domain;

namespace WaterCloud.Service
{
    public class DatabaseTableService:IDenpendency
    {
        private IUnitOfWork _unitOfWork;
        private static string cacheKey = GlobalContext.SystemConfig.ProjectPrefix + "_dblist";// 数据库键
        public DatabaseTableService(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        public List<DbTableInfo> GetTableList(string tableName, string dbNumber)
		{
			if (string.IsNullOrEmpty(dbNumber))
			{
                dbNumber = GlobalContext.SystemConfig.MainDbNumber;
            }
            _unitOfWork.GetDbClient().ChangeDatabase(dbNumber);
            var data = _unitOfWork.GetDbClient().DbMaintenance.GetTableInfoList(false);
            if (!string.IsNullOrEmpty(tableName))
                data = data.Where(a => a.Name.Contains(tableName)).ToList();
            _unitOfWork.GetDbClient().ChangeDatabase(GlobalContext.SystemConfig.MainDbNumber);
            return  data;

        }
        public List<DbTableInfo> GetTablePageList(string tableName, string dbNumber, Pagination pagination)
		{
            if (string.IsNullOrEmpty(dbNumber))
            {
                dbNumber = GlobalContext.SystemConfig.MainDbNumber;
            }
            _unitOfWork.GetDbClient().ChangeDatabase(dbNumber);
            var data = _unitOfWork.GetDbClient().DbMaintenance.GetTableInfoList(false);
			if (!string.IsNullOrEmpty(tableName))
                data = data.Where(a => a.Name.Contains(tableName)).ToList();
            pagination.records = data.Count();
            _unitOfWork.GetDbClient().ChangeDatabase(GlobalContext.SystemConfig.MainDbNumber);
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
            _unitOfWork.GetDbClient().ChangeDatabase(dbNumber);
            var data= _unitOfWork.GetDbClient().DbMaintenance.GetColumnInfosByTableName(tableName,false);
            _unitOfWork.GetDbClient().ChangeDatabase(GlobalContext.SystemConfig.MainDbNumber);
            return data;
        }
    }
}
