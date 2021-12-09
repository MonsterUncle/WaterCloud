using SqlSugar;
using System;
using System.Linq;
using WaterCloud.Code;

namespace WaterCloud.DataBase
{
	public class DBContexHelper
    {
        private static string defaultDbType = GlobalContext.SystemConfig.DBProvider;
        private static string defaultDbConnectionString = GlobalContext.SystemConfig.DBConnectionString;

        public static ConnectionConfig Contex(string ConnectStr = "", string providerName = "")
        {
            ConnectStr = string.IsNullOrEmpty(ConnectStr) ? defaultDbConnectionString : ConnectStr;
            providerName = string.IsNullOrEmpty(providerName) ? defaultDbType : providerName;
            var dbType = Convert.ToInt32(Enum.Parse(typeof(DbType), providerName));
            if (dbType == Convert.ToInt32(DbType.SqlServer))
            {
                return new ConnectionConfig()
                {
                    DbType = (DbType)dbType,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true,
                    ConnectionString = ConnectStr,
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsWithNoLockQuery = true//看这里
                    }
                };
            }
            else
            {
                return new ConnectionConfig()
                {
                    DbType = (DbType)dbType,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true,
                    ConnectionString = ConnectStr
                };
            }
        }
    }
}
