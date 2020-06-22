/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using System;

namespace WaterCloud.Domain
{
    public class IEntity<TEntity>
    {
        public void Create(string apitoken="")
        {
            var entity = this as ICreationAudited;
            entity.F_Id = Utils.GuId();
            var LoginInfo = OperatorProvider.Provider.GetCurrent(apitoken);
            if (LoginInfo != null)
            {
                entity.F_CreatorUserId = LoginInfo.UserId;
            }
            entity.F_CreatorTime = DateTime.Now;
        }
        public void Modify(string keyValue, string apitoken = "")
        {
            var entity = this as IModificationAudited;
            entity.F_Id = keyValue;
            var LoginInfo = OperatorProvider.Provider.GetCurrent(apitoken);
            if (LoginInfo != null)
            {
                entity.F_LastModifyUserId = LoginInfo.UserId;
            }
            entity.F_LastModifyTime = DateTime.Now;
        }
        public void Remove(string apitoken = "")
        {
            var entity = this as IDeleteAudited;
            var LoginInfo = OperatorProvider.Provider.GetCurrent(apitoken);
            if (LoginInfo != null)
            {
                entity.F_DeleteUserId = LoginInfo.UserId;
            }
            entity.F_DeleteTime = DateTime.Now;
            entity.F_DeleteMark = true;
        }
    }
}
