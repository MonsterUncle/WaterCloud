/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Entity.SystemManage;
using WaterCloud.Domain.IRepository.SystemManage;
using WaterCloud.Domain.ViewModel;
using WaterCloud.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WaterCloud.Application.SystemManage
{
    public class RoleAuthorizeApp
    {
        private IRoleRepository roleservice = new RoleRepository();
        private IRoleAuthorizeRepository service = new RoleAuthorizeRepository();
        private ModuleApp moduleApp = new ModuleApp();
        private ModuleButtonApp moduleButtonApp = new ModuleButtonApp();
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private ICache redisCache = CacheFactory.CaChe();
        private string cacheKey = "watercloud_authorizeurldata_";// +权限

        public List<RoleAuthorizeEntity> GetList(string ObjectId)
        {
            var cachedata = service.CheckCacheList(cacheKey + "list", CacheId.authorize);
            cachedata = cachedata.Where(t => t.F_ObjectId == ObjectId).ToList();
            return cachedata.ToList();
        }
        public List<ModuleEntity> GetMenuList(string roleId)
        {
            var data = new List<ModuleEntity>();
            if (OperatorProvider.Provider.GetCurrent().IsSystem)
            {
                data = moduleApp.GetList();
                data = data.Where(a => a.F_IsMenu == true && a.F_EnabledMark == true).ToList();
            }
            else
            {
                var moduledata = moduleApp.GetList();
                moduledata = moduledata.Where(a => a.F_IsMenu == true && a.F_EnabledMark == true).ToList();
                var role = roleservice.FindEntity(roleId);
                if (role==null||role.F_EnabledMark==false)
                {
                    return data;
                }
                var authorizedata = service.CheckCacheList(cacheKey + "list", CacheId.authorize).Where(t => t.F_ObjectId == roleId && t.F_ItemType == 1).ToList();
                foreach (var item in authorizedata)
                {
                    ModuleEntity moduleEntity = moduledata.Find(t => t.F_Id == item.F_ItemId);
                    if (moduleEntity != null)
                    {
                        data.Add(moduleEntity);
                    }
                }
            }
            return data.OrderBy(t => t.F_SortCode).ToList();
        }
        public List<ModuleButtonEntity> GetButtonList(string roleId)
        {
            var data = new List<ModuleButtonEntity>();
            if (OperatorProvider.Provider.GetCurrent().IsSystem)
            {
                data = moduleButtonApp.GetListNew();
            }
            else
            {
                var buttondata = moduleButtonApp.GetListNew();
                var role = roleservice.FindEntity(roleId);
                if (role == null || role.F_EnabledMark == false)
                {
                    return data;
                }
                var authorizedata = service.CheckCacheList(cacheKey + "list", CacheId.authorize).Where(t => t.F_ObjectId == roleId && t.F_ItemType == 2).ToList();
                foreach (var item in authorizedata)
                {
                    ModuleButtonEntity moduleButtonEntity = buttondata.Find(t => t.F_Id == item.F_ItemId);
                    if (moduleButtonEntity != null)
                    {
                        data.Add(moduleButtonEntity);
                    }
                }
            }
            return data.OrderBy(t => t.F_SortCode).ToList();
        }
        public bool ActionValidate(string roleId, string action)
        {
            var authorizeurldata = new List<AuthorizeActionModel>();
            var user = new UserApp().GetForm(OperatorProvider.Provider.GetCurrent().UserId);
            if (user == null || user.F_EnabledMark == false)
            {
                return false;
            }
            var cachedata = redisCache.Read<Dictionary<string, List<AuthorizeActionModel>>>(cacheKey + "authorize_list", CacheId.authorize);
            if (cachedata == null)
            {
                cachedata = new Dictionary<string, List<AuthorizeActionModel>>();
            }
            if (!cachedata.ContainsKey(roleId))
            {
                var moduledata = moduleApp.GetList();
                moduledata = moduledata.Where(a => a.F_EnabledMark == true).ToList();
                var buttondata = moduleButtonApp.GetList();
                buttondata = buttondata.Where(a => a.F_EnabledMark == true).ToList();
                var role = roleservice.FindEntity(roleId);
                if (role != null && role.F_EnabledMark != false)
                {
                    var authorizedata = GetList(roleId);
                    foreach (var item in authorizedata)
                    {
                        try
                        {
                            if (item.F_ItemType == 1)
                            {
                                ModuleEntity moduleEntity = moduledata.Find(t => t.F_Id == item.F_ItemId);
                                if (moduleEntity != null)
                                {
                                    authorizeurldata.Add(new AuthorizeActionModel { F_Id = moduleEntity.F_Id, F_UrlAddress = moduleEntity.F_UrlAddress });
                                }
                            }
                            else if (item.F_ItemType == 2)
                            {
                                ModuleButtonEntity moduleButtonEntity = buttondata.Find(t => t.F_Id == item.F_ItemId);
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
                    cachedata.Add(roleId, authorizeurldata);
                    redisCache.Remove(cacheKey + "authorize_list", CacheId.authorize);
                    redisCache.Write(cacheKey + "authorize_list", cachedata, CacheId.authorize);
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
        public bool RoleValidate(string userId,string roleId)
        {
            var authorizeurldata = new List<AuthorizeActionModel>();
            var user = new UserApp().GetForm(OperatorProvider.Provider.GetCurrent().UserId);
            if (user == null || user.F_EnabledMark == false)
            {
                return false;
            }
            var cachedata = redisCache.Read<Dictionary<string, List<AuthorizeActionModel>>>(cacheKey + "authorize_list", CacheId.authorize);
            if (cachedata == null)
            {
                cachedata = new Dictionary<string, List<AuthorizeActionModel>>();
            }
            if (!cachedata.ContainsKey(roleId))
            {
                var moduledata = moduleApp.GetList();
                moduledata = moduledata.Where(a => a.F_EnabledMark == true).ToList();
                var buttondata = moduleButtonApp.GetList();
                buttondata = buttondata.Where(a => a.F_EnabledMark == true).ToList();
                var role = roleservice.FindEntity(roleId);
                if (role != null && role.F_EnabledMark != false)
                {
                    var authorizedata = GetList(roleId);
                    foreach (var item in authorizedata)
                    {
                        try
                        {
                            if (item.F_ItemType == 1)
                            {
                                ModuleEntity moduleEntity = moduledata.Find(t => t.F_Id == item.F_ItemId);
                                if (moduleEntity != null)
                                {
                                    authorizeurldata.Add(new AuthorizeActionModel { F_Id = moduleEntity.F_Id, F_UrlAddress = moduleEntity.F_UrlAddress });
                                }
                            }
                            else if (item.F_ItemType == 2)
                            {
                                ModuleButtonEntity moduleButtonEntity = buttondata.Find(t => t.F_Id == item.F_ItemId);
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
                    cachedata.Add(roleId, authorizeurldata);
                    redisCache.Remove(cacheKey + "authorize_list", CacheId.authorize);
                    redisCache.Write(cacheKey + "authorize_list", cachedata, CacheId.authorize);
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
