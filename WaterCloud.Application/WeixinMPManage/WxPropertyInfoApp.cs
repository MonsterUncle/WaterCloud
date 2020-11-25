/*******************************************************************************
 * Copyright © 2020 WaterCloud 版权所有
 * Author: WaterCloud
 * Description: WaterCloud开发平台
 * Website：
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using WaterCloud.Code;
using WaterCloud.Entity.WeixinManage;
using WaterCloud.Repository.WeixinManage;

namespace WaterCloud.Application.WeixinManage
{
    public class WxPropertyInfoApp
    {
        private WxPropertyInfoRepository service = new WxPropertyInfoRepository();

        public List<WxPropertyInfoEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public List<WxPropertyInfoEntity> GetList(string apiid, string iName)
        {
            var expression = ExtLinq.True<WxPropertyInfoEntity>();
            if (!string.IsNullOrEmpty(apiid))
            {
                expression = expression.And(t => t.WxId.Equals(apiid));
            }
            if (!string.IsNullOrEmpty(iName))
            {
                expression = expression.And(t => t.IName.Equals(iName));
            }
            return service.IQueryable(expression).OrderBy(t => t.WxId).ToList();
        }
        public WxPropertyInfoEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void AddAccessToken(string apiid, string access_token)
        {
            try
            {
                WxPropertyInfoEntity wxProperty = new WxPropertyInfoEntity();
                wxProperty.IName = "access_token";
                wxProperty.TypeId = 1;
                wxProperty.TypeName = "base";
                wxProperty.IContent = access_token;
                wxProperty.ExpiresIn = 1200;
                wxProperty.Count = 1;
                wxProperty.WxId = apiid;
                wxProperty.Status = true;
                wxProperty.Create();
                service.Insert(wxProperty);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Exists(string keyValue)
        {
            WxPropertyInfoEntity entity = service.FindEntity(t => t.WxId == keyValue);
            if (entity != null)
            {
                return true;
            }
            return false;
        }


        public void SubmitForm(WxPropertyInfoEntity mEntity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue)==false)
            {
                service.Update(mEntity);
            }
            else
            {
                mEntity.Create();
                service.Insert(mEntity);
            }
        }

        public WxPropertyInfoEntity GetModel(int keyValue)
        {
            return service.FindEntity(keyValue);
        }
    }
}
