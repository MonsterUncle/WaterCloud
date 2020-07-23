/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chloe;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemOrganize
{
    public class UserService : DataFilterService<UserEntity>, IDenpendency
    {
        private SystemSetService syssetApp;
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_userdata_";
        private string cacheKeyOperator = "watercloud_operator_";// +登录者token
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public UserService(IDbContext context) : base(context)
        {
            syssetApp = new SystemSetService(context);
        }

        public async Task<List<UserEntity>> GetLookList(Pagination pagination, string keyword)
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(u => u.F_Account.Contains(keyword) || u.F_RealName.Contains(keyword)||u.F_MobilePhone.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark == false && u.F_IsAdmin == false);
            list.Select(a => Sql.Count()).First();
            return GetFieldsFilterData(await repository.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }
        public async Task<List<UserEntity>> GetList(string keyword)
        {
            var cachedata =await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_Account.Contains(keyword) || t.F_RealName.Contains(keyword) || t.F_MobilePhone.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_IsAdmin==false && t.F_DeleteMark == false).OrderBy(t => t.F_Account).ToList();
        }

        public async Task SubmitUserForm(UserEntity userEntity)
        {
            await repository.Update(userEntity);
            await CacheHelper.Remove(cacheKey + userEntity.F_Id);
            await CacheHelper.Remove(cacheKey + "list");
        }

        public async Task<List<UserEntity>> GetUserList(string keyword)
        {
            var cachedata =await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_Account.Contains(keyword) || t.F_RealName.Contains(keyword) || t.F_MobilePhone.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_EnabledMark ==true && t.F_DeleteMark == false).OrderBy(t => t.F_Account).ToList();
        }

        public async Task<UserEntity> GetForm(string keyValue)
        {
            var cachedata =await repository.CheckCache(cacheKey, keyValue);
            string[] temp;
            if (cachedata.F_RoleId!=null)
            {
                temp = cachedata.F_RoleId.Split(',');
                cachedata.F_RoleName = string.Join(",", uniwork.IQueryable<RoleEntity>().Where(a => temp.Contains(a.F_Id)).Select(a => a.F_FullName).ToList().ToArray());
            }
            if (cachedata.F_DepartmentId != null)
            {
                temp = cachedata.F_OrganizeId.Split(',');
                cachedata.F_DepartmentName = string.Join(",", uniwork.IQueryable<OrganizeEntity>().Where(a => temp.Contains(a.F_Id)).Select(a => a.F_FullName).ToList().ToArray());
            }

            return cachedata;
        }
        public async Task<UserEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            string[] temp;
            if (cachedata.F_RoleId != null)
            {
                temp = cachedata.F_RoleId.Split(',');
                cachedata.F_RoleName = string.Join(",", uniwork.IQueryable<RoleEntity>().Where(a => temp.Contains(a.F_Id)).Select(a => a.F_FullName).ToList().ToArray());
            }
            if (cachedata.F_DepartmentId != null)
            {
                temp = cachedata.F_DepartmentId.Split(',');
                cachedata.F_DepartmentName = string.Join(",", uniwork.IQueryable<OrganizeEntity>().Where(a => temp.Contains(a.F_Id)).Select(a => a.F_FullName).ToList().ToArray());
            }
            return GetFieldsFilterData(cachedata, className.Substring(0, className.Length - 7));
        }
        public async Task DeleteForm(string keyValue)
        {
            uniwork.BeginTrans();
            await repository.Delete(t => t.F_Id == keyValue);
            await uniwork.Delete<UserLogOnEntity>(t => t.F_UserId == keyValue);
            uniwork.Commit();
            await CacheHelper.Remove(cacheKey + keyValue);
            await CacheHelper.Remove(cacheKey + "list");
        }
        public async Task SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                userEntity.Modify(keyValue);
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                userEntity.Create();
                userLogOnEntity.F_Id= Utils.GuId();
                userLogOnEntity.F_UserId = userEntity.F_Id;
                 userLogOnEntity.F_ErrorNum = 0;
                userLogOnEntity.F_UserOnLine = false;
                userLogOnEntity.F_LogOnCount = 0;
                await CacheHelper.Remove(cacheKey + "list");
            }
            uniwork.BeginTrans();
            if (!string.IsNullOrEmpty(keyValue))
            {
                await repository.Update(userEntity);
            }
            else
            {
                userLogOnEntity.F_Id = userEntity.F_Id;
                userLogOnEntity.F_UserId = userEntity.F_Id;
                userLogOnEntity.F_UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                userLogOnEntity.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userLogOnEntity.F_UserPassword, 32).ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                await repository.Insert(userEntity);
                await uniwork.Insert(userLogOnEntity);
            }
            uniwork.Commit();
        }
        public async Task UpdateForm(UserEntity userEntity)
        {
            await repository.Update(userEntity);
            await CacheHelper.Remove(cacheKey + userEntity.F_Id);
            await CacheHelper.Remove(cacheKey + "list");
        }
        /// <summary>
        /// 登录判断
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<UserEntity> CheckLogin(string username, string password,string localurl)
        {
            //根据登录公司查找公司
            if (!string.IsNullOrEmpty(localurl))
            {
                var setTemp=(await syssetApp.GetList()).Where(a=> localurl.Contains(a.F_HostUrl)).FirstOrDefault();
                if (setTemp!=null)
                {
                    repository = new RepositoryBase<UserEntity>(setTemp.F_DbString,setTemp.F_DBProvider);
                    uniwork = new RepositoryBase(setTemp.F_DbString, setTemp.F_DBProvider);
                }
            }
            UserEntity userEntity =await repository.FindEntity(t => t.F_Account == username);
            if (userEntity != null)
            {
                if (userEntity.F_EnabledMark == true)
                {
                    //缓存用户账户信息
                    var userLogOnEntity=await CacheHelper.Get<OperatorUserInfo>(cacheKeyOperator + "info_" + userEntity.F_Id);
                    if (userLogOnEntity==null)
                    {
                        userLogOnEntity = new OperatorUserInfo();
                        UserLogOnEntity entity =await uniwork.FindEntity<UserLogOnEntity>(userEntity.F_Id);
                        userLogOnEntity.F_UserPassword = entity.F_UserPassword;
                        userLogOnEntity.F_UserSecretkey = entity.F_UserSecretkey;
                        userLogOnEntity.F_AllowEndTime = entity.F_AllowEndTime;
                        userLogOnEntity.F_AllowStartTime = entity.F_AllowStartTime;
                        userLogOnEntity.F_AnswerQuestion = entity.F_AnswerQuestion;
                        userLogOnEntity.F_ChangePasswordDate = entity.F_ChangePasswordDate;
                        userLogOnEntity.F_FirstVisitTime = entity.F_FirstVisitTime;
                        userLogOnEntity.F_LastVisitTime = entity.F_LastVisitTime;
                        userLogOnEntity.F_LockEndDate = entity.F_LockEndDate;
                        userLogOnEntity.F_LockStartDate = entity.F_LockStartDate;
                        userLogOnEntity.F_LogOnCount = entity.F_LogOnCount;
                        userLogOnEntity.F_PreviousVisitTime = entity.F_PreviousVisitTime;
                        userLogOnEntity.F_Question = entity.F_Question;
                        userLogOnEntity.F_Theme = entity.F_Theme;
                        await CacheHelper.Set(cacheKeyOperator + "info_" + userEntity.F_Id, userLogOnEntity);
                    }
                    if (userLogOnEntity == null)
                    {
                        throw new Exception("账户未初始化设置密码,请联系管理员");
                    }
                    string dbPassword = Md5.md5(DESEncrypt.Encrypt(password.ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                    if (dbPassword == userLogOnEntity.F_UserPassword)
                    {
                        if (userEntity.F_Account != "admin")
                        {
                            var list = userEntity.F_RoleId.Split(',');
                            var rolelist =uniwork.IQueryable<RoleEntity>(a=>list.Contains(a.F_Id)&&a.F_EnabledMark==true).ToList();
                            if (rolelist.Count() == 0)
                            {
                                throw new Exception("账户未设置权限,请联系管理员");
                            }
                        }
                        DateTime lastVisitTime = DateTime.Now;
                        int LogOnCount = (userLogOnEntity.F_LogOnCount).ToInt() + 1;
                        if (userLogOnEntity.F_LastVisitTime != null)
                        {
                            userLogOnEntity.F_PreviousVisitTime = userLogOnEntity.F_LastVisitTime.ToDate();
                        }
                        userLogOnEntity.F_LastVisitTime = lastVisitTime;
                        userLogOnEntity.F_LogOnCount = LogOnCount;
                        userLogOnEntity.F_UserOnLine = true;
                        await CacheHelper.Remove(cacheKeyOperator + "info_" + userEntity.F_Id);
                        await CacheHelper.Set(cacheKeyOperator + "info_" + userEntity.F_Id, userLogOnEntity);
                        await OperatorProvider.Provider.ClearCurrentErrorNum();
                        return userEntity;
                    }
                    else
                    {
                        if (userEntity.F_Account != "admin")
                        {
                            int num =await OperatorProvider.Provider.AddCurrentErrorNum();
                            string erornum = (5 - num).ToString();
                            if (num == 5)
                            {
                                userEntity.F_EnabledMark = false;
                                await repository.Update(userEntity);
                                await OperatorProvider.Provider.ClearCurrentErrorNum();
                                throw new Exception("密码不正确，账户被系统锁定");
                            }
                            else
                            {
                                throw new Exception("密码不正确，请重新输入，还有" + erornum + "次机会");
                            }
                        }
                        else
                        {
                            throw new Exception("密码不正确，请重新输入");
                        }
                    }
                }
                else
                {
                    throw new Exception("账户被系统锁定,请联系管理员");
                }
            }
            else
            {
                throw new Exception("账户不存在，请重新输入");
            }
        }
    }
}
