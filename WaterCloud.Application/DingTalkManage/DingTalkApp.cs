/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/

using System.Collections.Generic;
using System;
using System.Linq;
using WaterCloud.DingTalk;
using WaterCloud.Code;
using WaterCloud.Entity.DingTalkManage;
using WaterCloud.Repository.DingTalkManage;

namespace WaterCloud.Application.DingTalkManage
{
    public class DingTalkApp
    {


        public DingTalkCorpConfigEntity GetForm()
        {
            Code.SysConfig.DingTalkConfig entity = new Code.SysConfig.DingTalkConfigApp().LoadConfig();
            if (entity != null)
            {
                DingTalkCorpConfigEntity model = new DingTalkCorpConfigEntity();
                model.CorpId = entity.CorpId;
                model.CorpSecret = entity.CorpSecret;
                model.AppName = entity.AppName;
                model.HeadPic = entity.HeadPic;
                model.ApiUrl = entity.ApiUrl;
                model.AgentId = entity.AgentId;
                model.AppKey = entity.AppKey;
                model.AppSecret = entity.AppSecret;
                model.Encoding = entity.Encoding;
                model.AppType = (int)entity.AppType;
                return model;
            }
            else
            {
                return null;
            }
        }

        public void SubmitForm(DingTalkCorpConfigEntity mEntity)
        {
            try
            {
                Code.SysConfig.DingTalkConfig model = new Code.SysConfig.DingTalkConfigApp().LoadConfig();
                model.CorpId = mEntity.CorpId;
                model.CorpSecret = mEntity.CorpSecret;
                model.AppName = mEntity.AppName;
                model.HeadPic = mEntity.HeadPic;
                model.ApiUrl = mEntity.ApiUrl;
                model.AgentId = mEntity.AgentId;
                model.AppKey = mEntity.AppKey;
                model.AppSecret = mEntity.AppSecret;
                model.Encoding = mEntity.Encoding;
                model.AppType = mEntity.AppType;

                new Code.SysConfig.DingTalkConfigApp().SaveConifg(model);
            }
            catch (Exception ex)
            {
                throw new Exception("保存出错：" + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DingTalkCorpConfig GetCorpConfig()
        {
            Code.SysConfig.DingTalkConfig entity = new Code.SysConfig.DingTalkConfigApp().LoadConfig();
            if (entity != null)
            {
                DingTalkCorpConfig config = new DingTalkCorpConfig();

                config.CorpId = entity.CorpId;
                config.CorpSecret = entity.CorpSecret;
                config.AgentId = entity.AgentId;
                config.AppName = entity.AppName;
                config.AppKey = entity.AppKey;
                config.AppSecret = entity.AppSecret;
                //config.HeadPic = entity.HeadPic;
                return config;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DingTalkAppConfig GetAppConfig(string appcode)
        {
            Code.SysConfig.DingTalkConfig entity = new Code.SysConfig.DingTalkConfigApp().LoadConfig();
            if (entity != null)
            {
                DingTalkAppConfig config = new DingTalkAppConfig();
                config.AgentId = entity.AgentId;
                config.AppName = entity.AppName;
                config.AppKey = entity.AppKey;
                config.AppSecret = entity.AppSecret;
                return config;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取GetAccessToken
        /// </summary>
        /// <returns></returns>
        public DingTalkCorpConfig GetAccessToken()
        {
            if (DingTalkHelper.CorpConfig != null)
            {
                if (string.IsNullOrEmpty(DingTalkHelper.CorpConfig.CorpId) || string.IsNullOrEmpty(DingTalkHelper.CorpConfig.CorpSecret))
                {
                    DingTalkHelper.CorpConfig = GetCorpConfig();
                    DingTalkHelper.CorpConfig.AccessToken = DingTalkHelper.GetAccessToken(DingTalkHelper.CorpConfig.CorpId, DingTalkHelper.CorpConfig.CorpSecret);
                    DingTalkHelper.CorpConfig.ExpiresTime = DateTime.Now;
                }
                else
                {
                    TimeSpan ts = DateTime.Now - DingTalkHelper.AppConfig.ExpiresTime;
                    if (ts.TotalSeconds >= DingTalkHelper.CorpConfig.ExpiresIn)
                    {
                        DingTalkHelper.CorpConfig.AccessToken = DingTalkHelper.GetAccessToken(DingTalkHelper.CorpConfig.CorpId, DingTalkHelper.CorpConfig.CorpSecret);
                        DingTalkHelper.CorpConfig.ExpiresTime = DateTime.Now;
                    }
                }
                return DingTalkHelper.CorpConfig;
            }
            else
            {
                DingTalkHelper.CorpConfig = GetCorpConfig();
                DingTalkHelper.CorpConfig.AccessToken = DingTalkHelper.GetAccessToken(DingTalkHelper.CorpConfig.CorpId, DingTalkHelper.CorpConfig.CorpSecret);
                DingTalkHelper.CorpConfig.ExpiresTime = DateTime.Now;
                return DingTalkHelper.CorpConfig;
            }
        }
    }
}
