using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.DataBase;
using SqlSugar;
using System.IO;
using System;
using System.Reflection;

namespace WaterCloud.Service.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-12 13:50
    /// 描 述：系统设置服务类
    /// </summary>
    public class SystemSetService : DataFilterService<SystemSetEntity>, IDenpendency
    {
        private string cacheKeyOperator = "watercloud_operator_";// +登录者token
        
        public SystemSetService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #region 获取数据
        public async Task<List<SystemSetEntity>> GetList(string keyword = "")
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(t => t.F_CompanyName.Contains(keyword) || t.F_ProjectName.Contains(keyword));
            }
            return await query.Where(t => t.F_DeleteMark == false).OrderBy(a => a.F_Id, OrderByType.Desc).ToListAsync();
        }

        public async Task<List<SystemSetEntity>> GetLookList(string keyword = "")
        {
            var query = repository.IQueryable().Where(u => u.F_DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u => u.F_CompanyName.Contains(keyword) || u.F_ProjectName.Contains(keyword));
            }
            query = GetDataPrivilege("u", "", query);
            return await query.OrderBy(a => a.F_Id, OrderByType.Desc).ToListAsync();
        }

        public async Task<SystemSetEntity> GetFormByHost(string host)
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(host))
            {
                //此处需修改
                query = query.Where(t => t.F_HostUrl.Contains(host));
            }
            else
            {
                query = query.Where(t => t.F_Id==GlobalContext.SystemConfig.SysemMasterProject);
            }
            if (query.Clone().Count()==0)
            {
                query = repository.IQueryable();
                query = query.Where(t => t.F_Id == GlobalContext.SystemConfig.SysemMasterProject);
            }
            return await query.Where(t => t.F_DeleteMark == false).FirstAsync();
        }

        public async Task<List<SystemSetEntity>> GetLookList(Pagination pagination,string keyword = "")
        {
            var query = repository.IQueryable().Where(u => u.F_DeleteMark == false&&u.F_DbNumber!="0");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(u => u.F_CompanyName.Contains(keyword) || u.F_ProjectName.Contains(keyword));
            }
            query = GetDataPrivilege("u", "", query);
            return await repository.OrderList(query, pagination);
        }

        public async Task<SystemSetEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        #endregion

        public async Task<SystemSetEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }

        #region 提交数据
        public async Task SubmitForm(SystemSetEntity entity, string keyValue)
        {
            unitofwork.BeginTrans();
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.F_DeleteMark = false;
                entity.Create();
                unitofwork.GetDbClient().ChangeDatabase("0");
                await repository.Insert(entity);
				//新建数据库和表
				using (var db = new SqlSugarClient(DBContexHelper.Contex(entity.F_DbString, entity.F_DBProvider)))
				{
                    db.DbMaintenance.CreateDatabase();
                    var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
                    //反射取指定前后缀的dll
                    var referencedAssemblies = Directory.GetFiles(path, "WaterCloud.Domain.dll").Select(Assembly.LoadFrom).ToArray();
                    var types = referencedAssemblies.SelectMany(a => a.GetTypes().ToArray());
                    var implementType = types.Where(x => x.IsClass);
					foreach (var item in implementType)
					{
						if (item.GetCustomAttributes(typeof(SugarTable), true).FirstOrDefault((x => x.GetType() == typeof(SugarTable))) is SugarTable sugarTable)
						{
                            db.CodeFirst.SetStringDefaultLength(50).InitTables(item);
                        } 
                    }
                }
                //新建菜单
                //新建账户,密码
                //新建租户权限 同时增加定时器 定时同步租户菜单
                //待设计
            }
            else
            {
                entity.Modify(keyValue);
                //更新主库
                unitofwork.GetDbClient().ChangeDatabase("0");
                if (currentuser.UserId != GlobalContext.SystemConfig.SysemUserId || currentuser.UserId == null)
                {
                    unitofwork.CurrentBeginTrans();
                    var user = repository.Db.Queryable<UserEntity>().Where(a => a.F_OrganizeId == entity.F_Id && a.F_IsAdmin == true).First();
                    var userinfo = repository.Db.Queryable<UserLogOnEntity>().Where(a => a.F_UserId == user.F_Id).First();
                    userinfo.F_UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                    userinfo.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(entity.F_AdminPassword, 32).ToLower(), userinfo.F_UserSecretkey).ToLower(), 32).ToLower();
                    await repository.Db.Updateable<UserEntity>(a => new UserEntity
                    {
                        F_Account = entity.F_AdminAccount
                    }).Where(a => a.F_Id == user.F_Id).ExecuteCommandAsync();
                    await repository.Db.Updateable<UserLogOnEntity>(a => new UserLogOnEntity
                    {
                        F_UserPassword = userinfo.F_UserPassword,
                        F_UserSecretkey = userinfo.F_UserSecretkey
                    }).Where(a => a.F_Id == userinfo.F_Id).ExecuteCommandAsync();
                    await repository.Db.Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    unitofwork.CurrentCommit();
                }
                else
                {
                    entity.F_AdminAccount = null;
                    entity.F_AdminPassword = null;
                    await unitofwork.GetDbClient().Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
                //更新租户库
                if (GlobalContext.SystemConfig.SqlMode == Define.SQL_TENANT)
				{
                    var tenant = await unitofwork.GetDbClient().Queryable<SystemSetEntity>().InSingleAsync(entity.F_Id);
                    unitofwork.GetDbClient().ChangeDatabase(tenant.F_DbNumber);
                    var user = unitofwork.GetDbClient().Queryable<UserEntity>().Where(a => a.F_OrganizeId == entity.F_Id && a.F_IsAdmin == true).First();
                    var userinfo = unitofwork.GetDbClient().Queryable<UserLogOnEntity>().Where(a => a.F_UserId == user.F_Id).First();
                    userinfo.F_UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                    userinfo.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(entity.F_AdminPassword, 32).ToLower(), userinfo.F_UserSecretkey).ToLower(), 32).ToLower();
                    await unitofwork.GetDbClient().Updateable<UserEntity>(a => new UserEntity
                    {
                        F_Account = entity.F_AdminAccount
                    }).Where(a => a.F_Id == user.F_Id).ExecuteCommandAsync();
                    await unitofwork.GetDbClient().Updateable<UserLogOnEntity>(a => new UserLogOnEntity
                    {
                        F_UserPassword = userinfo.F_UserPassword,
                        F_UserSecretkey = userinfo.F_UserSecretkey
                    }).Where(a => a.F_Id == userinfo.F_Id).ExecuteCommandAsync();
                    await unitofwork.GetDbClient().Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    //更新租户权限
                    //更新菜单

                }
            }
            unitofwork.Commit();
            var set=await unitofwork.GetDbClient().Queryable<SystemSetEntity>().InSingleAsync(entity.F_Id);
            unitofwork.GetDbClient().ChangeDatabase(GlobalContext.SystemConfig.SqlMode == Define.SQL_TENANT?set.F_DbNumber:"0");
            var tempkey= unitofwork.GetDbClient().Queryable<UserEntity>().Where(a => a.F_IsAdmin == true).First().F_Id;
            await CacheHelper.Remove(cacheKeyOperator + "info_" + tempkey);
        }

        public async Task DeleteForm(string keyValue)
        {
            await repository.Update(t => t.F_Id == keyValue,a=>new SystemSetEntity { 
                F_DeleteMark=true,
                F_EnabledMark=false,
                F_DeleteUserId=currentuser.UserId
            });
        }
        #endregion

    }
}
