/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.DataBase;
using WaterCloud.Entity.SystemManage;
using WaterCloud.Domain.IRepository.SystemManage;
using WaterCloud.Repository.SystemManage;

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
        public void ChangeForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity)
        {
            using (var db =new RepositoryBase(ConnectStr, providerName).BeginTrans())
            {
                db.Update(userEntity);
                db.Update(userLogOnEntity);
                db.Commit();
            }
        }
    }
}
