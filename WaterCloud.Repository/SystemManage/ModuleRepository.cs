/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;
using System.Collections.Generic;
using System;

namespace WaterCloud.Repository.SystemManage
{
    public class ModuleRepository : RepositoryBase<ModuleEntity>, IModuleRepository
    {
        private string ConnectStr;
        private string providerName;
        public ModuleRepository()
        {
        }
        public ModuleRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            this.ConnectStr = ConnectStr;
            this.providerName = providerName;
        }

        public void CreateModuleCode(ModuleEntity entity, List<ModuleButtonEntity> buttonlist)
        {
            using (var db = new RepositoryBase(ConnectStr, providerName).BeginTrans())
            {
                db.Insert(entity);
                db.Insert(buttonlist);
                db.Commit();
            }
        }

        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase(ConnectStr, providerName).BeginTrans())
            {
                db.Delete<ModuleEntity>(a=>a.F_Id== keyValue);
                db.Delete<ModuleButtonEntity>(a=>a.F_ModuleId== keyValue);
                db.Commit();
            }
        }

        public List<ModuleEntity> GetListByRole(string roleid)
        {
            using (var db = new RepositoryBase(ConnectStr, providerName))
            {
                var moduleList = db.IQueryable<RoleAuthorizeEntity>(a => a.F_ObjectId == roleid && a.F_ItemType == 1).Select(a => a.F_ItemId).ToList();
                var query = db.IQueryable<ModuleEntity>().Where(a => moduleList.Contains(a.F_Id) && a.F_EnabledMark == true);
                var result = query.OrderBy(a => a.F_SortCode).ToList();
                return result;
            }
        }
    }
}
