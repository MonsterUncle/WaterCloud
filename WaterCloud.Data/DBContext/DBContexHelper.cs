using SqlSugar;
using System;
using System.Linq;
using WaterCloud.Code;

namespace WaterCloud.DataBase
{
	public class DBContexHelper
	{
		private static string defaultDbType = GlobalContext.SystemConfig.DBProvider;
		private static string defaultDbConnectionString = GlobalContext.SystemConfig.DBConnectionString;

		public static ConnectionConfig Contex(string ConnectStr = "", string providerName = "")
		{
			ConnectStr = string.IsNullOrEmpty(ConnectStr) ? defaultDbConnectionString : ConnectStr;
			providerName = string.IsNullOrEmpty(providerName) ? defaultDbType : providerName;
			var dbType = Convert.ToInt32(Enum.Parse(typeof(DbType), providerName));
			if (dbType == Convert.ToInt32(DbType.SqlServer))
			{
				return new ConnectionConfig()
				{
					DbType = (DbType)dbType,
					InitKeyType = InitKeyType.Attribute,
					IsAutoCloseConnection = true,
					ConnectionString = ConnectStr,
					MoreSettings = new ConnMoreSettings()
					{
						IsWithNoLockQuery = true,
						IsAutoRemoveDataCache = true//自动清理缓存
					},
					ConfigureExternalServices = new ConfigureExternalServices()
					{
						DataInfoCacheService = new SqlSugarCache(), //配置我们创建的缓存类
						EntityService = (property, column) =>
						{
							var attributes = property.GetCustomAttributes(true);//get all attributes

							if (attributes.Any(it => it is SugarColumn) && column.DataType == "longtext")
							{
								column.DataType = "nvarchar(4000)";
							}
						}
					}
				};
			}
			else
			{
				return new ConnectionConfig()
				{
					DbType = (DbType)dbType,
					InitKeyType = InitKeyType.Attribute,
					IsAutoCloseConnection = true,
					ConnectionString = ConnectStr,
					ConfigureExternalServices = new ConfigureExternalServices()
					{
						DataInfoCacheService = new SqlSugarCache() //配置我们创建的缓存类
					},
					MoreSettings = new ConnMoreSettings()
					{
						IsAutoRemoveDataCache = true//自动清理缓存
					}
				};
			}
		}
	}
}