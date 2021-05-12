/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using SqlSugar;

namespace WaterCloud.DataBase
{
    /// <summary>
    /// 非泛型仓储
    /// </summary>
    public class UnitOfWork: IUnitOfWork,IDisposable
    {
        private static int commandTimeout = GlobalContext.SystemConfig.CommandTimeout;
        private readonly ISqlSugarClient _context;
        public UnitOfWork(ISqlSugarClient context)
        {
            _context = context;
            _context.Ado.CommandTimeOut = commandTimeout;
            _context.Aop.OnLogExecuted = (sql, pars) => //SQL执行完
            {
                Console.Write("time:" + _context.Ado.SqlExecutionTime.ToString());//输出SQL执行时间
                Console.WriteLine(sql);
                Console.WriteLine(string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));
            };
        }
        public UnitOfWork(string ConnectStr, string providerName)
        {
            _context = new SqlSugarClient(DBContexHelper.Contex(ConnectStr, providerName));
            _context.Ado.CommandTimeOut = commandTimeout;
            _context.Aop.OnLogExecuted = (sql, pars) => //SQL执行完
            {
                Console.Write("time:" + _context.Ado.SqlExecutionTime.ToString());//输出SQL执行时间
                Console.WriteLine(sql);
                Console.WriteLine(string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));
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
	}
}
