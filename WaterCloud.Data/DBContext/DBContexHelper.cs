using Chloe;
using Chloe.MySql;
using Chloe.Oracle;
using Chloe.SqlServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.DataBase
{
    public class DBContexHelper
    {
        public static DbContext Contex(string ConnectStr = "", string providerName = "")
        {
            ConnectStr = string.IsNullOrEmpty(ConnectStr) ? ConfigurationManager.ConnectionStrings["WaterCloudDbContext"].ConnectionString : ConnectStr;
            providerName = string.IsNullOrEmpty(providerName) ? ConfigurationManager.ConnectionStrings["WaterCloudDbContext"].ProviderName : providerName;
            DbContext context;
            switch (providerName)
            {
                case "System.Data.SqlClient":
                    context = new MsSqlContext(ConnectStr);
                    break;
                case "MySql.Data.MySqlClient":
                    context = new MySqlContext(new MySqlConnectionFactory(ConnectStr));
                    break;
                case "Oracle.ManagedDataAccess.Client":
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
