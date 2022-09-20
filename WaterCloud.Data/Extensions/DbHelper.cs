/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/

using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using WaterCloud.Code;

namespace WaterCloud.DataBase.Extensions
{
	public class DbHelper
	{
		private static string connstring = GlobalContext.SystemConfig.DBConnectionString;
		private static string dbType = GlobalContext.SystemConfig.DBProvider;

		public static int ExecuteSqlCommand(string database, string backupPath)
		{
			try
			{
				string backupFile = string.Format("{0}\\{1}_{2}.bak", backupPath, database, DateTime.Now.ToString("yyyyMMddHHmmss"));
				using (DbConnection conn = new SqlConnection(connstring))
				{
					string strSql = string.Format(" backup database [{0}] to disk = '{1}'", database, backupFile);
					DbCommand cmd = new SqlCommand();
					PrepareCommand(cmd, conn, null, CommandType.Text, strSql, null);
					return cmd.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				LogHelper.WriteWithTime(ex);
				return 0;
			}
		}

		private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction isOpenTrans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
		{
			if (conn.State != ConnectionState.Open)
				conn.Open();
			cmd.Connection = conn;
			cmd.CommandText = cmdText;
			if (isOpenTrans != null)
				cmd.Transaction = isOpenTrans;
			cmd.CommandType = cmdType;
			if (cmdParms != null)
			{
				cmd.Parameters.AddRange(cmdParms);
			}
		}
	}
}