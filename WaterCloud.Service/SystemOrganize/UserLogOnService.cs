/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using SqlSugar;
using System;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Service.SystemOrganize
{
    public class UserLogOnService : DataFilterService<UserLogOnEntity>, IDenpendency
    {
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private string cacheKeyOperator = GlobalContext.SystemConfig.ProjectPrefix + "_operator_";// +登录者token
        public UserLogOnService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<UserLogOnEntity> GetForm(string keyValue)
        {
            return await repository.FindEntity(keyValue);
        }
        public async Task RevisePassword(string userPassword,string keyValue)
        {
            UserLogOnEntity entity = new UserLogOnEntity();
            entity = repository.IQueryable().InSingle(keyValue) ;
            if (entity == null)
            {
                entity = new UserLogOnEntity();
                entity.Id = keyValue;
                entity.UserId = keyValue;
                entity.LogOnCount = 0;
                entity.UserOnLine = false;
                entity.UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                entity.UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userPassword, 32).ToLower(), entity.UserSecretkey).ToLower(), 32).ToLower();
                await repository.Insert(entity);
            }
            else
            {
                //userLogOnEntity = new UserLogOnEntity();
                //userLogOnEntity.Id = keyValue;
                entity.UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                entity.UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userPassword, 32).ToLower(), entity.UserSecretkey).ToLower(), 32).ToLower();
                await repository.Update(entity);
            }
            //缓存用户账户信息
            var userLogOnEntity =await CacheHelper.GetAsync<OperatorUserInfo>(cacheKeyOperator + "info_" + keyValue);
            if (userLogOnEntity == null)
            {
                userLogOnEntity = new OperatorUserInfo();
                userLogOnEntity.UserPassword = entity.UserPassword;
                userLogOnEntity.UserSecretkey = entity.UserSecretkey;
                userLogOnEntity.AllowEndTime = entity.AllowEndTime;
                userLogOnEntity.AllowStartTime = entity.AllowStartTime;
                userLogOnEntity.AnswerQuestion = entity.AnswerQuestion;
                userLogOnEntity.ChangePasswordDate = entity.ChangePasswordDate;
                userLogOnEntity.FirstVisitTime = entity.FirstVisitTime;
                userLogOnEntity.LastVisitTime = entity.LastVisitTime;
                userLogOnEntity.LockEndDate = entity.LockEndDate;
                userLogOnEntity.LockStartDate = entity.LockStartDate;
                userLogOnEntity.LogOnCount = entity.LogOnCount;
                userLogOnEntity.PreviousVisitTime = entity.PreviousVisitTime;
                userLogOnEntity.Question = entity.Question;
                userLogOnEntity.Theme = entity.Theme;
            }
            userLogOnEntity.UserPassword = entity.UserPassword;
            userLogOnEntity.UserSecretkey = entity.UserSecretkey;
            await CacheHelper.RemoveAsync(cacheKeyOperator + "info_" + keyValue);
            await CacheHelper.SetAsync(cacheKeyOperator + "info_" + keyValue, userLogOnEntity);
        }

        public async Task ReviseSelfPassword(string userPassword, string keyValue)
        {
            UserLogOnEntity entity = new UserLogOnEntity();
            entity = repository.IQueryable().InSingle(keyValue);
            entity.UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
            entity.UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userPassword, 32).ToLower(), entity.UserSecretkey).ToLower(), 32).ToLower();
            await repository.Update(entity);
            var userLogOnEntity = await CacheHelper.GetAsync<OperatorUserInfo>(cacheKeyOperator + "info_" + keyValue);
            userLogOnEntity.UserPassword = entity.UserPassword;
            userLogOnEntity.UserSecretkey = entity.UserSecretkey;
            await CacheHelper.SetAsync(cacheKeyOperator + "info_" + keyValue, userLogOnEntity);
        }
    }
}
