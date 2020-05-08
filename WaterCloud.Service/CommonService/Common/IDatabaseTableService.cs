using System.Collections.Generic;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Domain;

namespace WaterCloud.Service.CommonService
{
    public interface IDatabaseTableService
    {
        bool DatabaseBackup(string database, string backupPath);
        List<TableInfo> GetTableList(string tableName);
        List<TableInfo> GetTablePageList(string tableName, Pagination pagination);
        List<TableFieldInfo> GetTableFieldList(string tableName);
    }
}
