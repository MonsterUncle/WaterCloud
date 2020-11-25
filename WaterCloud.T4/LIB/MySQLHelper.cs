using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace AdoNetBase
{
    public class MySQLHelper
    {
        public static string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();
        }

        public static void UpdateConnectionString(string newConnStrValue)
        {
            bool isModified = false;
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == "ConnStr")
                {
                    isModified = true;
                }
            }
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            if (isModified)
            {
                config.AppSettings.Settings.Remove("ConnStr");
            }
            config.AppSettings.Settings.Add("ConnStr", newConnStrValue);
            
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        #region ExecuteNonQuery方法
        public static int ExecuteNonQuery(string connectionString,string sql,params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using(MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region ExecuteScalar方法
        public static object ExecuteScalar(string connectionString,string sql, params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using(MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }
        #endregion

        #region ExecuteTable方法
        public static DataTable ExecuteDataTable(string connectionString,string sql, params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            { 
                using(MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);
                            da.FillSchema(dt, SchemaType.Source);   //从数据源中检索架构
                            return dt;
                        }
                    }
                }
            }
        }
        #endregion

        #region ExecuteReader方法
        public static MySqlDataReader ExecuteDataReader(string connectionString,string sql, params MySqlParameter[] parameters)
        {

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddRange(parameters);
            MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;       
        }

        #endregion

        public static string GetDataBaseString(string connectionString)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                return conn.Database;
            }
        }
    }
}