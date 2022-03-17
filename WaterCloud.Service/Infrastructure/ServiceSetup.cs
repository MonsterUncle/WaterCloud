using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.AdoJobStore.Common;
using Quartz.Spi;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Service.AutoJob;

namespace WaterCloud.Service
{
	public static class ServiceSetup
    {
		public static void AddSqlSugar(this IServiceCollection services)
		{
            //注入数据库连接
            // 注册 SqlSugar
            services.AddScoped<ISqlSugarClient>(u =>
            {
                var configList = DBInitialize.GetConnectionConfigs();
                var db = new SqlSugarClient(configList);
                configList.ForEach(config => {
                    string temp = config.ConfigId;
                    db.GetConnection(temp).Ado.CommandTimeOut = GlobalContext.SystemConfig.CommandTimeout;
                    db.GetConnection(temp).CurrentConnectionConfig.ConfigureExternalServices = new ConfigureExternalServices()
                    {
                        DataInfoCacheService = new SqlSugarCache(), //配置我们创建的缓存类
                        EntityService = (property, column) =>
                        {
                            var attributes = property.GetCustomAttributes(true);//get all attributes 

                            if (attributes.Any(it => it is SugarColumn) && column.DataType == "longtext" && config.DbType == DbType.SqlServer)
                            {
                                column.DataType = "nvarchar(4000)";
                            }
                        }
                    };
                    db.GetConnection(temp).Aop.OnLogExecuted = (sql, pars) => //SQL执行完
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
                        Console.WriteLine($"执行库{config.ConfigId}");
                        Console.WriteLine("NeedTime-" + db.Ado.SqlExecutionTime.ToString());
                        //App.PrintToMiniProfiler("SqlSugar", "Info", sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                        Console.WriteLine("Content:" + SqlProfiler.ParameterFormat(sql, pars));
                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("");
                    };
                });
                return db;
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        public static void AddQuartz(this IServiceCollection services)
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
                        ["quartz.dataSource.myDS.connectionString"] = GlobalContext. SystemConfig.DBConnectionString, // 配置数据库连接字符串，自己处理好连接字符串，我这里就直接这么写了
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
            if (GlobalContext.SystemConfig.OpenQuarz == true)
            {
                services.AddHostedService<JobCenter>();
            }
        }
        public static void AddAutofac(this ContainerBuilder builder,List<string> projects,Type controller,Type program)
		{
			if (projects == null)
			{
                projects = new List<string>();
                projects.Add("WaterCloud.Service");
            }
			foreach (var item in projects)
			{
                var assemblys = Assembly.Load(item);//Service是继承接口的实现方法类库名称
                var baseType = typeof(IDenpendency);//IDenpendency 是一个接口（所有要实现依赖注入的借口都要继承该接口）
                builder.RegisterAssemblyTypes(assemblys).Where(m => baseType.IsAssignableFrom(m) && m != baseType)
                  .InstancePerLifetimeScope()//生命周期，这里没有使用接口方式
                  .PropertiesAutowired();//属性注入
            }
            //Controller中使用属性注入
            var controllerBaseType = controller;
            builder.RegisterAssemblyTypes(program.Assembly)
            .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
            .PropertiesAutowired();
            //注册html解析
            builder.RegisterInstance(HtmlEncoder.Create(UnicodeRanges.All)).SingleInstance();
        }
    }
}
