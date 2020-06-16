/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Service.SystemOrganize;
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WaterCloud.Service.SystemManage;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.Web.Areas.SystemOrganize.Controllers
{
    [Area("SystemOrganize")]
    public class RoleAuthorizeController : ControllerBase
    {
        private readonly RoleAuthorizeService _roleAuthorizeService;
        private readonly ModuleService _moduleService;
        private readonly ModuleButtonService _moduleButtonService;
        private readonly ModuleFieldsService _moduleFieldsService;
        public RoleAuthorizeController(RoleAuthorizeService roleAuthorizeService, ModuleButtonService moduleButtonService, ModuleService moduleService, ModuleFieldsService moduleFieldsService)
        {
            _roleAuthorizeService = roleAuthorizeService;
            _moduleService = moduleService;
            _moduleButtonService = moduleButtonService;
            _moduleFieldsService = moduleFieldsService;
        }
        [HttpGet]
        public async Task<ActionResult> GetPermissionTree(string roleId)
        {
            string roleid = OperatorProvider.Provider.GetCurrent().RoleId;
            var moduledata = new List<ModuleEntity>();
            var buttondata = new List<ModuleButtonEntity>();
            //隐藏系统菜单及字典管理
            if (roleid != null)
            {
                moduledata =await _moduleService.GetListByRole(roleid);
                buttondata =await _moduleButtonService.GetListByRole(roleid);
            }
            else
            {
                moduledata =await _moduleService.GetList();
                buttondata =await _moduleButtonService.GetList();
                moduledata = moduledata.Where(a => a.F_EnabledMark == true).ToList();
                buttondata = buttondata.Where(a => a.F_EnabledMark == true).ToList();
            }
            var authorizedata = new List<RoleAuthorizeEntity>();
            if (!string.IsNullOrEmpty(roleId))
            {
                authorizedata =await _roleAuthorizeService.GetList(roleId);
            }
            var treeList = new List<TreeGridModel>();
            foreach (ModuleEntity item in moduledata)
            {
                TreeGridModel tree = new TreeGridModel();
                tree.id = item.F_Id;
                tree.title = item.F_FullName;
                tree.parentId = item.F_ParentId;
                if (item.F_IsPublic == true)
                {
                    tree.checkArr = "1";
                    tree.disabled = true;
                }
                else
                {
                    tree.checkArr = authorizedata.Count(t => t.F_ItemId == item.F_Id) > 0 ? "1" : "0";
                }
                treeList.Add(tree);
            }
            foreach (ModuleButtonEntity item in buttondata)
            {
                TreeGridModel tree = new TreeGridModel();
                tree.id = item.F_Id;
                tree.title = item.F_FullName;
                tree.parentId = item.F_ParentId == "0" ? item.F_ModuleId : item.F_ParentId;
                if (item.F_IsPublic==true)
                {
                    tree.checkArr = "1";
                    tree.disabled = true;
                }
                else
                {
                    tree.checkArr = authorizedata.Count(t => t.F_ItemId == item.F_Id) > 0 ? "1" : "0";
                }
                treeList.Add(tree);
            }
            return ResultDTree(treeList.TreeList());
        }
        [HttpPost]
        public async Task<ActionResult> GetPermissionFieldsTree(string roleId,string moduleids)
        {
            string roleid = OperatorProvider.Provider.GetCurrent().RoleId;
            var moduledata = new List<ModuleEntity>();
            var fieldsdata = new List<ModuleFieldsEntity>();
            //隐藏系统菜单及字典管理
            if (roleid != null)
            {
                moduledata = await _moduleService.GetListByRole(roleid);
                fieldsdata = await _moduleFieldsService.GetListByRole(roleid);
            }
            else
            {
                moduledata = await _moduleService.GetList();
                fieldsdata = await _moduleFieldsService.GetList();
                moduledata = moduledata.Where(a => a.F_EnabledMark == true).ToList();
                fieldsdata = fieldsdata.Where(a => a.F_EnabledMark == true).ToList();
            }
            moduledata = moduledata.Where(a => a.F_IsFields==true||(a.F_Layers==1&&a.F_IsExpand==true)).ToList();
            if (!string.IsNullOrEmpty(moduleids))
            {
                var list=moduleids.Split(',');
                moduledata= moduledata.Where(a=> list.Contains(a.F_Id)||a.F_IsPublic==true).ToList();
            }
            else
            {
                moduledata = moduledata.Where(a =>a.F_IsPublic == true).ToList();
            }
            var authorizedata = new List<RoleAuthorizeEntity>();
            if (!string.IsNullOrEmpty(roleId))
            {
                authorizedata = await _roleAuthorizeService.GetList(roleId);
            }
            var treeList = new List<TreeGridModel>();
            foreach (ModuleEntity item in moduledata)
            {
                TreeGridModel tree = new TreeGridModel();
                tree.id = item.F_Id;
                tree.title = item.F_FullName;
                tree.parentId = item.F_ParentId;
                if (item.F_IsPublic == true)
                {
                    tree.checkArr = "1";
                    tree.disabled = true;
                }
                else
                {
                    tree.checkArr = authorizedata.Count(t => t.F_ItemId == item.F_Id) > 0 ? "1" : "0";
                }
                treeList.Add(tree);
            }
            foreach (ModuleFieldsEntity item in fieldsdata)
            {
                TreeGridModel tree = new TreeGridModel();
                tree.id = item.F_Id;
                tree.title = item.F_FullName;
                tree.parentId = item.F_ModuleId;
                if (item.F_IsPublic == true)
                {
                    tree.checkArr = "1";
                    tree.disabled = true;
                }
                else
                {
                    tree.checkArr = authorizedata.Count(t => t.F_ItemId == item.F_Id) > 0 ? "1" : "0";
                }
                treeList.Add(tree);
            }
            return ResultDTree(treeList.TreeList());
        }
    }
}
