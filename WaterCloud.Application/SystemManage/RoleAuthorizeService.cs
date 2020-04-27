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

namespace WaterCloud.Service.SystemManage
{
    public class RoleAuthorizeService:IDenpendency
    {
        private IRoleRepository roleservice = new RoleRepository();
        private IRoleAuthorizeRepository service = new RoleAuthorizeRepository();
        private ModuleService moduleApp = new ModuleService();
        private ModuleButtonService moduleButtonApp = new ModuleButtonService();
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_authorizeurldata_";// +权限

        public List<RoleAuthorizeEntity> GetList(string ObjectId)
        {
            var cachedata = service.CheckCacheList(cacheKey + "list");
            cachedata = cachedata.Where(t => t.F_ObjectId == ObjectId).ToList();
            return cachedata.ToList();
        }
        public List<ModuleEntity> GetMenuList(string roleId)
        {
            var data = new List<ModuleEntity>();
            if (OperatorProvider.Provider.GetCurrent().IsSystem)
            {
                data = moduleApp.GetList();
            }
            else
            {
                var moduledata = moduleApp.GetList();
                var role = roleservice.FindEntity(roleId);
                if (role==null||role.F_EnabledMark==false)
                {
                    return data;
                }
                var authorizedata = service.CheckCacheList(cacheKey + "list").Where(t => t.F_ObjectId == roleId && t.F_ItemType == 1).ToList();
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
                var authorizedata = service.CheckCacheList(cacheKey + "list").Where(t => t.F_ObjectId == roleId && t.F_ItemType == 2).ToList();
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
            var user = new UserService().GetForm(OperatorProvider.Provider.GetCurrent().UserId);
            if (user == null || user.F_EnabledMark == false)
            {
                return false;
            }
            var cachedata = RedisHelper.Get<List<AuthorizeActionModel>>(cacheKey + "authorize_" + roleId);
            if (cachedata == null)
            {
                var moduledata = moduleApp.GetList();
                var buttondata = moduleButtonApp.GetList();
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
                    RedisHelper.Set(cacheKey + "authorize_" + roleId,authorizeurldata);
                }
            }
            else
            {
                authorizeurldata = cachedata;
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
            var user = new UserService().GetForm(userId);
            if (user==null||user.F_EnabledMark==false)
            {
                return false;
            }
            var cachedata = RedisHelper.Get<List<AuthorizeActionModel>>(cacheKey + "authorize_" + roleId);
            if (cachedata == null)
            {
                var moduledata = moduleApp.GetList();
                var buttondata = moduleButtonApp.GetList();
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
                                if (moduleEntity!=null)
                                {
                                    authorizeurldata.Add(new AuthorizeActionModel { F_Id = moduleEntity.F_Id, F_UrlAddress = moduleEntity.F_UrlAddress });
                                }
                            }
                            else if (item.F_ItemType == 2)
                            {
                                ModuleButtonEntity moduleButtonEntity = buttondata.Find(t => t.F_Id == item.F_ItemId);
                                if (moduleButtonEntity!=null)
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
                    RedisHelper.Set(cacheKey + "authorize_" + roleId, authorizeurldata);
                }               
            }
            else
            {
                authorizeurldata = cachedata;
            }            
            if (authorizeurldata.Count>0)
            {
                return true;
            }
            return false;
        }
    }
}
