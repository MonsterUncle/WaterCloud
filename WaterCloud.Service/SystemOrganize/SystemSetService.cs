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
using WaterCloud.Service.SystemManage;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.Service.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-12 13:50
    /// 描 述：系统设置服务类
    /// </summary>
    public class SystemSetService : DataFilterService<SystemSetEntity>, IDenpendency
    {
        private string cacheKeyOperator = GlobalContext.SystemConfig.ProjectPrefix + "_operator_";// +登录者token
        private static string cacheKey = GlobalContext.SystemConfig.ProjectPrefix + "_dblist";// 数据库键
        public ModuleService moduleApp { get; set; }
        public ModuleButtonService moduleButtonApp { get; set; }
        public ModuleFieldsService moduleFieldsApp { get; set; }

        public SystemSetService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #region 获取数据
        public async Task<List<SystemSetEntity>> GetList(string keyword = "")
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.CompanyName.Contains(keyword) || a.ProjectName.Contains(keyword));
            }
            return await query.Where(a => a.DeleteMark == false).OrderBy(a => a.Id, OrderByType.Desc).ToListAsync();
        }

        public async Task<List<SystemSetEntity>> GetLookList(string keyword = "")
        {
            var query = repository.IQueryable().Where(a => a.DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.CompanyName.Contains(keyword) || a.ProjectName.Contains(keyword));
            }
            query = GetDataPrivilege("a", "", query);
            return await query.OrderBy(a => a.Id, OrderByType.Desc).ToListAsync();
        }

        public async Task<SystemSetEntity> GetFormByHost(string host)
        {
            unitOfWork.GetDbClient().ChangeDatabase(GlobalContext.SystemConfig.MainDbNumber);
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(host))
            {
                //此处需修改
                query = query.Where(a => a.HostUrl.Contains(host));
            }
            else
            {
                query = query.Where(a => a.DbNumber == "0");
            }
            if (!await query.Clone().AnyAsync())
            {
                query = repository.IQueryable();
                query = query.Where(a => a.DbNumber == "0");
            }
            var data = await query.Where(a => a.DeleteMark == false).FirstAsync();
            return data;
        }

        public async Task<List<SystemSetEntity>> GetLookList(Pagination pagination,string keyword = "")
        {
            var query = repository.IQueryable().Where(a => a.DeleteMark == false && a.DbNumber != "0");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(a => a.CompanyName.Contains(keyword) || a.ProjectName.Contains(keyword));
            }
            query = GetDataPrivilege("a", "", query);
            return await query.ToPageListAsync(pagination);
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
        public async Task SubmitForm(SystemSetEntity entity, string keyValue, string[] permissionIds = null, string[] permissionfieldsIds = null)
        {
            unitofwork.GetDbClient().ChangeDatabase(GlobalContext.SystemConfig.MainDbNumber);
            List<RoleAuthorizeEntity> roleAuthorizeEntitys = new List<RoleAuthorizeEntity>();
            List<ModuleEntity> modules = new List<ModuleEntity>();
            List<ModuleButtonEntity> modulebtns = new List<ModuleButtonEntity>();
            List<ModuleFieldsEntity> modulefileds = new List<ModuleFieldsEntity>();
            //字典数据
            var itemsTypes = await unitofwork.GetDbClient().Queryable<ItemsEntity>().ToListAsync();
            var itemsDetails = await unitofwork.GetDbClient().Queryable<ItemsDetailEntity>().ToListAsync();
            unitofwork.BeginTrans();
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.DeleteMark = false;
                entity.Create();
                await repository.Insert(entity);
                if (permissionIds != null)
                {
                    var moduledata = await moduleApp.GetList();
                    var buttondata = await moduleButtonApp.GetList();
                    foreach (var itemId in permissionIds.Distinct())
                    {
                        RoleAuthorizeEntity roleAuthorizeEntity = new RoleAuthorizeEntity();
                        roleAuthorizeEntity.Id = Utils.GuId();
                        roleAuthorizeEntity.ObjectType = 1;
                        roleAuthorizeEntity.ObjectId = entity.Id;
                        roleAuthorizeEntity.ItemId = itemId;
                        if (moduledata.Find(a => a.Id == itemId) != null)
                        {
                            roleAuthorizeEntity.ItemType = 1;
                            roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                            modules.Add(moduledata.Find(a => a.Id == itemId));
                        }
                        if (buttondata.Find(a => a.Id == itemId) != null)
                        {
                            roleAuthorizeEntity.ItemType = 2;
                            roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                            modulebtns.Add(buttondata.Find(a => a.Id == itemId));
                        }
                    }
                    //排除租户
                    modules.AddRange(moduledata.Where(a => a.IsPublic == true && a.EnabledMark == true && a.DeleteMark == false && a.EnCode!= "SystemSet"));
                    modulebtns.AddRange(buttondata.Where(a => a.IsPublic == true && a.EnabledMark == true && a.DeleteMark == false));
                }
                if (permissionfieldsIds != null)
                {
                    var fieldsdata = await moduleFieldsApp.GetList();
                    foreach (var itemId in permissionfieldsIds.Distinct())
                    {
                        RoleAuthorizeEntity roleAuthorizeEntity = new RoleAuthorizeEntity();
                        roleAuthorizeEntity.Id = Utils.GuId();
                        roleAuthorizeEntity.ObjectType = 1;
                        roleAuthorizeEntity.ObjectId = entity.Id;
                        roleAuthorizeEntity.ItemId = itemId;
                        if (fieldsdata.Find(a => a.Id == itemId) != null)
                        {
                            roleAuthorizeEntity.ItemType = 3;
                            roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                            modulefileds.Add(fieldsdata.Find(a => a.Id == itemId));
                        }
                    }
                    modulefileds.AddRange(fieldsdata.Where(a => a.IsPublic == true && a.EnabledMark == true && a.DeleteMark == false));
                }
                //新建租户权限
                if (roleAuthorizeEntitys.Count>0)
				{
                    await repository.Db.Insertable(roleAuthorizeEntitys).ExecuteCommandAsync();
                }
                //新建数据库和表
                using (var db = new SqlSugarClient(DBContexHelper.Contex(entity.DbString, entity.DBProvider)))
				{
                    //判断数据库有没有被使用
                    db.DbMaintenance.CreateDatabase();
                    if (db.DbMaintenance.GetTableInfoList(false).Where(a=>a.Name.ToLower()== "sys_module").Any())
                        throw new Exception("数据库已存在,请重新设置数据库");
                    var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
                    //反射取指定前后缀的dll
                    var referencedAssemblies = Directory.GetFiles(path, "WaterCloud.Domain.dll").Select(Assembly.LoadFrom).ToArray();
                    var types = referencedAssemblies.SelectMany(a => a.GetTypes().ToArray());
                    var implementType = types.Where(x => x.IsClass);
                    foreach (var item in implementType)
                    {
                        try
                        {
                            if (item.GetCustomAttributes(typeof(SugarTable), true).FirstOrDefault((x => x.GetType() == typeof(SugarTable))) is SugarTable sugarTable)
                            {
                                db.CodeFirst.SetStringDefaultLength(50).InitTables(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            continue;
                        }

                    }
                    //新建账户,密码
                    UserEntity user = new UserEntity();
                    user.Create();
                    user.Account = entity.AdminAccount;
                    user.RealName = entity.CompanyName;
                    user.Gender = true;
                    user.OrganizeId = entity.Id;
                    user.IsAdmin = true;
                    user.DeleteMark = false;
                    user.EnabledMark = true;
                    user.IsBoss = false;
                    user.IsLeaderInDepts = false;
                    user.SortCode = 0;
                    user.IsSenior = false;
                    UserLogOnEntity logon = new UserLogOnEntity();
                    logon.Id = user.Id;
                    logon.UserId = user.Id;
                    logon.UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                    logon.UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(entity.AdminPassword, 32).ToLower(), logon.UserSecretkey).ToLower(), 32).ToLower();
                    db.Insertable(user).ExecuteCommand();
                    db.Insertable(logon).ExecuteCommand();
                    await db.Insertable(roleAuthorizeEntitys).ExecuteCommandAsync();
                    await db.Insertable(entity).ExecuteCommandAsync();
                    //新建菜单
                    await db.Insertable(modules).ExecuteCommandAsync();
                    await db.Insertable(modulebtns).ExecuteCommandAsync();
                    await db.Insertable(modulefileds).ExecuteCommandAsync();
                    //同步字典
                    await db.Deleteable<ItemsEntity>().ExecuteCommandAsync();
                    await db.Deleteable<ItemsDetailEntity>().ExecuteCommandAsync();
                    await db.Insertable(itemsTypes).ExecuteCommandAsync();
                    await db.Insertable(itemsDetails).ExecuteCommandAsync();
                }
            }
            else
            {
                entity.Modify(keyValue);
                if (permissionIds != null)
                {
                    var moduledata = await moduleApp.GetList();
                    var buttondata = await moduleButtonApp.GetList();
                    foreach (var itemId in permissionIds.Distinct())
                    {
                        RoleAuthorizeEntity roleAuthorizeEntity = new RoleAuthorizeEntity();
                        roleAuthorizeEntity.Id = Utils.GuId();
                        roleAuthorizeEntity.ObjectType = 1;
                        roleAuthorizeEntity.ObjectId = entity.Id;
                        roleAuthorizeEntity.ItemId = itemId;
                        if (moduledata.Find(a => a.Id == itemId) != null)
                        {
                            roleAuthorizeEntity.ItemType = 1;
                            roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                            modules.Add(moduledata.Find(a => a.Id == itemId));
                        }
                        if (buttondata.Find(a => a.Id == itemId) != null)
                        {
                            roleAuthorizeEntity.ItemType = 2;
                            roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                            modulebtns.Add(buttondata.Find(a => a.Id == itemId));
                        }
                    }
                }
                if (permissionfieldsIds != null)
                {
                    var fieldsdata = await moduleFieldsApp.GetList();
                    foreach (var itemId in permissionfieldsIds.Distinct())
                    {
                        RoleAuthorizeEntity roleAuthorizeEntity = new RoleAuthorizeEntity();
                        roleAuthorizeEntity.Id = Utils.GuId();
                        roleAuthorizeEntity.ObjectType = 1;
                        roleAuthorizeEntity.ObjectId = entity.Id;
                        roleAuthorizeEntity.ItemId = itemId;
                        if (fieldsdata.Find(a => a.Id == itemId) != null)
                        {
                            roleAuthorizeEntity.ItemType = 3;
                            roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                            modulefileds.Add(fieldsdata.Find(a => a.Id == itemId));
                        }
                    }
                }
                if (roleAuthorizeEntitys.Count>0)
				{
                    await repository.Db.Deleteable<RoleAuthorizeEntity>(a => a.ObjectId == entity.Id).ExecuteCommandAsync();
                    await repository.Db.Insertable(roleAuthorizeEntitys).ExecuteCommandAsync();
                }
                //更新主库
                if (currentuser.IsSuperAdmin == true)
                {
                    await repository.Db.Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
                else
                {
                    entity.AdminAccount = null;
                    entity.AdminPassword = null;
                    await repository.Db.Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
                //更新租户库
                if (GlobalContext.SystemConfig.SqlMode == Define.SQL_TENANT)
				{
                    var tenant = await unitofwork.GetDbClient().Queryable<SystemSetEntity>().InSingleAsync(entity.Id);
                    unitofwork.GetDbClient().ChangeDatabase(tenant.DbNumber);
                    var user = unitofwork.GetDbClient().Queryable<UserEntity>().First(a => a.OrganizeId == entity.Id && a.IsAdmin == true);
                    var userinfo = unitofwork.GetDbClient().Queryable<UserLogOnEntity>().First(a => a.UserId == user.Id);
                    userinfo.UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                    userinfo.UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(entity.AdminPassword, 32).ToLower(), userinfo.UserSecretkey).ToLower(), 32).ToLower();
                    await unitofwork.GetDbClient().Updateable<UserEntity>(a => new UserEntity
                    {
                        Account = entity.AdminAccount
                    }).Where(a => a.Id == user.Id).ExecuteCommandAsync();
                    await unitofwork.GetDbClient().Updateable<UserLogOnEntity>(a => new UserLogOnEntity
                    {
                        UserPassword = userinfo.UserPassword,
                        UserSecretkey = userinfo.UserSecretkey
                    }).Where(a => a.Id == userinfo.Id).ExecuteCommandAsync();
                    await unitofwork.GetDbClient().Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    //更新菜单
                    if (roleAuthorizeEntitys.Count > 0)
                    {
                        await unitofwork.GetDbClient().Deleteable<ModuleEntity>().ExecuteCommandAsync();
                        await unitofwork.GetDbClient().Deleteable<ModuleButtonEntity>().ExecuteCommandAsync();
                        await unitofwork.GetDbClient().Deleteable<ModuleFieldsEntity>().ExecuteCommandAsync();
                        await unitofwork.GetDbClient().Insertable(modules).ExecuteCommandAsync();
                        await unitofwork.GetDbClient().Insertable(modulebtns).ExecuteCommandAsync();
                        await unitofwork.GetDbClient().Insertable(modulefileds).ExecuteCommandAsync();
                    }
                    //同步字典
                    await unitofwork.GetDbClient().Deleteable<ItemsEntity>().ExecuteCommandAsync();
                    await unitofwork.GetDbClient().Deleteable<ItemsDetailEntity>().ExecuteCommandAsync();
                    await unitofwork.GetDbClient().Insertable(itemsTypes).ExecuteCommandAsync();
                    await unitofwork.GetDbClient().Insertable(itemsDetails).ExecuteCommandAsync();
                }
				else
				{
                    var user = repository.Db.Queryable<UserEntity>().First(a => a.OrganizeId == entity.Id && a.IsAdmin == true);
                    var userinfo = repository.Db.Queryable<UserLogOnEntity>().First(a => a.UserId == user.Id);
                    userinfo.UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                    userinfo.UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(entity.AdminPassword, 32).ToLower(), userinfo.UserSecretkey).ToLower(), 32).ToLower();
                    await repository.Db.Updateable<UserEntity>(a => new UserEntity
                    {
                        Account = entity.AdminAccount
                    }).Where(a => a.Id == user.Id).ExecuteCommandAsync();
                    await repository.Db.Updateable<UserLogOnEntity>(a => new UserLogOnEntity
                    {
                        UserPassword = userinfo.UserPassword,
                        UserSecretkey = userinfo.UserSecretkey
                    }).Where(a => a.Id == userinfo.Id).ExecuteCommandAsync();
                }
                var set = await unitofwork.GetDbClient().Queryable<SystemSetEntity>().InSingleAsync(entity.Id);
                unitofwork.GetDbClient().ChangeDatabase(GlobalContext.SystemConfig.SqlMode == Define.SQL_TENANT ? set.DbNumber : "0");
                var tempkey = unitofwork.GetDbClient().Queryable<UserEntity>().First(a => a.IsAdmin == true).Id;
                await CacheHelper.RemoveAsync(cacheKeyOperator + "info_" + tempkey);
            }
            unitofwork.Commit();
            unitofwork.GetDbClient().ChangeDatabase(GlobalContext.SystemConfig.MainDbNumber);
            //清空缓存，重新拉数据
            DBInitialize.GetConnectionConfigs(true);
        }

        public async Task DeleteForm(string keyValue)
        {
            await repository.Update(a => a.Id == keyValue,a=>new SystemSetEntity { 
                DeleteMark=true,
                EnabledMark=false,
                DeleteUserId=currentuser.UserId
            });
        }
        #endregion

    }
}
