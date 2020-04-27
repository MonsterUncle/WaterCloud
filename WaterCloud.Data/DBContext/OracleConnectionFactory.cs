using Chloe.Infrastructure;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.DataBase
{
    public class OracleConnectionFactory : IDbConnectionFactory
    {
        string _connString = null;
        public OracleConnectionFactory(string connString)
        {
            this._connString = connString;
        }
        public IDbConnection CreateConnection()
        {
            OracleConnection oracleConnection = new OracleConnection(this._connString);
            OracleConnectionDecorator conn = new OracleConnectionDecorator(oracleConnection);
            return conn;
        }
    }
}