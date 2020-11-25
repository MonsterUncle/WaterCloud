/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Entity.SystemManage;
using WaterCloud.Domain.IRepository.SystemManage;
using WaterCloud.Repository.SystemManage;
using System;

namespace WaterCloud.Application.SystemManage
{
    public class UserLogOnApp
    {
        private IUserLogOnRepository service = new UserLogOnRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private ICache redisCache = CacheFactory.CaChe();
        private string cacheKeyOperator = "watercloud_operator_";// +登录者token

        public UserLogOnEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void RevisePassword(string userPassword,string keyValue)
        {
            UserLogOnEntity entity = new UserLogOnEntity();
            entity = service.IQueryable(a => a.F_UserId == keyValue).FirstOrDefault() ;
            if (entity == null)
            {
                entity = new UserLogOnEntity();
                entity.F_Id = keyValue;
                entity.F_UserId = keyValue;
                entity.F_LogOnCount = 0;
                entity.F_UserOnLine = false;
                entity.F_UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                entity.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userPassword, 32).ToLower(), entity.F_UserSecretkey).ToLower(), 32).ToLower();
                service.Insert(entity);
            }
            else
            {
                //userLogOnEntity = new UserLogOnEntity();
                //userLogOnEntity.F_Id = keyValue;
                entity.F_UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                entity.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userPassword, 32).ToLower(), entity.F_UserSecretkey).ToLower(), 32).ToLower();
                service.Update(entity);
            }
            //缓存用户账户信息
            var userLogOnEntity = redisCache.Read<OperatorUserInfo>(cacheKeyOperator + "info_" + keyValue, CacheId.loginInfo);
            if (userLogOnEntity == null)
            {
                userLogOnEntity = new OperatorUserInfo();
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
            }
            userLogOnEntity.F_UserPassword = entity.F_UserPassword;
            redisCache.Remove(cacheKeyOperator + "info_" + keyValue, CacheId.loginInfo);
            redisCache.Write<OperatorUserInfo>(cacheKeyOperator + "info_" + keyValue, userLogOnEntity, CacheId.loginInfo);
        }

        public void ReviseSelfPassword(string userPassword, string keyValue)
        {
            UserLogOnEntity entity = new UserLogOnEntity();
            entity = service.IQueryable(a => a.F_UserId == keyValue).FirstOrDefault();
            entity.F_UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
            entity.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userPassword, 32).ToLower(), entity.F_UserSecretkey).ToLower(), 32).ToLower();
            service.Update(entity);
            var userLogOnEntity = redisCache.Read<OperatorUserInfo>(cacheKeyOperator + "info_" + keyValue, CacheId.loginInfo);
            userLogOnEntity.F_UserPassword = entity.F_UserPassword;
            userLogOnEntity.F_UserSecretkey = entity.F_UserSecretkey;
            redisCache.Write<OperatorUserInfo>(cacheKeyOperator + "info_" + keyValue, userLogOnEntity, CacheId.loginInfo);
        }
    }
}
