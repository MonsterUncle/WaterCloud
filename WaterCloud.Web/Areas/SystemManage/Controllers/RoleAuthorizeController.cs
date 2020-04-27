/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Service.SystemManage;
using WaterCloud.Code;
using WaterCloud.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace WaterCloud.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class RoleAuthorizeController : ControllerBase
    {
        private RoleAuthorizeService roleAuthorizeApp = new RoleAuthorizeService();
        private ModuleService moduleApp = new ModuleService();
        private ModuleButtonService moduleButtonApp = new ModuleButtonService();
        public ActionResult GetPermissionTree(string roleId)
        {
            string roleid = OperatorProvider.Provider.GetCurrent().RoleId;
            var moduledata = new List<ModuleEntity>();
            var buttondata = new List<ModuleButtonEntity>();
            //隐藏系统菜单及字典管理
            if (roleid != null)
            {
                moduledata = moduleApp.GetListByRole(roleid);
                buttondata = moduleButtonApp.GetListByRole(roleid);
            }
            else
            {
                moduledata = moduleApp.GetList();
                buttondata = moduleButtonApp.GetList();
            }
            var authorizedata = new List<RoleAuthorizeEntity>();
            if (!string.IsNullOrEmpty(roleId))
            {
                authorizedata = roleAuthorizeApp.GetList(roleId);
            }
            var treeList = new List<TreeGridModel>();
            foreach (ModuleEntity item in moduledata)
            {
                TreeGridModel tree = new TreeGridModel();
                tree.id = item.F_Id;
                tree.title = item.F_FullName;
                tree.parentId = item.F_ParentId;
                tree.checkArr = authorizedata.Count(t => t.F_ItemId == item.F_Id) > 0?"1":"0";
                treeList.Add(tree);
            }
            foreach (ModuleButtonEntity item in buttondata)
            {
                TreeGridModel tree = new TreeGridModel();
                tree.id = item.F_Id;
                tree.title = item.F_FullName;
                tree.parentId = item.F_ParentId == "0" ? item.F_ModuleId : item.F_ParentId;
                tree.checkArr = authorizedata.Count(t => t.F_ItemId == item.F_Id) > 0 ?"1":"0";
                treeList.Add(tree);
            }
            return ResultDTree(treeList.TreeList());
        }
    }
}
