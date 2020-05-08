/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WaterCloud.Service.SystemManage
{
    public class UserService: IDenpendency
    {
        private IRoleRepository roleservice = new RoleRepository();
        private IUserRepository service = new UserRepository();
        private UserLogOnService userLogOnApp = new UserLogOnService();

        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_userdata_";
        private string cacheKeyOperator = "watercloud_operator_";// +登录者token

        public List<UserEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<UserEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_Account.Contains(keyword));
                expression = expression.Or(t => t.F_RealName.Contains(keyword));
                expression = expression.Or(t => t.F_MobilePhone.Contains(keyword));
            }
            expression = expression.And(t => t.F_Account != "admin");
            return service.FindList(expression, pagination);
        }
        public List<UserEntity> GetList(string keyword)
        {
            var cachedata = service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_Account.Contains(keyword) || t.F_RealName.Contains(keyword) || t.F_MobilePhone.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_Account != "admin").OrderBy(t => t.F_Account).ToList();
        }

        public void SubmitUserForm(UserEntity userEntity)
        {
            service.Update(userEntity);
            RedisHelper.Del(cacheKey + userEntity.F_Id);
            RedisHelper.Del(cacheKey + "list");
        }

        public List<UserEntity> GetUserList(string keyword)
        {
            var cachedata = service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_Account.Contains(keyword) || t.F_RealName.Contains(keyword) || t.F_MobilePhone.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_EnabledMark ==true).OrderBy(t => t.F_Account).ToList();
        }

        public UserEntity GetForm(string keyValue)
        {
            var cachedata = service.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
            RedisHelper.Del(cacheKey + keyValue);
            RedisHelper.Del(cacheKey + "list");
        }
        public void SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                userEntity.Modify(keyValue);
                RedisHelper.Del(cacheKey + keyValue);
                RedisHelper.Del(cacheKey + "list");
            }
            else
            {
                userEntity.Create();
                userLogOnEntity.F_Id= Utils.GuId();
                userLogOnEntity.F_UserId = userEntity.F_Id;
               userLogOnEntity.F_ErrorNum = 0;
                userLogOnEntity.F_UserOnLine = false;
                userLogOnEntity.F_LogOnCount = 0;
                RedisHelper.Del(cacheKey + "list");
            }
            service.SubmitForm(userEntity, userLogOnEntity, keyValue);
        }
        public void UpdateForm(UserEntity userEntity)
        {
            service.Update(userEntity);
            RedisHelper.Del(cacheKey + userEntity.F_Id);
            RedisHelper.Del(cacheKey + "list");
        }
        /// <summary>
        /// 登录判断
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserEntity CheckLogin(string username, string password)
        {
            UserEntity userEntity = service.FindEntity(t => t.F_Account == username);
            if (userEntity != null)
            {
                if (userEntity.F_EnabledMark == true)
                {
                    //缓存用户账户信息
                    var userLogOnEntity= RedisHelper.Get<OperatorUserInfo>(cacheKeyOperator + "info_" + userEntity.F_Id);
                    if (userLogOnEntity==null)
                    {
                        userLogOnEntity = new OperatorUserInfo();
                        UserLogOnEntity entity = userLogOnApp.GetForm(userEntity.F_Id);
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
                        RedisHelper.Set(cacheKeyOperator + "info_" + userEntity.F_Id, userLogOnEntity);
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
                            var role = roleservice.FindEntity(userEntity.F_RoleId);
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
                        RedisHelper.Del(cacheKeyOperator + "info_" + userEntity.F_Id);
                        RedisHelper.Set(cacheKeyOperator + "info_" + userEntity.F_Id, userLogOnEntity);
                        OperatorProvider.Provider.ClearCurrentErrorNum();
                        return userEntity;
                    }
                    else
                    {
                        if (userEntity.F_Account != "admin")
                        {
                            int num = OperatorProvider.Provider.AddCurrentErrorNum();
                            string erornum = (5 - num).ToString();
                            if (num == 5)
                            {
                                userEntity.F_EnabledMark = false;
                                service.Update(userEntity);
                                OperatorProvider.Provider.ClearCurrentErrorNum();
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

        /// <summary>
        /// 通过钉钉UserId实现用户登录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserEntity CheckLoginByDingTalk(string userId)
        {
            UserEntity userEntity = service.FindEntity(t => t.F_DingTalkUserId == userId);
            if (userEntity != null)
            {
                if (userEntity.F_EnabledMark == true)
                {
                    return userEntity;
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

        /// <summary>
        /// 根据用户Id或者角色Id查询用户
        /// </summary>
        /// <param name="list"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<UserEntity> GetUserList(List<string> list,string type="Users")
        {
            var expression = ExtLinq.True<UserEntity>();
            expression = expression.And(t => t.F_EnabledMark == true);
            List<UserEntity> data = service.IQueryable(expression).ToList();
            List<UserEntity> users = new List<UserEntity>();
            try
            {
                if (data != null && list != null)
                {
                    if (type == "Roles")
                    {
                        foreach (string f_id in list)
                        {
                            users.AddRange(data.FindAll(delegate (UserEntity p) { return p.F_RoleId == f_id; }));
                        }
                    }
                    else
                    {
                        foreach (string f_id in list)
                        {
                            users.Add(data.Find(delegate (UserEntity p) { return p.F_Id == f_id; }));
                        }
                    }
                    return users;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
