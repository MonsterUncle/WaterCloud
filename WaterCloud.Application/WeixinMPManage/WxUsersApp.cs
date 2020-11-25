/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Entity.WeixinManage;
using WaterCloud.Repository.WeixinManage;
using System.Collections.Generic;
using System.Linq;
using System;

namespace WaterCloud.Application.WeixinManage
{
    public class WxUsersApp
    {
        private UsersRepository service = new UsersRepository();

        public List<UsersEntity> GetList(string keyword = "")
        {
            var expression = ExtLinq.True<UsersEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.Nickname.Contains(keyword));
                expression = expression.Or(t => t.Mobile.Contains(keyword));
            }
            return service.IQueryable(expression).OrderBy(t => t.CreateDate).ToList();
        }

        /// <summary>
        /// 根据日期查询未更新的用户
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<UsersEntity> GetList2(DateTime keyword)
        {
            var expression = ExtLinq.True<UsersEntity>();
            expression = expression.And(t => t.UpdateDate< keyword);

            return service.IQueryable(expression).OrderBy(t => t.CreateDate).ToList();
        }
        public UsersEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.OpenId == keyValue);
        }
        public void SubmitForm(UsersEntity roleEntity, string keyValue)
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
