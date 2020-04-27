using Chloe.Infrastructure;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.DataBase
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        string _connString = null;
        public MySqlConnectionFactory(string connString)
        {
            this._connString = connString;
        }
        public IDbConnection CreateConnection()
        {
            IDbConnection conn = new MySqlConnection(this._connString);
            /*如果有必要需要包装一下驱动的 MySqlConnection*/
            //conn = new Chloe.MySql.ChloeMySqlConnection(conn); 
            return conn;

        }
    }
}
