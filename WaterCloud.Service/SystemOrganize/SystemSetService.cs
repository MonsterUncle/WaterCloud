using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using Chloe;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-12 13:50
    /// 描 述：系统设置服务类
    /// </summary>
    public class SystemSetService : DataFilterService<SystemSetEntity>, IDenpendency
    {
        private IDbContext _context;
        private string cacheKeyOperator = GlobalContext.SystemConfig.ProjectPrefix + "_operator_";// +登录者token
        
        public SystemSetService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork.GetDbContext();
        }
        #region 获取数据
        public async Task<List<SystemSetEntity>> GetList(string keyword = "")
        {
            var data =  repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                data = data.Where(t => t.F_CompanyName.Contains(keyword) || t.F_ProjectName.Contains(keyword));
            }
            return data.Where(t => t.F_DeleteMark == false).OrderByDesc(t => t.F_CreatorTime).ToList();
        }

        public async Task<List<SystemSetEntity>> GetLookList(string keyword = "")
        {
            var query = repository.IQueryable().Where(u => u.F_DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(u => u.F_CompanyName.Contains(keyword) || u.F_ProjectName.Contains(keyword));
            }
            query = GetDataPrivilege("u", "", query);
            return query.OrderByDesc(t => t.F_CreatorTime).ToList();
        }

        public async Task<SystemSetEntity> GetFormByHost(string host)
        {
            var data =  repository.IQueryable();
            if (!string.IsNullOrEmpty(host))
            {
                //此处需修改
                data = data.Where(t => t.F_HostUrl.Contains(host));
            }
            if (data.Count()==0)
            {
                data =  repository.IQueryable();
            }
            return data.Where(t => t.F_DeleteMark == false).OrderBy(a=>a.F_CreatorTime).FirstOrDefault();
        }

        public async Task<List<SystemSetEntity>> GetLookList(Pagination pagination,string keyword = "")
        {
            var query = repository.IQueryable().Where(u => u.F_DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(u => u.F_CompanyName.Contains(keyword) || u.F_ProjectName.Contains(keyword));
            }
            query = GetDataPrivilege("u", "", query);
            return await repository.OrderList(query, pagination);
        }

        public async Task<SystemSetEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        #endregion

        public async Task<SystemSetEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }

        #region 提交数据
        public async Task SubmitForm(SystemSetEntity entity, string keyValue)
        {
            IUnitOfWork ibs = new UnitOfWork(_context);
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.F_DeleteMark = false;
                //此处需修改
                entity.Create();
                await repository.Insert(entity);
            }
            else
            {
                    //此处需修改
                entity.Modify(keyValue);
                if (currentuser.IsSuperAdmin == true || currentuser.UserId == null)
                {
                    var setentity = await repository.FindEntity(entity.F_Id);
                    unitwork.BeginTrans();
                    var user = unitwork.IQueryable<UserEntity>(a => a.F_OrganizeId == entity.F_Id && a.F_IsAdmin == true).FirstOrDefault();
                    var userinfo = unitwork.IQueryable<UserLogOnEntity>(a => a.F_UserId == user.F_Id).FirstOrDefault();
                    userinfo.F_UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                    userinfo.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(entity.F_AdminPassword, 32).ToLower(), userinfo.F_UserSecretkey).ToLower(), 32).ToLower();
                    await unitwork.Update<UserEntity>(a => a.F_Id == user.F_Id, a => new UserEntity
                    {
                        F_Account = entity.F_AdminAccount
                    });
                    await unitwork.Update<UserLogOnEntity>(a => a.F_Id == userinfo.F_Id, a => new UserLogOnEntity
                    {
                        F_UserPassword = userinfo.F_UserPassword,
                        F_UserSecretkey = userinfo.F_UserSecretkey
                    });
                    await unitwork.Update(entity);
                    unitwork.Commit();
                }
                else
                {
                    entity.F_AdminAccount = null;
                    entity.F_AdminPassword = null;
                }
                await ibs.Update(entity);
            }
            var set=await ibs.FindEntity<SystemSetEntity>(entity.F_Id);
            var tempkey= ibs.IQueryable<UserEntity>().Where(a => a.F_IsAdmin == true && a.F_OrganizeId == keyValue).FirstOrDefault().F_Id;
            await CacheHelper.Remove(cacheKeyOperator + "info_" + tempkey);
            if (currentuser.UserId == null)
            {
                _context.Dispose();
            }
        }

        public async Task DeleteForm(string keyValue)
        {
            await repository.Delete(t => t.F_Id == keyValue);
        }
        #endregion

    }
}
