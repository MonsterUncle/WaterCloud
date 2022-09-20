using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.AdoJobStore.Common;
using Quartz.Spi;
using SqlSugar;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Service.AutoJob;

namespace WaterCloud.Service
{
	/// <summary>
	/// 服务设置
	/// </summary>
	public static class ServiceSetup
	{
		/// <summary>
		/// SqlSugar设置
		/// </summary>
		/// <param name="services"></param>
		public static IServiceCollection AddSqlSugar(this IServiceCollection services)
		{
			var configList = DBInitialize.GetConnectionConfigs(true);
			SqlSugarScope sqlSugarScope = new SqlSugarScope(configList,
				//全局上下文生效
				db =>
				{
					foreach (var item in configList)
					{
						string temp = item.ConfigId;
						db.GetConnection(temp).DefaultConfig();
					}
				});
			//注入数据库连接
			// 注册 SqlSugar
			services.AddSingleton<ISqlSugarClient>(sqlSugarScope);
			return services;
		}

		/// <summary>
		/// sqlsugar配置
		/// </summary>
		/// <param name="db"></param>
		public static void DefaultConfig(this SqlSugarProvider db)
		{
			db.Ado.CommandTimeOut = GlobalContext.SystemConfig.DBCommandTimeout;
			db.CurrentConnectionConfig.ConfigureExternalServices = new ConfigureExternalServices()
			{
				DataInfoCacheService = new SqlSugarCache(), //配置我们创建的缓存类
				EntityNameService = (type, entity) =>
				{
					var attributes = type.GetCustomAttributes(true);
					if (attributes.Any(it => it is TableAttribute))
					{
						entity.DbTableName = (attributes.First(it => it is TableAttribute) as TableAttribute).Name;
					}
				},
				EntityService = (property, column) =>
				{
					var attributes = property.GetCustomAttributes(true);//get all attributes
					if (attributes.Any(it => it is KeyAttribute))// by attribute set primarykey
					{
						column.IsPrimarykey = true; //有哪些特性可以看 1.2 特性明细
					}
					if (attributes.Any(it => it is ColumnAttribute))
					{
						column.DbColumnName = (attributes.First(it => it is ColumnAttribute) as ColumnAttribute).Name;
					}
					if (attributes.Any(it => it is SugarColumn) && column.DataType == "longtext" && db.CurrentConnectionConfig.DbType == DbType.SqlServer)
					{
						column.DataType = "nvarchar(4000)";
					}
				}
			};
			db.Aop.OnLogExecuted = (sql, pars) => //SQL执行完
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
				Console.WriteLine($"执行库{db.CurrentConnectionConfig.ConfigId}");
				Console.WriteLine("NeedTime-" + db.Ado.SqlExecutionTime.ToString());
				//App.PrintToMiniProfiler("SqlSugar", "Info", sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
				Console.WriteLine("Content:" + UtilMethods.GetSqlString(db.CurrentConnectionConfig.DbType, sql, pars));
				Console.WriteLine("---------------------------------");
				Console.WriteLine("");
			};
		}

		/// <summary>
		/// Quartz设置
		/// </summary>
		/// <param name="services"></param>
		public static IServiceCollection AddQuartz(this IServiceCollection services)
		{
			services.AddSingleton<JobExecute>();
			//注册ISchedulerFactory的实例。
			services.AddSingleton<IJobFactory, IOCJobFactory>();
			if (GlobalContext.SystemConfig.IsCluster != true)
			{
				services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
			}
			else
			{
				//开启集群模式,具体数据库从官方github下载
				//https://github.com/quartznet/quartznet/blob/main/database/tables/
				services.AddSingleton<ISchedulerFactory>(u =>
				{
					//当前是mysql的例子，其他数据库做相应更改
					DbProvider.RegisterDbMetadata("mysql-custom", new DbMetadata()
					{
						AssemblyName = typeof(MySqlConnection).Assembly.GetName().Name,
						ConnectionType = typeof(MySqlConnection),
						CommandType = typeof(MySqlCommand),
						ParameterType = typeof(MySqlParameter),
						ParameterDbType = typeof(DbType),
						ParameterDbTypePropertyName = "DbType",
						ParameterNamePrefix = "@",
						ExceptionType = typeof(MySqlException),
						BindByName = true
					});
					var properties = new NameValueCollection
					{
						["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz", // 配置Quartz以使用JobStoreTx
																								 //["quartz.jobStore.useProperties"] = "true", // 配置AdoJobStore以将字符串用作JobDataMap值
						["quartz.jobStore.dataSource"] = "myDS", // 配置数据源名称
						["quartz.jobStore.tablePrefix"] = "QRTZ_", // quartz所使用的表，在当前数据库中的表前缀
						["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.MySQLDelegate, Quartz",  // 配置AdoJobStore使用的DriverDelegate
						["quartz.dataSource.myDS.connectionString"] = GlobalContext.SystemConfig.DBConnectionString, // 配置数据库连接字符串，自己处理好连接字符串，我这里就直接这么写了
						["quartz.dataSource.myDS.provider"] = "mysql-custom", // 配置数据库提供程序（这里是自定义的，定义的代码在上面）
						["quartz.jobStore.lockHandler.type"] = "Quartz.Impl.AdoJobStore.UpdateLockRowSemaphore, Quartz",
						["quartz.serializer.type"] = "json",
						["quartz.jobStore.clustered"] = "true",    //  指示Quartz.net的JobStore是应对 集群模式
						["quartz.scheduler.instanceId"] = "AUTO"
					};
					return new StdSchedulerFactory(properties);
				});
			}
			//是否开启后台任务
			if (GlobalContext.SystemConfig.OpenQuartz == true)
			{
				services.AddHostedService<JobCenter>();
			}
			return services;
		}

		/// <summary>
		/// 重置超管密码
		/// </summary>
		public static IServiceCollection ReviseSuperSysem(this IServiceCollection services)
		{
			var data = GlobalContext.SystemConfig;
			try
			{
				if (data.ReviseSystem == true)
				{
					using (var context = new SqlSugarClient(DBContexHelper.Contex()))
					{
						context.Ado.BeginTran();
						var systemSet = context.Queryable<SystemSetEntity>().First(a => a.F_DbNumber == data.MainDbNumber);
						var user = context.Queryable<UserEntity>().First(a => a.F_OrganizeId == systemSet.F_Id && a.F_IsAdmin == true);
						var userinfo = context.Queryable<UserLogOnEntity>().Where(a => a.F_UserId == user.F_Id).First();
						userinfo.F_UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
						userinfo.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(systemSet.F_AdminPassword, 32).ToLower(), userinfo.F_UserSecretkey).ToLower(), 32).ToLower();
						context.Updateable<UserEntity>(a => new UserEntity
						{
							F_Account = systemSet.F_AdminAccount
						}).Where(a => a.F_Id == userinfo.F_Id).ExecuteCommand();
						context.Updateable<UserLogOnEntity>(a => new UserLogOnEntity
						{
							F_UserPassword = userinfo.F_UserPassword,
							F_UserSecretkey = userinfo.F_UserSecretkey
						}).Where(a => a.F_Id == userinfo.F_Id).ExecuteCommand();
						context.Ado.CommitTran();
						CacheHelper.Remove(GlobalContext.SystemConfig.ProjectPrefix + "_operator_" + "info_" + user.F_Id);
					}
				}
			}
			catch (Exception ex)
			{
				LogHelper.Write(ex);
			}
			return services;
		}
	}
}