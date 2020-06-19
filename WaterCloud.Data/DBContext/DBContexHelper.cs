using Chloe;
using Chloe.MySql;
using Chloe.Oracle;
using Chloe.SqlServer;
using WaterCloud.Code;

namespace WaterCloud.DataBase
{
    public class DBContexHelper
    {
        private static string dbType = GlobalContext.SystemConfig.DBProvider;
        private static string dbConnectionString = GlobalContext.SystemConfig.DBConnectionString;

        public static IDbContext Contex(string ConnectStr = "", string providerName = "")
        {
            ConnectStr = string.IsNullOrEmpty(ConnectStr) ? dbConnectionString : ConnectStr;
            providerName = string.IsNullOrEmpty(providerName) ? dbType : providerName;
            IDbContext context;
            switch (providerName)
            {
                case Define.DBTYPE_SQLSERVER:
                    context = new MsSqlContext(ConnectStr);
                    break;
                case Define.DBTYPE_MYSQL:
                    context = new MySqlContext(new MySqlConnectionFactory(ConnectStr));
                    break;
                case Define.DBTYPE_ORACLE:
                    var con = new OracleContext(new OracleConnectionFactory(ConnectStr));
                    con.ConvertToUppercase = false;
                    context = con;
                    break;
                default:
                    context = new MsSqlContext(ConnectStr);
                    break;
            }
            return context;
        }
    }
}
