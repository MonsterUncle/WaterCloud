/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WaterCloud.Domain.SystemOrganize
{
    public interface IRoleRepository : IRepositoryBase<RoleEntity>
    {
        Task DeleteForm(string keyValue);
        Task SubmitForm(RoleEntity roleEntity, List<RoleAuthorizeEntity> roleAuthorizeEntitys, string keyValue);
    }
}
