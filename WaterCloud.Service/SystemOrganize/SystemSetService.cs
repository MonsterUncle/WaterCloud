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
        private string cacheKeyOperator = "watercloud_operator_";// +登录者token
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
        public async Task SubmitForm(SystemSetEntity entity, string keyValue, string[] permissionIds = null, string[] permissionfieldsIds = null)
        {
            unitofwork.GetDbClient().ChangeDatabase("0");
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
                entity.F_DeleteMark = false;
                entity.Create();
                await repository.Insert(entity);
                if (permissionIds != null)
                {
                    var moduledata = await moduleApp.GetList();
                    var buttondata = await moduleButtonApp.GetList();
                    foreach (var itemId in permissionIds.Distinct())
                    {
                        RoleAuthorizeEntity roleAuthorizeEntity = new RoleAuthorizeEntity();
                        roleAuthorizeEntity.F_Id = Utils.GuId();
                        roleAuthorizeEntity.F_ObjectType = 1;
                        roleAuthorizeEntity.F_ObjectId = entity.F_Id;
                        roleAuthorizeEntity.F_ItemId = itemId;
                        if (moduledata.Find(t => t.F_Id == itemId) != null)
                        {
                            roleAuthorizeEntity.F_ItemType = 1;
                            roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                            modules.Add(moduledata.Find(t => t.F_Id == itemId));
                        }
                        if (buttondata.Find(t => t.F_Id == itemId) != null)
                        {
                            roleAuthorizeEntity.F_ItemType = 2;
                            roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                            modulebtns.Add(buttondata.Find(t => t.F_Id == itemId));
                        }
                    }
                    modules.AddRange(moduledata.Where(a => a.F_IsPublic == true && a.F_EnabledMark == true && a.F_DeleteMark == false));
                    modulebtns.AddRange(buttondata.Where(a => a.F_IsPublic == true && a.F_EnabledMark == true && a.F_DeleteMark == false));
                }
                if (permissionfieldsIds != null)
                {
                    var fieldsdata = await moduleFieldsApp.GetList();
                    foreach (var itemId in permissionfieldsIds.Distinct())
                    {
                        RoleAuthorizeEntity roleAuthorizeEntity = new RoleAuthorizeEntity();
                        roleAuthorizeEntity.F_Id = Utils.GuId();
                        roleAuthorizeEntity.F_ObjectType = 1;
                        roleAuthorizeEntity.F_ObjectId = entity.F_Id;
                        roleAuthorizeEntity.F_ItemId = itemId;
                        if (fieldsdata.Find(t => t.F_Id == itemId) != null)
                        {
                            roleAuthorizeEntity.F_ItemType = 3;
                            roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                            modulefileds.Add(fieldsdata.Find(t => t.F_Id == itemId));
                        }
                    }
                    modulefileds.AddRange(fieldsdata.Where(a => a.F_IsPublic == true && a.F_EnabledMark == true && a.F_DeleteMark == false));
                }
                //新建租户权限
                if (roleAuthorizeEntitys.Count>0)
				{
                    await repository.Db.Insertable(roleAuthorizeEntitys).ExecuteCommandAsync();
                }
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
                    user.F_Account = entity.F_AdminAccount;
                    user.F_RealName = entity.F_CompanyName;
                    user.F_Gender = true;
                    user.F_OrganizeId = entity.F_Id;
                    user.F_IsAdmin = true;
                    user.F_DeleteMark = false;
                    user.F_EnabledMark = true;
                    user.F_IsBoss = false;
                    user.F_IsLeaderInDepts = false;
                    user.F_SortCode = 0;
                    user.F_IsSenior = false;
                    UserLogOnEntity logon = new UserLogOnEntity();
                    logon.F_Id = user.F_Id;
                    logon.F_UserId = user.F_Id;
                    logon.F_UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                    logon.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(entity.F_AdminPassword, 32).ToLower(), logon.F_UserSecretkey).ToLower(), 32).ToLower();
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
                        roleAuthorizeEntity.F_Id = Utils.GuId();
                        roleAuthorizeEntity.F_ObjectType = 1;
                        roleAuthorizeEntity.F_ObjectId = entity.F_Id;
                        roleAuthorizeEntity.F_ItemId = itemId;
                        if (moduledata.Find(t => t.F_Id == itemId) != null)
                        {
                            roleAuthorizeEntity.F_ItemType = 1;
                            roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                            modules.Add(moduledata.Find(t => t.F_Id == itemId));
                        }
                        if (buttondata.Find(t => t.F_Id == itemId) != null)
                        {
                            roleAuthorizeEntity.F_ItemType = 2;
                            roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                            modulebtns.Add(buttondata.Find(t => t.F_Id == itemId));
                        }
                    }
                }
                if (permissionfieldsIds != null)
                {
                    var fieldsdata = await moduleFieldsApp.GetList();
                    foreach (var itemId in permissionfieldsIds.Distinct())
                    {
                        RoleAuthorizeEntity roleAuthorizeEntity = new RoleAuthorizeEntity();
                        roleAuthorizeEntity.F_Id = Utils.GuId();
                        roleAuthorizeEntity.F_ObjectType = 1;
                        roleAuthorizeEntity.F_ObjectId = entity.F_Id;
                        roleAuthorizeEntity.F_ItemId = itemId;
                        if (fieldsdata.Find(t => t.F_Id == itemId) != null)
                        {
                            roleAuthorizeEntity.F_ItemType = 3;
                            roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                            modulefileds.Add(fieldsdata.Find(t => t.F_Id == itemId));
                        }
                    }
                }
                if (roleAuthorizeEntitys.Count>0)
				{
                    await repository.Db.Deleteable<RoleAuthorizeEntity>(t => t.F_ObjectId == entity.F_Id).ExecuteCommandAsync();
                    await repository.Db.Insertable(roleAuthorizeEntitys).ExecuteCommandAsync();
                }
                //更新主库
                if (currentuser.IsAdmin == true)
                {
                    await repository.Db.Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
                else
                {
                    entity.F_AdminAccount = null;
                    entity.F_AdminPassword = null;
                    await repository.Db.Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
                //更新租户库
                if (GlobalContext.SystemConfig.SqlMode == Define.SQL_TENANT)
				{
                    var tenant = await unitofwork.GetDbClient().Queryable<SystemSetEntity>().InSingleAsync(entity.F_Id);
                    unitofwork.GetDbClient().ChangeDatabase(tenant.F_DbNumber);
                    var user = unitofwork.GetDbClient().Queryable<UserEntity>().First(a => a.F_OrganizeId == entity.F_Id && a.F_IsAdmin == true);
                    var userinfo = unitofwork.GetDbClient().Queryable<UserLogOnEntity>().First(a => a.F_UserId == user.F_Id);
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
                    var user = repository.Db.Queryable<UserEntity>().First(a => a.F_OrganizeId == entity.F_Id && a.F_IsAdmin == true);
                    var userinfo = repository.Db.Queryable<UserLogOnEntity>().First(a => a.F_UserId == user.F_Id);
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
                }
                var set = await unitofwork.GetDbClient().Queryable<SystemSetEntity>().InSingleAsync(entity.F_Id);
                unitofwork.GetDbClient().ChangeDatabase(GlobalContext.SystemConfig.SqlMode == Define.SQL_TENANT ? set.F_DbNumber : "0");
                var tempkey = unitofwork.GetDbClient().Queryable<UserEntity>().First(a => a.F_IsAdmin == true).F_Id;
                await CacheHelper.Remove(cacheKeyOperator + "info_" + tempkey);
            }
            unitofwork.Commit();
            unitofwork.GetDbClient().ChangeDatabase("0");
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
