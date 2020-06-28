using Chloe;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Repository.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-12 13:50
    /// 描 述：系统设置数据实现类
    /// </summary>
    public class SystemSetRepository : RepositoryBase<SystemSetEntity>,ISystemSetRepository
    {
        private IDbContext dbcontext;
        public SystemSetRepository(IDbContext context) : base(context)
        {
            dbcontext = context;
        }
        public SystemSetRepository(string ConnectStr, string providerName)
             : base(ConnectStr, providerName)
        {
            dbcontext = GetDbContext();
        }

        public async Task UpdateForm(SystemSetEntity entity)
        {
            if (entity.F_Id != Define.SYSTEM_MASTERPROJECT)
            {
                var setentity =await FindEntity(entity.F_Id);
                var newdb = DBContexHelper.Contex(setentity.F_DbString, setentity.F_DBProvider);
                using (var db=new RepositoryBase(newdb).BeginTrans())
                {
                    var user = db.IQueryable<UserEntity>(a => a.F_OrganizeId == entity.F_Id && a.F_IsAdmin == true).FirstOrDefault();
                    var userinfo = db.IQueryable<UserLogOnEntity>(a => a.F_UserId == user.F_Id).FirstOrDefault();
                    userinfo.F_UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                    userinfo.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(entity.F_AdminPassword, 32).ToLower(), userinfo.F_UserSecretkey).ToLower(), 32).ToLower();
                    await db.Update<UserEntity>(a => a.F_Id == user.F_Id, a => new UserEntity
                    {
                        F_Account = entity.F_AdminAccount
                    });
                    await db.Update<UserLogOnEntity>(a => a.F_Id == userinfo.F_Id, a => new UserLogOnEntity
                    {
                        F_UserPassword = userinfo.F_UserPassword,
                        F_UserSecretkey = userinfo.F_UserSecretkey
                    });
                    await db.Update(entity);
                    db.Commit();
                }
            }
            else
            {
                entity.F_AdminAccount = null;
                entity.F_AdminPassword = null;
            }
            await Update(entity);

        }
    }
}
