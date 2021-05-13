using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.DataBase;
using SqlSugar;

namespace WaterCloud.Service.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-12 13:50
    /// 描 述：系统设置服务类
    /// </summary>
    public class SystemSetService : DataFilterService<SystemSetEntity>, IDenpendency
    {
        private string cacheKeyOperator = "watercloud_operator_";// +登录者token
        private string cacheKeyUser = "watercloud_userdata_";
        
        public SystemSetService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #region 获取数据
        public async Task<List<SystemSetEntity>> GetList(string keyword = "")
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(t => t.F_CompanyName.Contains(keyword) || t.F_ProjectName.Contains(keyword));
            }
            return await query.Where(t => t.F_DeleteMark == false).OrderBy(a => a.F_Id, OrderByType.Desc).ToListAsync();
        }

        public async Task<List<SystemSetEntity>> GetLookList(string keyword = "")
        {
            var query = repository.IQueryable().Where(u => u.F_DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u => u.F_CompanyName.Contains(keyword) || u.F_ProjectName.Contains(keyword));
            }
            query = GetDataPrivilege("u", "", query);
            return await query.OrderBy(a => a.F_Id, OrderByType.Desc).ToListAsync();
        }

        public async Task<SystemSetEntity> GetFormByHost(string host)
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(host))
            {
                //此处需修改
                query = query.Where(t => t.F_HostUrl.Contains(host));
            }
            else
            {
                query = query.Where(t => t.F_Id==GlobalContext.SystemConfig.SysemMasterProject);
            }
            if (query.Clone().Count()==0)
            {
                query = repository.IQueryable();
                query = query.Where(t => t.F_Id == GlobalContext.SystemConfig.SysemMasterProject);
            }
            return await query.Where(t => t.F_DeleteMark == false).FirstAsync();
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
                if (currentuser.UserId != GlobalContext.SystemConfig.SysemUserId || currentuser.UserId == null)
                {
                    var setentity = await repository.FindEntity(entity.F_Id);
                    unitofwork.BeginTrans();
                    var user = repository.Db.Queryable<UserEntity>().Where(a => a.F_OrganizeId == entity.F_Id && a.F_IsAdmin == true).First();
                    var userinfo = repository.Db.Queryable<UserLogOnEntity>().Where(a => a.F_UserId == user.F_Id).First();
                    userinfo.F_UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                    userinfo.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(entity.F_AdminPassword, 32).ToLower(), userinfo.F_UserSecretkey).ToLower(), 32).ToLower();
                    await repository.Db.Updateable<UserEntity>(a => new UserEntity
                    {
                        F_Account = entity.F_AdminAccount
                    }).Where(a => a.F_Id == user.F_Id).ExecuteCommandAsync();
                    await CacheHelper.Remove(cacheKeyUser + user.F_Id);
                    await repository.Db.Updateable<UserLogOnEntity>(a => new UserLogOnEntity
                    {
                        F_UserPassword = userinfo.F_UserPassword,
                        F_UserSecretkey = userinfo.F_UserSecretkey
                    }).Where(a => a.F_Id == userinfo.F_Id).ExecuteCommandAsync();
                    await repository.Db.Updateable(entity).IgnoreColumns(ignoreAllNullColumns:true).ExecuteCommandAsync();
                    unitofwork.Commit();
                }
                else
                {
                    entity.F_AdminAccount = null;
                    entity.F_AdminPassword = null;
                }
                await unitofwork.GetDbClient().Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            }
            var set=await unitofwork.GetDbClient().Queryable<SystemSetEntity>().InSingleAsync(entity.F_Id);
            unitofwork.GetDbClient().ChangeDatabase(set.F_DBNumber);
            var tempkey= unitofwork.GetDbClient().Queryable<UserEntity>().Where(a => a.F_IsAdmin == true && a.F_OrganizeId == keyValue).First().F_Id;
            await CacheHelper.Remove(cacheKeyOperator + "info_" + tempkey);
        }

        public async Task DeleteForm(string keyValue)
        {
            await repository.Delete(t => t.F_Id == keyValue);
        }
        #endregion

    }
}
