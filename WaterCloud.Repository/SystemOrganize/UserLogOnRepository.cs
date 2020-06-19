/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using Chloe;
using System.Threading.Tasks;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Repository.SystemOrganize
{
    public class UserLogOnRepository : RepositoryBase<UserLogOnEntity>, IUserLogOnRepository
    {
        private IDbContext dbcontext;
        public UserLogOnRepository(IDbContext context) : base(context)
        {
            dbcontext = context;
        }
        public UserLogOnRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            dbcontext = GetDbContext();
        }
        public async Task ChangeForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity)
        {
            using (var db =new RepositoryBase(dbcontext).BeginTrans())
            {
                await db.Update(userEntity);
                await db.Update(userLogOnEntity);
                db.Commit();
            }
        }
    }
}
