/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemManage
{
    public interface IModuleRepository : IRepositoryBase<ModuleEntity>
    {
        Task<List<ModuleEntity>> GetListByRole(string roleid);
        Task CreateModuleCode(ModuleEntity entity,List<ModuleButtonEntity> buttonlist, List<ModuleFieldsEntity> moduleFieldsList);
        Task DeleteForm(string keyValue);
        Task<List<ModuleEntity>> GetBesidesList();
    }
}
