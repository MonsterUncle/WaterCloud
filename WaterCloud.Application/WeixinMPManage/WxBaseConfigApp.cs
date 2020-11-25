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
    public class WxBaseConfigApp
    {
        private WxBaseConfigRepository service = new WxBaseConfigRepository();

        public List<WxBaseConfigEntity> GetList()
        {
            return service.IQueryable().ToList();
        }

        public List<WxBaseConfigEntity> GetList(string keyword = "")
        {
            var expression = ExtLinq.True<WxBaseConfigEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.WxName.Contains(keyword));
                expression = expression.Or(t => t.WxId.Contains(keyword));
                expression = expression.Or(t => t.uuId.Contains(keyword));
            }
            return service.IQueryable(expression).OrderBy(t => t.uuId).ToList();
        }

        public WxBaseConfigEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        
        public void SubmitForm(WxBaseConfigEntity mEntity, string keyValue)
        {
            if (service.IQueryable(t => t.WxCode.Equals(mEntity.WxCode) && !t.uuId.Equals(keyValue)).Count() > 0)
            {
                throw new Exception("微信号已存在");
            }
            
            if (!string.IsNullOrEmpty(keyValue))
            {
                mEntity.Modify(keyValue);
                service.Update(mEntity);
            }
            else
            {
                mEntity.Create();
                service.Insert(mEntity);
            }
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.uuId == keyValue);
        }

        public WxBaseConfigEntity GetModel(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public WxBaseConfigEntity GetDefaultConfig(string apiid="")
        {
            WxBaseConfigEntity model = new WxBaseConfigEntity();
            var expression = ExtLinq.True<WxBaseConfigEntity>();
            if (!string.IsNullOrEmpty(apiid))
            {
                expression = expression.And(t => t.WxCode.Contains(apiid));
            }
            expression = expression.And(t => t.Status==true);
            expression = expression.And(t => t.Status == true);
            return  model = service.IQueryable(expression).FirstOrDefault();
        }

        public bool ExistsWidAndWxId(string apiid, string wxid)
        {
            WxBaseConfigEntity model = new WxBaseConfigEntity();
            var expression = ExtLinq.True<WxBaseConfigEntity>();
            if (!string.IsNullOrEmpty(apiid))
            {
                expression = expression.And(t => t.uuId.Contains(apiid));
                expression = expression.Or(t => t.WxId.Contains(wxid));
            }
            model = service.IQueryable(expression).FirstOrDefault();
            if (model != null)
            {
                return true;
            }
            return false;
        }

        public bool wxCloseKW(string apiid)
        {
            WxBaseConfigEntity model = new WxBaseConfigEntity();
            var expression = ExtLinq.True<WxBaseConfigEntity>();
            if (!string.IsNullOrEmpty(apiid))
            {
                expression = expression.And(t => t.uuId.Contains(apiid));
            }
            model = service.IQueryable(expression).FirstOrDefault();
            if (model != null)
            {
                return model.CloseKW;
            }
            return true;
        }

        public string GetWeiXinToken(string apiid)
        {
            WxBaseConfigEntity model = new WxBaseConfigEntity();
            var expression = ExtLinq.True<WxBaseConfigEntity>();
            if (!string.IsNullOrEmpty(apiid))
            {
                expression = expression.And(t => t.WxCode.Contains(apiid));
                expression = expression.And(t => t.Status==true);
            }
            model = service.IQueryable(expression).FirstOrDefault();
            if (model != null)
            {
                return model.Token ;
            }
            return string.Empty;
        }

        
    }
}
