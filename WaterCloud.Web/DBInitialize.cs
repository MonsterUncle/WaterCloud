using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Web
{
	public class DBInitialize
	{
        private static string cacheKey = GlobalContext.SystemConfig.ProjectPrefix + "_dblist";// 数据库键
        public static List<ConnectionConfig> GetConnectionConfigs()
		{
            List<ConnectionConfig> list = CacheHelper.Get<List<ConnectionConfig>>(cacheKey).Result;
            if (list == null || list.Count() == 0)
			{
                list=new List<ConnectionConfig>();
                var data = GlobalContext.SystemConfig;
                var defaultConfig = DBContexHelper.Contex(data.DBConnectionString, data.DBProvider);
                defaultConfig.ConfigId = "0";
                list.Add(defaultConfig);
                if (data.SqlMode == "TenantSql")
                {
                    try
                    {
                        using (var context = new UnitOfWork(new SqlSugarClient(defaultConfig)))
                        {
                            var sqls = context.GetDbClient().Queryable<SystemSetEntity>().ToList();
                            foreach (var item in sqls.Where(a => a.F_EnabledMark == true && a.F_EndTime > DateTime.Now.Date && a.F_DbNumber != "0"))
                            {
                                var config = DBContexHelper.Contex(item.F_DbString, item.F_DBProvider);
                                config.ConfigId = item.F_DbNumber;
                                list.Add(config);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteWithTime(ex);
                    }
                }
                else
                {
                    try
                    {
                        foreach (var item in data.SqlConfig)
                        {
                            var config = DBContexHelper.Contex(item.DBConnectionString, item.DBProvider);
                            config.ConfigId = item.DBNumber;
                            list.Add(config);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteWithTime(ex);
                    }
                }
                CacheHelper.Set(cacheKey, list);
            }
            return list;
        }
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
                        var systemSet = context.GetDbClient().Queryable<SystemSetEntity>().First(a => a.F_DbNumber == data.MainDbNumber);
                        var user = context.GetDbClient().Queryable<UserEntity>().First(a => a.F_Id == systemSet.F_Id && a.F_IsAdmin == true);
                        var userinfo = context.GetDbClient().Queryable<UserLogOnEntity>().Where(a => a.F_UserId == user.F_Id).First();
                        userinfo.F_UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                        userinfo.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(systemSet.F_AdminPassword, 32).ToLower(), userinfo.F_UserSecretkey).ToLower(), 32).ToLower();
                        context.GetDbClient().Updateable<UserEntity>(a => new UserEntity
                        {
                            F_Account = systemSet.F_AdminAccount
                        }).Where(a => a.F_Id == userinfo.F_Id).ExecuteCommand();
                        context.GetDbClient().Updateable<UserLogOnEntity>(a => new UserLogOnEntity
                        {
                            F_UserPassword = userinfo.F_UserPassword,
                            F_UserSecretkey = userinfo.F_UserSecretkey
                        }).Where(a => a.F_Id == userinfo.F_Id).ExecuteCommand();
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
