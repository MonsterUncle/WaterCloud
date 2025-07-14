using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.AdoJobStore.Common;
using Quartz.Spi;
using RabbitMQ.Client;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Service.AutoJob;
using WaterCloud.Service.Event;

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
						db.GetConnection(item.ConfigId).DefaultConfig();
					}
				});
			//初始化数据库
			foreach (var item in configList)
			{
				var db = sqlSugarScope.GetConnection(item.ConfigId);
                if (GlobalContext.SystemConfig.IsInitDb)
                {
                    InitDb(item, db);
                }
                if (GlobalContext.SystemConfig.IsSeedData)
                {
                    InitSeedData(item, db);
                }
            }
            //注入数据库连接
            // 注册 SqlSugar
            services.AddSingleton<ISqlSugarClient>(sqlSugarScope);
			return services;
		}

		/// <summary>
		/// sqlsugar配置
		/// </summary>
		/// <param name="db"></param>
		public static void DefaultConfig(this ISqlSugarClient db)
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
					if (attributes.Any(it => it is SugarColumn) && column.DataType == "longtext" && db.CurrentConnectionConfig.DbType == SqlSugar.DbType.SqlServer)
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


        private static void InitSeedData(ConnectionConfig config, SqlSugarProvider db)
        {
            var entityTypes = GlobalContext.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.IsDefined(typeof(SugarTable), false));
            if (!entityTypes.Any()) return;//没有就退出

            // 获取所有种子配置-初始化数据
            var seedDataTypes = GlobalContext.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
                && u.GetInterfaces().Any(i => i.HasImplementedRawGeneric(typeof(ISqlSugarEntitySeedData<>))));
            if (!seedDataTypes.Any()) return;
            foreach (var seedType in seedDataTypes)//遍历种子类
            {
                //使用与指定参数匹配程度最高的构造函数来创建指定类型的实例。
                var instance = Activator.CreateInstance(seedType);
                //获取SeedData方法
                var hasDataMethod = seedType.GetMethod("SeedData");
                //判断是否有种子数据
                var seedData = ((IEnumerable)hasDataMethod?.Invoke(instance, null))?.Cast<object>();
                if (seedData == null) continue;//没有种子数据就下一个
                var entityType = seedType.GetInterfaces().First().GetGenericArguments().First();//获取实体类型
                var tenantAtt = entityType.GetCustomAttribute<TenantAttribute>();//获取sqlsugar租户特性
                if (tenantAtt != null && tenantAtt.configId.ToString() != config.ConfigId.ToString()) continue;//如果不是当前租户的就下一个
                var seedDataTable = seedData.ToList().ToDataTable();//获取种子数据
                seedDataTable.TableName = db.EntityMaintenance.GetEntityInfo(entityType).DbTableName;//获取表名

                if (seedDataTable.Columns.Contains("F_Id"))//判断种子数据是否有主键
                {
                    var storage = db.Storageable(seedDataTable).WhereColumns("F_Id").ToStorage();

                    //codefirst暂时全部新增,根据主键更新,用户表暂不更新
                    storage.AsInsertable.ExecuteCommand();

                    var ignoreUpdate = hasDataMethod.GetCustomAttribute<IgnoreSeedDataUpdateAttribute>();//读取忽略更新特性
                    if (ignoreUpdate == null)
                        storage.AsUpdateable.ExecuteCommand();//只有没有忽略更新的特性才执行更新
                }
                else // 没有主键或者不是预定义的主键(有重复的可能)
                {
                    //全量插入
                    var storage = db.Storageable(seedDataTable).ToStorage();
                    storage.AsInsertable.ExecuteCommand();
                }
            }
        }

        /// <summary>
        /// 初始化数据库表结构
        /// </summary>
        /// <param name="config">数据库配置</param>
        private static void InitDb(ConnectionConfig config, SqlSugarProvider db)
        {
            try
            {
                db.DbMaintenance.CreateDatabase();//创建数据库
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建数据库失败,开始尝试操作表! ex:{ex.Message}");
            }

            var entityTypes = GlobalContext.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.IsDefined(typeof(SugarTable), false));
            if (!entityTypes.Any()) return;//没有就退出
            foreach (var entityType in entityTypes)
            {
                var tenantAtt = entityType.GetCustomAttribute<TenantAttribute>();//获取Sqlsugar多租户特性
                var ignoreInit = entityType.GetCustomAttribute<IgnoreInitTableAttribute>();//获取忽略初始化特性
                if (ignoreInit != null) continue;//如果有忽略初始化特性
                if (tenantAtt != null && tenantAtt.configId.ToString() != config.ConfigId.ToString()) continue;//如果特性存在并且租户ID不是当前数据库ID
                var splitTable = entityType.GetCustomAttribute<SplitTableAttribute>();//获取自动分表特性

                if (splitTable == null)//如果特性是空
                {
                    db.CodeFirst.SetStringDefaultLength(50).InitTables(entityType);//普通创建

                }
                else
                    db.CodeFirst.SetStringDefaultLength(50).SplitTables().InitTables(entityType);//自动分表创建
            }
        }

        /// <summary>
        /// 判断类型是否实现某个泛型
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="generic">泛型类型</param>
        /// <returns>bool</returns>
        public static bool HasImplementedRawGeneric(this System.Type type, System.Type generic)
        {
            // 检查接口类型
            var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
            if (isTheRawGenericType) return true;

            // 检查类型
            while (type != null && type != typeof(object))
            {
                isTheRawGenericType = IsTheRawGenericType(type);
                if (isTheRawGenericType) return true;
                type = type.BaseType;
            }

            return false;

            // 判断逻辑
            bool IsTheRawGenericType(System.Type type) => generic == (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
        }

        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            DataTable result = new();
            if (list.Count > 0)
            {
                // result.TableName = list[0].GetType().Name; // 表名赋值
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    System.Type colType = pi.PropertyType;
                    if (colType.IsGenericType && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    if (IsIgnoreColumn(pi))
                        continue;
                    if (IsJsonColumn(pi))//如果是json特性就是sting类型
                        colType = typeof(string);
                    result.Columns.Add(pi.Name, colType);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (IsIgnoreColumn(pi))
                            continue;
                        object obj = pi.GetValue(list[i], null);
                        if (IsJsonColumn(pi))//如果是json特性就是转化为json格式
                            obj = obj?.ToJson();//如果json字符串是空就传null
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// 排除SqlSugar忽略的列
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        private static bool IsIgnoreColumn(PropertyInfo pi)
        {
            var sc = pi.GetCustomAttributes<SugarColumn>(false).FirstOrDefault(u => u.IsIgnore == true);
            return sc != null;
        }

        private static bool IsJsonColumn(PropertyInfo pi)
        {
            var sc = pi.GetCustomAttributes<SugarColumn>(false).FirstOrDefault(u => u.IsJson == true);
            return sc != null;
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
						ParameterDbType = typeof(System.Data.DbType),
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
						var user = context.Queryable<UserEntity>().First(a => a.F_CompanyId == systemSet.F_Id && a.F_IsAdmin == true);
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
		/// <summary>
		/// 注册事件总线
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddEventBus(this IServiceCollection services)
		{
			// 注册 EventBus 服务
			return services.AddEventBus(builder =>
			{
				if (GlobalContext.SystemConfig.RabbitMq.Enabled)
				{
					// 创建连接工厂
					var factory = new ConnectionFactory
					{
						HostName= GlobalContext.SystemConfig.RabbitMq.HostName,
						VirtualHost= GlobalContext.SystemConfig.RabbitMq.VirtualHost,
						Port= GlobalContext.SystemConfig.RabbitMq.Port,
						UserName = GlobalContext.SystemConfig.RabbitMq.UserName,
						Password = GlobalContext.SystemConfig.RabbitMq.Password
					};
					// 创建默认内存通道事件源对象，可自定义队列路由key，比如这里是 eventbus
					var rbmqEventSourceStorer = new RabbitMQEventSourceStorer(factory, GlobalContext.SystemConfig.ProjectPrefix 
						+ "_eventbus", 3000);
					// 替换默认事件总线存储器
					builder.ReplaceStorer(serviceProvider =>
					{
						return rbmqEventSourceStorer;
					});
				}
				// 注册 ToDo 事件订阅者
				builder.AddSubscriber<LogEventSubscriber>();
				builder.AddSubscriber<MessageEventSubscriber>();
			});
		}
	}
}