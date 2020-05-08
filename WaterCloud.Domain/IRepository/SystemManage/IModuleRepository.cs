/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System.Collections.Generic;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemManage
{
    public interface IModuleRepository : IRepositoryBase<ModuleEntity>
    {
        List<ModuleEntity> GetListByRole(string roleid);
        void CreateModuleCode(ModuleEntity entity,List<ModuleButtonEntity> buttonlist);
    }
}
