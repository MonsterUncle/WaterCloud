/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Repository.SystemManage;

namespace WaterCloud.Service.SystemManage
{
    public class UserLogOnService: IDenpendency
    {
        private IUserLogOnRepository service = new UserLogOnRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKeyOperator = "watercloud_operator_";// +登录者token

        public async Task<UserLogOnEntity> GetForm(string keyValue)
        {
            return await service.FindEntity(keyValue);
        }
        public async Task RevisePassword(string userPassword,string keyValue)
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
                await service.Insert(entity);
            }
            else
            {
                //userLogOnEntity = new UserLogOnEntity();
                //userLogOnEntity.F_Id = keyValue;
                entity.F_UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                entity.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userPassword, 32).ToLower(), entity.F_UserSecretkey).ToLower(), 32).ToLower();
                await service.Update(entity);
            }
            //缓存用户账户信息
            var userLogOnEntity =await  RedisHelper.GetAsync<OperatorUserInfo>(cacheKeyOperator + "info_" + keyValue);
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
                RedisHelper.Set(cacheKeyOperator + "info_" + keyValue, userLogOnEntity);
            }
            userLogOnEntity.F_UserPassword = entity.F_UserPassword;
            await RedisHelper.DelAsync(cacheKeyOperator + "info_" + keyValue);
            await RedisHelper.SetAsync(cacheKeyOperator + "info_" + keyValue, userLogOnEntity);
        }
    }
}
