using SqlSugar;
using System;
using System.Linq;
using WaterCloud.Code;

namespace WaterCloud.DataBase
{
	public class DBContexHelper
    {
        private static string dbType = GlobalContext.SystemConfig.DBProvider;
        private static string dbConnectionString = GlobalContext.SystemConfig.DBConnectionString;

        public static ConnectionConfig Contex(string ConnectStr = "", string providerName = "")
        {
            ConnectStr = string.IsNullOrEmpty(ConnectStr) ? dbConnectionString : ConnectStr;
            providerName = string.IsNullOrEmpty(providerName) ? dbType : providerName;
            if (providerName == Define.DBTYPE_SQLSERVER)
            {
                return new ConnectionConfig()
                {
                    DbType = DbType.SqlServer,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true,
                    ConnectionString = ConnectStr,
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsWithNoLockQuery = true//看这里
                    }
                };
            }
            else if (providerName == Define.DBTYPE_MYSQL)
            {
                return new ConnectionConfig()
                {
                    DbType = DbType.MySql,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true,
                    ConnectionString = ConnectStr
                };
            }
            else if (providerName == Define.DBTYPE_ORACLE)
            {
                return new ConnectionConfig()
                {
                    DbType = DbType.Oracle,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true,
                    ConnectionString = ConnectStr
                };
            }
            else
            {
                return null;
            }
        }
    }
}
