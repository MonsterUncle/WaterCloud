/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System.Threading.Tasks;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemOrganize
{
    public interface IUserLogOnRepository : IRepositoryBase<UserLogOnEntity>
    {
        Task ChangeForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity);
    }
}
