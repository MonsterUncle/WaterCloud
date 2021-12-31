/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCloud.Service.SystemManage;
using WaterCloud.Domain.SystemManage;
using SqlSugar;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemOrganize
{
    public class RoleAuthorizeService : DataFilterService<RoleAuthorizeEntity>, IDenpendency
    {
        public ModuleService moduleApp { get; set; }
        public ModuleButtonService moduleButtonApp { get; set; }
        public ModuleFieldsService moduleFieldsApp { get; set; }
        public UserService userApp { get; set; }
        public RoleService roleApp { get; set; }
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private string cacheKey = GlobalContext.SystemConfig.ProjectPrefix + "_authorizeurldata_";// +权限
        public RoleAuthorizeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<RoleAuthorizeEntity>> GetList(string ObjectId)
        {
            var query = repository.IQueryable();
            query = query.Where(a => a.F_ObjectId == ObjectId);
            return await query.ToListAsync();
        }
        public async Task<List<ModuleEntity>> GetMenuList(string roleId)
        {
            var data = new List<ModuleEntity>();
            if (currentuser.IsAdmin)
            {
                data =await moduleApp.GetList();
                data = data.Where(a => a.F_IsMenu == true&&a.F_EnabledMark==true).ToList();
            }
            else
            {
                var rolelist = roleId.Split(',');
                var moduledata =await moduleApp.GetList();
                moduledata = moduledata.Where(a => a.F_IsMenu == true && a.F_EnabledMark == true).ToList();
                var role =repository.Db.Queryable<RoleEntity>().Where(a=>rolelist.Contains(a.F_Id)&&a.F_EnabledMark==true).ToList();
                if (role.Count==0)
                {
                    return data;
                }
                var authorizedata = repository.IQueryable().Where(a => rolelist.Contains(a.F_ObjectId) && a.F_ItemType == 1).Distinct().ToList();
                foreach (var item in authorizedata)
                {
                    ModuleEntity moduleEntity = moduledata.Find(a => a.F_Id == item.F_ItemId && a.F_IsPublic==false);
                    if (moduleEntity != null && data.Find(a=>a.F_Id==moduleEntity.F_Id)==null)
                    {
                        data.Add(moduleEntity);
                    }
                }
                data.AddRange(moduledata.Where(a => a.F_IsPublic == true));
            }
            return data.OrderBy(a => a.F_SortCode).ToList();
        }
        public async Task<List<ModuleButtonEntity>> GetButtonList(string roleId)
        {
            var data = new List<ModuleButtonEntity>();
            if (currentuser.IsAdmin)
            {
                data = await moduleButtonApp.GetListNew();
            }
            else
            {
                var buttondata = await moduleButtonApp.GetListNew();
                var role = await roleApp.GetForm(roleId);
                if (role == null || role.F_EnabledMark == false)
                {
                    return data;
                }
                var authorizedata = repository.IQueryable().Where(a => a.F_ObjectId == roleId && a.F_ItemType == 2).ToList();
                foreach (var item in authorizedata)
                {
                    ModuleButtonEntity moduleButtonEntity = buttondata.Find(a => a.F_Id == item.F_ItemId && a.F_IsPublic == false);
                    if (moduleButtonEntity != null)
                    {
                        data.Add(moduleButtonEntity);
                    }
                }
                data.AddRange(buttondata.Where(a => a.F_IsPublic == true));
            }
            return data.OrderBy(a => a.F_SortCode).ToList();
        }
        public async Task<List<ModuleFieldsEntity>> GetFieldsList(string roleId)
        {
            var data = new List<ModuleFieldsEntity>();
            if (currentuser.IsAdmin)
            {
                data = await moduleFieldsApp.GetListNew();
            }
            else
            {
                var fieldsdata = await moduleFieldsApp.GetListNew();
                var role = await roleApp.GetForm(roleId);
                if (role == null || role.F_EnabledMark == false)
                {
                    return data;
                }
                var authorizedata = repository.IQueryable().Where(a => a.F_ObjectId == roleId && a.F_ItemType == 3).ToList();
                foreach (var item in authorizedata)
                {
                    ModuleFieldsEntity moduleFieldsEntity = fieldsdata.Where(a => a.F_Id == item.F_ItemId && a.F_IsPublic == false).FirstOrDefault();
                    if (moduleFieldsEntity != null)
                    {
                        data.Add(moduleFieldsEntity);
                    }
                }
                data.AddRange(fieldsdata.Where(a => a.F_IsPublic == true));
            }
            return data.OrderByDescending(a => a.F_CreatorTime).ToList();
        }
        public async Task<bool> ActionValidate(string action,bool isAuthorize=false)
        {
            var user = await userApp.GetForm(currentuser.UserId);
            if (user == null || user.F_EnabledMark == false)
            {
                return false;
            }
            var authorizeurldata = new List<AuthorizeActionModel>();
            var cachedata =await CacheHelper.Get<Dictionary<string,List<AuthorizeActionModel>>>(cacheKey +repository.Db.CurrentConnectionConfig.ConfigId+ "_list");
            if (cachedata == null)
            {
                cachedata = new Dictionary<string, List<AuthorizeActionModel>>();
            }
            if (user.F_IsAdmin == true)
            {
                if (await unitofwork.GetDbClient().Queryable<ModuleEntity>().Where(a => a.F_UrlAddress == action).AnyAsync() || await unitofwork.GetDbClient().Queryable<ModuleButtonEntity>().Where(a => a.F_UrlAddress == action).AnyAsync())
                {
                    return true;
                }
                return false;
            }
            else
			{
                var rolelist = user.F_RoleId.Split(',');
                foreach (var roles in rolelist)
                {
                    if (!cachedata.ContainsKey(roles))
                    {
                        var moduledata = await moduleApp.GetList();
                        moduledata = moduledata.Where(a => a.F_EnabledMark == true).ToList();
                        var buttondata = await moduleButtonApp.GetList();
                        buttondata = buttondata.Where(a => a.F_EnabledMark == true).ToList();
                        var role = await roleApp.GetForm(roles);
                        if (role != null && role.F_EnabledMark == true)
                        {
                            var authdata = new List<AuthorizeActionModel>();
                            var authorizedata = await GetList(roles);
                            foreach (var item in authorizedata)
                            {
                                try
                                {
                                    if (item.F_ItemType == 1)
                                    {
                                        ModuleEntity moduleEntity = moduledata.Where(a => a.F_Id == item.F_ItemId && a.F_IsPublic == false).FirstOrDefault();
                                        if (moduleEntity != null)
                                        {
                                            authdata.Add(new AuthorizeActionModel { F_Id = moduleEntity.F_Id, F_UrlAddress = moduleEntity.F_UrlAddress, F_Authorize = moduleEntity.F_Authorize });
                                        }
                                    }
                                    else if (item.F_ItemType == 2)
                                    {
                                        ModuleButtonEntity moduleButtonEntity = buttondata.Where(a => a.F_Id == item.F_ItemId && a.F_IsPublic == false).FirstOrDefault();
                                        if (moduleButtonEntity != null)
                                        {
                                            authdata.Add(new AuthorizeActionModel { F_Id = moduleButtonEntity.F_ModuleId, F_UrlAddress = moduleButtonEntity.F_UrlAddress, F_Authorize = moduleButtonEntity.F_Authorize });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string e = ex.Message;
                                    continue;
                                }
                            }
                            authdata.AddRange(moduledata.Where(a => a.F_IsPublic == true).Select(a => new AuthorizeActionModel { F_Id = a.F_Id, F_UrlAddress = a.F_UrlAddress, F_Authorize = a.F_Authorize }).ToList());
                            authdata.AddRange(buttondata.Where(a => a.F_IsPublic == true).Select(a => new AuthorizeActionModel { F_Id = a.F_ModuleId, F_UrlAddress = a.F_UrlAddress, F_Authorize = a.F_Authorize }).ToList());
                            cachedata.Add(roles, authdata);
                            authorizeurldata.AddRange(authdata);
                            await CacheHelper.Remove(cacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
                            await CacheHelper.Set(cacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list", cachedata);
                        }
                    }
                    else
                    {
                        authorizeurldata.AddRange(cachedata[roles]);
                    }
                }
            }
            var module = authorizeurldata.Find(a => a.F_UrlAddress == action);
            if (isAuthorize)
			{
                var temps = action.Split(',');
                module = authorizeurldata.Where(a => temps.Contains(a.F_Authorize)).FirstOrDefault();
            }
            if (module!=null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CheckReturnUrl(string userId, string url, bool isAll=false)
        {
            var user = await userApp.GetForm(userId);
            if (isAll == false &&(user == null || user.F_EnabledMark == false))
            {
                return false;
            }
            if (user.F_IsAdmin == true|| isAll == true)
            {
                if (unitofwork.GetDbClient().Queryable<ModuleEntity>().Where(a=>a.F_UrlAddress==url).Any()|| unitofwork.GetDbClient().Queryable<ModuleButtonEntity>().Where(a => a.F_UrlAddress == url).Any())
                {
                    return true;
                }
                return false;
            }
            else
            {
                var authorizeurldata = new List<AuthorizeActionModel>();
                var rolelist = user.F_RoleId.Split(',');
                var cachedata = await CacheHelper.Get<Dictionary<string, List<AuthorizeActionModel>>>(cacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
                if (cachedata == null)
                {
                    cachedata = new Dictionary<string, List<AuthorizeActionModel>>();
                }
                foreach (var roles in rolelist)
                {
                    if (!cachedata.ContainsKey(roles))
                    {
                        var moduledata = await moduleApp.GetList();
                        moduledata = moduledata.Where(a => a.F_EnabledMark == true).ToList();
                        var buttondata = await moduleButtonApp.GetList();
                        buttondata = buttondata.Where(a => a.F_EnabledMark == true).ToList();
                        var role = await roleApp.GetForm(roles);
                        if (role != null && role.F_EnabledMark == true)
                        {
                            var authdata = new List<AuthorizeActionModel>();
                            var authorizedata = await GetList(roles);
                            foreach (var item in authorizedata)
                            {
                                try
                                {
                                    if (item.F_ItemType == 1)
                                    {
                                        ModuleEntity moduleEntity = moduledata.Where(a => a.F_Id == item.F_ItemId && a.F_IsPublic == false).FirstOrDefault();
                                        if (moduleEntity != null)
                                        {
                                            authdata.Add(new AuthorizeActionModel { F_Id = moduleEntity.F_Id, F_UrlAddress = moduleEntity.F_UrlAddress, F_Authorize = moduleEntity.F_Authorize });
                                        }
                                    }
                                    else if (item.F_ItemType == 2)
                                    {
                                        ModuleButtonEntity moduleButtonEntity = buttondata.Where(a => a.F_Id == item.F_ItemId && a.F_IsPublic == false).FirstOrDefault();
                                        if (moduleButtonEntity != null)
                                        {
                                            authdata.Add(new AuthorizeActionModel { F_Id = moduleButtonEntity.F_ModuleId, F_UrlAddress = moduleButtonEntity.F_UrlAddress, F_Authorize = moduleButtonEntity.F_Authorize });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string e = ex.Message;
                                    continue;
                                }
                            }
                            authdata.AddRange(moduledata.Where(a => a.F_IsPublic == true).Select(a => new AuthorizeActionModel { F_Id = a.F_Id, F_UrlAddress = a.F_UrlAddress, F_Authorize = a.F_Authorize }).ToList());
                            authdata.AddRange(buttondata.Where(a => a.F_IsPublic == true).Select(a => new AuthorizeActionModel { F_Id = a.F_ModuleId, F_UrlAddress = a.F_UrlAddress, F_Authorize = a.F_Authorize }).ToList());
                            cachedata.Add(roles, authdata);
                            authorizeurldata.AddRange(authdata);
                            await CacheHelper.Remove(cacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
                            await CacheHelper.Set(cacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list", cachedata);
                        }
                    }
                    else
                    {
                        authorizeurldata.AddRange(cachedata[roles]);
                    }
                }
                var module = authorizeurldata.Find(a => a.F_UrlAddress == url);
                if (module != null)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<bool> RoleValidate()
        {
            var current = OperatorProvider.Provider.GetCurrent();
            if (current == null || string.IsNullOrEmpty(current.UserId))
            {
                return false;
            }
            var user = await userApp.GetForm(current.UserId);
            if (user == null || user.F_EnabledMark == false)
            {
                return false;
            }
			if (user.F_IsAdmin == true)
			{
                return true;
			}
            var authorizeurldata = new List<AuthorizeActionModel>();
            var rolelist = user.F_RoleId.Split(',');
            var cachedata = await CacheHelper.Get<Dictionary<string, List<AuthorizeActionModel>>>(cacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
            if (cachedata == null)
            {
                cachedata = new Dictionary<string, List<AuthorizeActionModel>>();
            }
            foreach (var roles in rolelist)
            {
                if (!cachedata.ContainsKey(roles))
                {
                    var moduledata = await moduleApp.GetList();
                    moduledata = moduledata.Where(a => a.F_EnabledMark == true).ToList();
                    var buttondata = await moduleButtonApp.GetList();
                    buttondata = buttondata.Where(a => a.F_EnabledMark == true).ToList();
                    var role = await roleApp.GetForm(roles);
                    if (role != null && role.F_EnabledMark == true)
                    {
                        var authdata = new List<AuthorizeActionModel>();
                        var authorizedata = await GetList(roles);
                        foreach (var item in authorizedata)
                        {
                            try
                            {
                                if (item.F_ItemType == 1)
                                {
                                    ModuleEntity moduleEntity = moduledata.Where(a => a.F_Id == item.F_ItemId && a.F_IsPublic == false).FirstOrDefault();
                                    if (moduleEntity != null)
                                    {
                                        authdata.Add(new AuthorizeActionModel { F_Id = moduleEntity.F_Id, F_UrlAddress = moduleEntity.F_UrlAddress, F_Authorize = moduleEntity.F_Authorize });
                                    }
                                }
                                else if (item.F_ItemType == 2)
                                {
                                    ModuleButtonEntity moduleButtonEntity = buttondata.Where(a => a.F_Id == item.F_ItemId && a.F_IsPublic == false).FirstOrDefault();
                                    if (moduleButtonEntity != null)
                                    {
                                        authdata.Add(new AuthorizeActionModel { F_Id = moduleButtonEntity.F_ModuleId, F_UrlAddress = moduleButtonEntity.F_UrlAddress, F_Authorize = moduleButtonEntity.F_Authorize });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string e = ex.Message;
                                continue;
                            }
                        }
                        authdata.AddRange(moduledata.Where(a => a.F_IsPublic == true).Select(a => new AuthorizeActionModel { F_Id = a.F_Id, F_UrlAddress = a.F_UrlAddress, F_Authorize = a.F_Authorize }).ToList());
                        authdata.AddRange(buttondata.Where(a => a.F_IsPublic == true).Select(a => new AuthorizeActionModel { F_Id = a.F_ModuleId, F_UrlAddress = a.F_UrlAddress, F_Authorize = a.F_Authorize }).ToList());
                        cachedata.Add(roles, authdata);
                        authorizeurldata.AddRange(authdata);
                        await CacheHelper.Remove(cacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
                        await CacheHelper.Set(cacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list", cachedata);
                    }
                }
                else
                {
                    authorizeurldata.AddRange(cachedata[roles]);
                }
            }
            if (authorizeurldata.Count>0)
            {
                return true;
            }
            return false;
        }
    }
}
