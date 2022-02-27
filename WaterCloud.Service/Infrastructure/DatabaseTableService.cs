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
        public DatabaseTableService(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        public List<DbTableInfo> GetTableList(string tableName)
		{
          return  _unitOfWork.GetDbClient().DbMaintenance.GetTableInfoList(false);

        }
        public List<DbTableInfo> GetTablePageList(string tableName, Pagination pagination)
		{
            var data = _unitOfWork.GetDbClient().DbMaintenance.GetTableInfoList(false);
            pagination.records = data.Count();
            return data.Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows).ToList();
        }
        public List<DbColumnInfo> GetTableFieldList(string tableName)
		{
            return _unitOfWork.GetDbClient().DbMaintenance.GetColumnInfosByTableName(tableName,false);
        }
    }
}
