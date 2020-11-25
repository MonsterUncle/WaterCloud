/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System;
using WaterCloud.Code;
using WaterCloud.Entity.DingTalkManage;
using WaterCloud.Repository.DingTalkManage;
using System.Collections.Generic;
using System.Linq;

namespace WaterCloud.Application.DingTalkManage
{
    public class DingTalkUsersApp
    {
        private DingTalkUserRepository service = new DingTalkUserRepository();

        public List<DingTalkUserEntity> GetList(string keyword = "")
        {
            var expression = ExtLinq.True<DingTalkUserEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.UserName.Contains(keyword));
                expression = expression.Or(t => t.Mobile.Contains(keyword));
            }
            return service.IQueryable(expression).OrderBy(t => t.UserName).ToList();
        }
        public DingTalkUserEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.UserId == keyValue);
        }

        /// <summary>
        /// 删除指定更新日期前的数据
        /// </summary>
        /// <param name="keyValue"></param>
        public void DeleteForm(DateTime update)
        {
            service.Delete(t => t.UpdateTime < update);
        }

        public void SubmitForm(DingTalkUserEntity roleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                service.Update(roleEntity);
            }
            else
            {
                service.Insert(roleEntity);
            }
        }
    }
}
