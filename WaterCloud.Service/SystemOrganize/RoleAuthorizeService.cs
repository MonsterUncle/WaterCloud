/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Domain.ViewModel;
using WaterCloud.Repository.SystemOrganize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCloud.Service.SystemManage;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.Service.SystemOrganize
{
    public class RoleAuthorizeService:IDenpendency
    {
        private IRoleRepository roleservice;
        private IRoleAuthorizeRepository service;
        private ModuleService moduleApp = new ModuleService();
        private ModuleButtonService moduleButtonApp = new ModuleButtonService();
        private ModuleFieldsService moduleFieldsApp = new ModuleFieldsService();
        
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_authorizeurldata_";// +权限
        public RoleAuthorizeService()
        {
            var currentuser = OperatorProvider.Provider.GetCurrent();
            service = currentuser != null ? new RoleAuthorizeRepository(currentuser.DbString, currentuser.DBProvider) : new RoleAuthorizeRepository();
            roleservice = currentuser != null ? new RoleRepository(currentuser.DbString, currentuser.DBProvider) : new RoleRepository();
        }

        public async Task<List<RoleAuthorizeEntity>> GetList(string ObjectId)
        {
            var cachedata =await service.CheckCacheList(cacheKey + "list");
            cachedata = cachedata.Where(t => t.F_ObjectId == ObjectId).ToList();
            return cachedata.ToList();
        }
        public async Task<List<ModuleEntity>> GetMenuList(string roleId)
        {
            var data = new List<ModuleEntity>();
            if (OperatorProvider.Provider.GetCurrent().IsSystem)
            {
                data =await moduleApp.GetList();
                data = data.Where(a => a.F_IsMenu == true&&a.F_EnabledMark==true).ToList();
            }
            else
            {
                var moduledata =await moduleApp.GetList();
                moduledata = moduledata.Where(a => a.F_IsMenu == true && a.F_EnabledMark == true).ToList();
                var role =await roleservice.FindEntity(roleId);
                if (role==null||role.F_EnabledMark==false)
                {
                    return data;
                }
                var authorizedata =(await service.CheckCacheList(cacheKey + "list")).Where(t => t.F_ObjectId == roleId && t.F_ItemType == 1).ToList();
                foreach (var item in authorizedata)
                {
                    ModuleEntity moduleEntity = moduledata.Find(t => t.F_Id == item.F_ItemId && t.F_IsPublic==false);
                    if (moduleEntity != null)
                    {
                        data.Add(moduleEntity);
                    }
                }
                data.AddRange(moduledata.Where(a => a.F_IsPublic == true));
            }
            return data.OrderBy(t => t.F_SortCode).ToList();
        }
        public async Task<List<ModuleButtonEntity>> GetButtonList(string roleId)
        {
            var data = new List<ModuleButtonEntity>();
            if (OperatorProvider.Provider.GetCurrent().IsSystem)
            {
                data =await moduleButtonApp.GetListNew();
            }
            else
            {
                var buttondata =await moduleButtonApp.GetListNew();
                var role =await roleservice.FindEntity(roleId);
                if (role == null || role.F_EnabledMark == false)
                {
                    return data;
                }
                var authorizedata =(await service.CheckCacheList(cacheKey + "list")).Where(t => t.F_ObjectId == roleId && t.F_ItemType == 2).ToList();
                foreach (var item in authorizedata)
                {
                    ModuleButtonEntity moduleButtonEntity = buttondata.Find(t => t.F_Id == item.F_ItemId && t.F_IsPublic == false);
                    if (moduleButtonEntity != null)
                    {
                        data.Add(moduleButtonEntity);
                    }
                }
                data.AddRange(buttondata.Where(a => a.F_IsPublic == true));
            }
            return data.OrderBy(t => t.F_SortCode).ToList();
        }
        public async Task<List<ModuleFieldsEntity>> GetFieldsList(string roleId)
        {
            var data = new List<ModuleFieldsEntity>();
            if (OperatorProvider.Provider.GetCurrent().IsSystem)
            {
                data = await moduleFieldsApp.GetListNew();
            }
            else
            {
                var fieldsdata = await moduleFieldsApp.GetListNew();
                var role = await roleservice.FindEntity(roleId);
                if (role == null || role.F_EnabledMark == false)
                {
                    return data;
                }
                var authorizedata = (await service.CheckCacheList(cacheKey + "list")).Where(t => t.F_ObjectId == roleId && t.F_ItemType == 3).ToList();
                foreach (var item in authorizedata)
                {
                    ModuleFieldsEntity moduleFieldsEntity = fieldsdata.Find(t => t.F_Id == item.F_ItemId&&t.F_IsPublic==false);
                    if (moduleFieldsEntity != null)
                    {
                        data.Add(moduleFieldsEntity);
                    }
                }
                data.AddRange(fieldsdata.Where(a => a.F_IsPublic == true));
            }
            return data.OrderByDescending(t => t.F_CreatorTime).ToList();
        }
        public async Task<bool> ActionValidate(string roleId, string action)
        {
            var authorizeurldata = new List<AuthorizeActionModel>();
            var user =await new UserService().GetForm(OperatorProvider.Provider.GetCurrent().UserId);
            if (user == null || user.F_EnabledMark == false)
            {
                return false;
            }
            var cachedata =await CacheHelper.Get<Dictionary<string,List<AuthorizeActionModel>>>(cacheKey + "authorize_list");
            if (cachedata == null)
            {
                cachedata = new Dictionary<string, List<AuthorizeActionModel>>();
            }
            if (!cachedata.ContainsKey(roleId))
            {
                var moduledata =await moduleApp.GetList();
                moduledata = moduledata.Where(a => a.F_EnabledMark == true).ToList();
                var buttondata =await moduleButtonApp.GetList();
                buttondata = buttondata.Where(a => a.F_EnabledMark == true).ToList();
                var role =await roleservice.FindEntity(roleId);
                if (role != null && role.F_EnabledMark != false)
                {
                    var authorizedata =await GetList(roleId);
                    foreach (var item in authorizedata)
                    {
                        try
                        {
                            if (item.F_ItemType == 1)
                            {
                                ModuleEntity moduleEntity = moduledata.Find(t => t.F_Id == item.F_ItemId && t.F_IsPublic == false);
                                if (moduleEntity != null)
                                {
                                    authorizeurldata.Add(new AuthorizeActionModel { F_Id = moduleEntity.F_Id, F_UrlAddress = moduleEntity.F_UrlAddress });
                                }
                            }
                            else if (item.F_ItemType == 2)
                            {
                                ModuleButtonEntity moduleButtonEntity = buttondata.Find(t => t.F_Id == item.F_ItemId && t.F_IsPublic == false);
                                if (moduleButtonEntity != null)
                                {
                                    authorizeurldata.Add(new AuthorizeActionModel { F_Id = moduleButtonEntity.F_ModuleId, F_UrlAddress = moduleButtonEntity.F_UrlAddress });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string e = ex.Message;
                            continue;
                        }
                    }
                    authorizeurldata.AddRange(moduledata.Where(a => a.F_IsPublic == true).Select(a => new AuthorizeActionModel { F_Id = a.F_Id, F_UrlAddress = a.F_UrlAddress }).ToList());
                    authorizeurldata.AddRange(buttondata.Where(a => a.F_IsPublic == true).Select(a => new AuthorizeActionModel { F_Id = a.F_ModuleId, F_UrlAddress = a.F_UrlAddress }).ToList());
                    cachedata.Add(roleId, authorizeurldata);
                    await CacheHelper.Remove(cacheKey + "authorize_list");
                    await CacheHelper.Set(cacheKey + "authorize_list", cachedata);
                }
            }
            else
            {
                authorizeurldata = cachedata[roleId];
            }
            var module = authorizeurldata.Find(t => t.F_UrlAddress==action);
            if (module!=null)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> RoleValidate(string userId,string roleId)
        {
            var authorizeurldata = new List<AuthorizeActionModel>();
            var user = await new UserService().GetForm(userId);
            if (user == null || user.F_EnabledMark == false)
            {
                return false;
            }
            var cachedata = await CacheHelper.Get<Dictionary<string, List<AuthorizeActionModel>>>(cacheKey + "authorize_list");
            if (cachedata == null)
            {
                cachedata = new Dictionary<string, List<AuthorizeActionModel>>();
            }
            if (!cachedata.ContainsKey(roleId))
            {
                var moduledata = await moduleApp.GetList();
                moduledata = moduledata.Where(a => a.F_EnabledMark == true).ToList();
                var buttondata = await moduleButtonApp.GetList();
                buttondata = buttondata.Where(a => a.F_EnabledMark == true).ToList();
                var role = await roleservice.FindEntity(roleId);
                if (role != null && role.F_EnabledMark != false)
                {
                    var authorizedata = await GetList(roleId);
                    foreach (var item in authorizedata)
                    {
                        try
                        {
                            if (item.F_ItemType == 1)
                            {
                                ModuleEntity moduleEntity = moduledata.Find(t => t.F_Id == item.F_ItemId && t.F_IsPublic == false);
                                if (moduleEntity != null)
                                {
                                    authorizeurldata.Add(new AuthorizeActionModel { F_Id = moduleEntity.F_Id, F_UrlAddress = moduleEntity.F_UrlAddress });
                                }
                            }
                            else if (item.F_ItemType == 2)
                            {
                                ModuleButtonEntity moduleButtonEntity = buttondata.Find(t => t.F_Id == item.F_ItemId && t.F_IsPublic == false);
                                if (moduleButtonEntity != null)
                                {
                                    authorizeurldata.Add(new AuthorizeActionModel { F_Id = moduleButtonEntity.F_ModuleId, F_UrlAddress = moduleButtonEntity.F_UrlAddress });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string e = ex.Message;
                            continue;
                        }
                    }
                    authorizeurldata.AddRange(moduledata.Where(a => a.F_IsPublic == true).Select(a => new AuthorizeActionModel { F_Id = a.F_Id, F_UrlAddress = a.F_UrlAddress }).ToList());
                    authorizeurldata.AddRange(buttondata.Where(a => a.F_IsPublic == true).Select(a => new AuthorizeActionModel { F_Id = a.F_ModuleId, F_UrlAddress = a.F_UrlAddress }).ToList());
                    cachedata.Add(roleId, authorizeurldata);
                    await CacheHelper.Remove(cacheKey + "authorize_list");
                    await CacheHelper.Set(cacheKey + "authorize_list", cachedata);
                }
            }
            else
            {
                authorizeurldata = cachedata[roleId];
            }
            if (authorizeurldata.Count>0)
            {
                return true;
            }
            return false;
        }
    }
}
