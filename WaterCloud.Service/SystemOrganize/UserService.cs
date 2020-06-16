/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Repository.SystemOrganize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaterCloud.Service.SystemOrganize
{
    public class UserService : DataFilterService<UserEntity>, IDenpendency
    {
        private IRoleRepository roleservice;
        private IUserRepository service;
        private UserLogOnService userLogOnApp = new UserLogOnService();

        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_userdata_";
        private string cacheKeyOperator = "watercloud_operator_";// +登录者token
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public UserService()
        {
            var currentuser = OperatorProvider.Provider.GetCurrent();

            service = currentuser!=null? new UserRepository(currentuser.DbString, currentuser.DBProvider) : new UserRepository();
            roleservice = currentuser != null ? new RoleRepository(currentuser.DbString, currentuser.DBProvider) : new RoleRepository();
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
            return GetFieldsFilterData(await service.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }
        public async Task<List<UserEntity>> GetList(string keyword)
        {
            var cachedata =await service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_Account.Contains(keyword) || t.F_RealName.Contains(keyword) || t.F_MobilePhone.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_IsAdmin==false && t.F_DeleteMark == false).OrderBy(t => t.F_Account).ToList();
        }

        public async Task SubmitUserForm(UserEntity userEntity)
        {
            await service.Update(userEntity);
            await CacheHelper.Remove(cacheKey + userEntity.F_Id);
            await CacheHelper.Remove(cacheKey + "list");
        }

        public async Task<List<UserEntity>> GetUserList(string keyword)
        {
            var cachedata =await service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_Account.Contains(keyword) || t.F_RealName.Contains(keyword) || t.F_MobilePhone.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_EnabledMark ==true && t.F_DeleteMark == false).OrderBy(t => t.F_Account).ToList();
        }

        public async Task<UserEntity> GetForm(string keyValue)
        {
            var cachedata =await service.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        public async Task<UserEntity> GetLookForm(string keyValue)
        {
            var cachedata = await service.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata, className.Substring(0, className.Length - 7));
        }
        public async Task DeleteForm(string keyValue)
        {
            await service.DeleteForm(keyValue);
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
            await service.SubmitForm(userEntity, userLogOnEntity, keyValue);
        }
        public async Task UpdateForm(UserEntity userEntity)
        {
            await service.Update(userEntity);
            await CacheHelper.Remove(cacheKey + userEntity.F_Id);
            await CacheHelper.Remove(cacheKey + "list");
        }
        /// <summary>
        /// 登录判断
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<UserEntity> CheckLogin(string username, string password,string localurl, string apitoken="")
        {
            //根据登录公司查找公司
            if (string.IsNullOrEmpty(localurl))
            {
                service=new UserRepository();
            }
            else
            {
                var setTemp=(await new SystemSetService().GetList()).Where(a=> localurl.Contains(a.F_HostUrl)).FirstOrDefault();
                if (setTemp!=null)
                {
                    service = new UserRepository(setTemp.F_DbString,setTemp.F_DBProvider);
                }
                else
                {
                    service = new UserRepository();
                }
            }
            UserEntity userEntity =await service.FindEntity(t => t.F_Account == username);
            if (userEntity != null)
            {
                if (userEntity.F_EnabledMark == true)
                {
                    //缓存用户账户信息
                    var userLogOnEntity=await CacheHelper.Get<OperatorUserInfo>(cacheKeyOperator + "info_" + userEntity.F_Id);
                    if (userLogOnEntity==null)
                    {
                        userLogOnEntity = new OperatorUserInfo();
                        UserLogOnEntity entity =await userLogOnApp.GetForm(userEntity.F_Id);
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
                            var role =await roleservice.FindEntity(userEntity.F_RoleId);
                            if (role == null || role.F_EnabledMark == false)
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
                        await OperatorProvider.Provider.ClearCurrentErrorNum(apitoken);
                        return userEntity;
                    }
                    else
                    {
                        if (userEntity.F_Account != "admin")
                        {
                            int num =await OperatorProvider.Provider.AddCurrentErrorNum(apitoken);
                            string erornum = (5 - num).ToString();
                            if (num == 5)
                            {
                                userEntity.F_EnabledMark = false;
                                await service.Update(userEntity);
                                await OperatorProvider.Provider.ClearCurrentErrorNum(apitoken);
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
