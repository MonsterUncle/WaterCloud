/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System.Threading.Tasks;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.Repository.SystemManage
{
    public class UserLogOnRepository : RepositoryBase<UserLogOnEntity>, IUserLogOnRepository
    {
        private string ConnectStr;
        private string providerName;
        public UserLogOnRepository()
        {
        }
        public UserLogOnRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            this.ConnectStr = ConnectStr;
            this.providerName = providerName;
        }
        public async Task ChangeForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity)
        {
            using (var db =new RepositoryBase(ConnectStr, providerName).BeginTrans())
            {
                await db.Update(userEntity);
                await db.Update(userLogOnEntity);
                db.Commit();
            }
        }
    }
}
