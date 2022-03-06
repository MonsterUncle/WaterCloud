/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using System;
using SqlSugar;

namespace WaterCloud.DataBase
{
	/// <summary>
	/// 非泛型仓储
	/// </summary>
	public class UnitOfWork: IUnitOfWork,IDisposable
    {
        private readonly ISqlSugarClient _context;
        public UnitOfWork(ISqlSugarClient context)
        {
            int commandTimeout =30;
			if (GlobalContext.SystemConfig!=null)
			{
                commandTimeout = GlobalContext.SystemConfig.CommandTimeout;
                var current = OperatorProvider.Provider.GetCurrent();
                if (GlobalContext.SystemConfig.SqlMode==Define.SQL_TENANT && current != null && !string.IsNullOrEmpty(current.DbNumber))
                {
                    (context as SqlSugarClient).ChangeDatabase(current.DbNumber);
                }
            }
            _context = context;
            _context.Ado.CommandTimeOut = commandTimeout;
            _context.CurrentConnectionConfig.ConfigureExternalServices = new ConfigureExternalServices()
            {
                DataInfoCacheService = new SqlSugarCache() //配置我们创建的缓存类
            };
            _context.Aop.OnLogExecuted = (sql, pars) => //SQL执行完
            {
                if (sql.StartsWith("SELECT"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[SELECT]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                }
                if (sql.StartsWith("INSERT"))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("[INSERT]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                }
                if (sql.StartsWith("UPDATE"))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[UPDATE]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                }
                if (sql.StartsWith("DELETE"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[DELETE]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                }
                Console.WriteLine("NeedTime-" + _context.Ado.SqlExecutionTime.ToString());
                //App.PrintToMiniProfiler("SqlSugar", "Info", sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine("Content:" + SqlProfiler.ParameterFormat(sql, pars));
                Console.WriteLine("---------------------------------");
                Console.WriteLine("");
            };
        }
        public UnitOfWork(string ConnectStr, string providerName)
        {
            _context = new SqlSugarClient(DBContexHelper.Contex(ConnectStr, providerName));
            int commandTimeout = 30;
            if (GlobalContext.SystemConfig != null)
            {
                commandTimeout = GlobalContext.SystemConfig.CommandTimeout;
            }
            _context.Ado.CommandTimeOut = commandTimeout;
            _context.CurrentConnectionConfig.ConfigureExternalServices = new ConfigureExternalServices()
            {
                DataInfoCacheService = new SqlSugarCache() //配置我们创建的缓存类
            };
            _context.Aop.OnLogExecuted = (sql, pars) => //SQL执行完
            {
                if (sql.StartsWith("SELECT"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[SELECT]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                }
                if (sql.StartsWith("INSERT"))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("[INSERT]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                }
                if (sql.StartsWith("UPDATE"))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[UPDATE]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                }
                if (sql.StartsWith("DELETE"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[DELETE]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                }
                Console.WriteLine("NeedTime-" + _context.Ado.SqlExecutionTime.ToString());
                //App.PrintToMiniProfiler("SqlSugar", "Info", sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine("Content:" + SqlProfiler.ParameterFormat(sql, pars));
                Console.WriteLine("---------------------------------");
                Console.WriteLine("");
            };
        }
        public void BeginTrans()
        {
            GetDbClient().BeginTran();
        }
        public void Commit()
        {
            try
            {
                GetDbClient().CommitTran();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex);
                GetDbClient().RollbackTran();
            }
        }
        public void Rollback()
        {
            try
            {
                GetDbClient().RollbackTran();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex);
            }

        }

		public SqlSugarClient GetDbClient()
		{
            // 必须要as，后边会用到切换数据库操作
            return _context as SqlSugarClient;
        }

		public void Dispose()
		{
            GetDbClient().Dispose();
		}

		public void CurrentBeginTrans()
		{
            GetDbClient().Ado.BeginTran();
        }

		public void CurrentCommit()
		{
            try
            {
                GetDbClient().Ado.CommitTran();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex);
                GetDbClient().Ado.RollbackTran();
            }
        }

		public void CurrentRollback()
		{
            try
            {
                GetDbClient().Ado.RollbackTran();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex);
            }
        }
    }
}
