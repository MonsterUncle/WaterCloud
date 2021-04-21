using Chloe.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WaterCloud.DataBase
{
	public class MSSqlConnectionFactory : IDbConnectionFactory
    {
        string _connString = null;
        public MSSqlConnectionFactory(string connString)
        {
            this._connString = connString;
        }
        public IDbConnection CreateConnection()
        {
            IDbConnection conn = new SqlConnection(this._connString);
            return conn;
        }
    }
}