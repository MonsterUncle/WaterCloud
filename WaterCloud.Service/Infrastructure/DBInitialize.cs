using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using WaterCloud.Code;
using WaterCloud.Code.Model;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Service
{
    /// <summary>
    /// 初始数据库操作类
    /// </summary>
	public class DBInitialize
	{
        private static string cacheKey = GlobalContext.SystemConfig.ProjectPrefix + "_dblist";// 数据库键
        /// <summary>
        /// 获取注册数据库list
        /// </summary>
        /// <param name="readDb">重置数据库list</param>
        /// <returns></returns>
        public static List<ConnectionConfig> GetConnectionConfigs(bool readDb=false)
		{
            List<ConnectionConfig> list = CacheHelper.Get<List<ConnectionConfig>>(cacheKey);
            if (list == null || !list.Any() || readDb)
			{
                list=new List<ConnectionConfig>();
                var data = GlobalContext.SystemConfig;
                var defaultConfig = DBContexHelper.Contex(data.DBConnectionString, data.DBProvider);
                defaultConfig.ConfigId = "0";
                list.Add(defaultConfig);
                try
                {
                    //租户数据库
					if (data.SqlMode== Define.SQL_TENANT)
					{
                        using (var context = new SqlSugarClient(defaultConfig))
                        {
                            var sqls = context.Queryable<SystemSetEntity>().ToList();
                            foreach (var item in sqls.Where(a => a.EnabledMark == true && a.EndTime > DateTime.Now.Date && a.DbNumber != "0"))
                            {
                                var config = DBContexHelper.Contex(item.DbString, item.DBProvider);
                                config.ConfigId = item.DbNumber;
                                list.Add(config);
                            }
                        }
                    }
                    if (data.SqlConfig == null)
                        data.SqlConfig = new List<DBConfig>();

                    //扩展数据库
                    foreach (var item in data.SqlConfig)
                    {
                        var config = DBContexHelper.Contex(item.DBConnectionString, item.DBProvider);
                        config.ConfigId = item.DBNumber;
						if (list.Any(a=>a.ConfigId == config.ConfigId))
						{
                            throw new Exception($"数据库编号重复，请检查{config.ConfigId}");
						}
                        list.Add(config);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteWithTime(ex);
                }
                CacheHelper.SetBySecond(cacheKey, list);
            }
            return list;
        }
    }
}
