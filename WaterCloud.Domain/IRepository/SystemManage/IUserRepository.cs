/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System.Threading.Tasks;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemManage
{
    public interface IUserRepository : IRepositoryBase<UserEntity>
    {
        Task DeleteForm(string keyValue);
        Task SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue);
    }
}
