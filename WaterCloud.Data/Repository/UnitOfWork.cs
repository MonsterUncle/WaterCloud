/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using System;
using SqlSugar;
using WaterCloud.Code.Model;
using System.Linq;

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
			if (GlobalContext.SystemConfig!=null)
			{
                var current = OperatorProvider.Provider.GetCurrent();
                if (GlobalContext.SystemConfig.SqlMode == Define.SQL_TENANT && current != null && !string.IsNullOrEmpty(current.DbNumber))
                {
                    (context as SqlSugarClient).ChangeDatabase(current.DbNumber);
                }
            }
            _context = context;
        }
        public UnitOfWork(string ConnectStr, string providerName)
        {
            var list = GlobalContext.SystemConfig.SqlConfig.MapToList<DBConfig>();
            DBConfig config = list.FirstOrDefault(a => a.DBProvider == providerName && a.DBConnectionString == ConnectStr);
            var context = (SqlSugarClient)GlobalContext.ServiceProvider.GetService(typeof(ISqlSugarClient));
            _context = context;
            if (config == null)
			{
                config = new DBConfig();
                config.DBNumber = DateTime.Now.ToString();
                config.DBConnectionString = ConnectStr;
                config.DBProvider = providerName;
                list.Add(config);
                GlobalContext.SystemConfig.SqlConfig = list;
                if (!context.IsAnyConnection(config.DBNumber))
                {
                    var connect = DBContexHelper.Contex(ConnectStr, providerName);
                    connect.ConfigId = config.DBNumber;
                    context.AddConnection(connect);
                }
                //清除注入的数据库缓存
                CacheHelper.Remove(GlobalContext.SystemConfig.ProjectPrefix + "_dblist");
            }
            context.ChangeDatabase(config.DBNumber);
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
