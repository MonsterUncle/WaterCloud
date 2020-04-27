using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.DataBase
{
    public class OracleConnectionDecorator : IDbConnection, IDisposable
    {
        private OracleConnection _oracleConnection;
        public OracleConnectionDecorator(OracleConnection oracleConnection)
        {
            if (oracleConnection == null)
                throw new Exception("Please call 911.");
            _oracleConnection = oracleConnection;
        }

        public string ConnectionString
        {
            get { return _oracleConnection.ConnectionString; }
            set { _oracleConnection.ConnectionString = value; }
        }
        public int ConnectionTimeout
        {
            get { return _oracleConnection.ConnectionTimeout; }
        }
        public string Database
        {
            get { return _oracleConnection.Database; }
        }
        public ConnectionState State
        {
            get { return _oracleConnection.State; }
        }

        public IDbTransaction BeginTransaction()
        {
            return _oracleConnection.BeginTransaction();
        }
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return _oracleConnection.BeginTransaction(il);
        }
        public void ChangeDatabase(string databaseName)
        {
            _oracleConnection.ChangeDatabase(databaseName);
        }
        public void Close()
        {
            _oracleConnection.Close();
        }
        public IDbCommand CreateCommand()
        {
            var cmd = _oracleConnection.CreateCommand();
            cmd.BindByName = true; //修改 DbCommand 参数绑定方式
            return cmd;
        }
        public void Open()
        {
            _oracleConnection.Open();
        }

        public void Dispose()
        {
            _oracleConnection.Dispose();
        }
    }
}
