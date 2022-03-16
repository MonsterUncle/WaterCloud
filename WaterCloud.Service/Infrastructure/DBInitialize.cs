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
        /// <summary>
        /// 重置超管密码
        /// </summary>
        public static void ReviseSuperSysem()
        {
            var data = GlobalContext.SystemConfig;
            try
            {
                if (data.ReviseSysem == true)
                {
                    using (var context = new UnitOfWork(new SqlSugarClient(DBContexHelper.Contex())))
                    {
                        context.CurrentBeginTrans();
                        var systemSet = context.GetDbClient().Queryable<SystemSetEntity>().First(a => a.DbNumber == data.MainDbNumber);
                        var user = context.GetDbClient().Queryable<UserEntity>().First(a => a.OrganizeId == systemSet.Id && a.IsAdmin == true);
                        var userinfo = context.GetDbClient().Queryable<UserLogOnEntity>().Where(a => a.UserId == user.Id).First();
                        userinfo.UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                        userinfo.UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(systemSet.AdminPassword, 32).ToLower(), userinfo.UserSecretkey).ToLower(), 32).ToLower();
                        context.GetDbClient().Updateable<UserEntity>(a => new UserEntity
                        {
                            Account = systemSet.AdminAccount
                        }).Where(a => a.Id == userinfo.Id).ExecuteCommand();
                        context.GetDbClient().Updateable<UserLogOnEntity>(a => new UserLogOnEntity
                        {
                            UserPassword = userinfo.UserPassword,
                            UserSecretkey = userinfo.UserSecretkey
                        }).Where(a => a.Id == userinfo.Id).ExecuteCommand();
                        context.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex);
            }
        }
    }
}
