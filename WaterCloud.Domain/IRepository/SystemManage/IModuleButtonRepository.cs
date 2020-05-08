/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.DataBase;
using System.Collections.Generic;

namespace WaterCloud.Domain.SystemManage
{
    public interface IModuleButtonRepository : IRepositoryBase<ModuleButtonEntity>
    {
        void SubmitCloneButton(List<ModuleButtonEntity> entitys);
        List<ModuleButtonEntity> GetListNew(string moduleId);
        List<ModuleButtonEntity> GetListByRole(string roleid);
    }
}
