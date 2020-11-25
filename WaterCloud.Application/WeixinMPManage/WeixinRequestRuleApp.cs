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
    public class WeixinRequestRuleApp
    {
        private RequestRuleRepository rlservice = new RequestRuleRepository();
        private RequestRuleContentRepository rlcservice = new RequestRuleContentRepository();
        public List<RequestRuleEntity> GetList()
        {
            return rlservice.IQueryable().ToList();
        }
        public RequestRuleEntity GetForm(string keyValue)
        {
            return rlservice.FindEntity(keyValue);
        }
        
        public void SubmitForm(RequestRuleEntity mEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                rlservice.Update(mEntity);
            }
            else
            {
                mEntity.Create();
                rlservice.Insert(mEntity);
            }
        }

        public RequestRuleEntity GetModel(int keyValue)
        {
            return rlservice.FindEntity(keyValue);
        }


        #region 微信前端使用，需要高效率

        /// <summary>
        /// 得到回复规则的主键Id
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public string GetRuleId(string keyValue)
        {
            return rlservice.FindEntity(keyValue).uuId;
        }

        /// <summary>
        /// 得到回复规则的主键Id以及回复的类型
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="responseType"></param>
        /// <returns></returns>
        public string GetRuleIdAndResponseType(string apiid,string keyValue, out int responseType)
        {
            int ret = 0;
            responseType = 0;
            RequestRuleEntity model= rlservice.FindEntity(keyValue);
            if (model != null)
            {
                responseType = model.ResponseType;
            }
            return model.uuId;
        }


        /// <summary>
        /// 关键词查询，找到该规则对应的主键ID,使用存储过程
        /// </summary>
        /// <param name="keywords">关键词</param>
        /// <param name="responseType">回复类型</param>
        /// <param name="modelFunctionName">模块名称</param>
        /// <param name="modelFunctionId">模块Id</param>
        /// <returns></returns>
        public string GetRuleIdByKeyWords(string apiid, string keywords, out int responseType, out string modelFunctionName, out int modelFunctionId)
        {

            List<RequestRuleEntity> list = GetList();
            var expression = ExtLinq.True<RequestRuleEntity>();
            if (!string.IsNullOrEmpty(keywords))
            {
                expression = expression.And(t => t.ReqestType.Contains("1"));
                expression = expression.Or(t => t.ReqKeywords.Contains(keywords));
            }
            list= rlservice.IQueryable(expression).OrderBy(t => t.CreateDate).ToList();
            if (list!=null && list.Count > 0)
            {
                responseType = list[0].ResponseType;
                modelFunctionName = list[0].ModelFunctionName;
                modelFunctionId = list[0].ModelFunctionId;
            }else
            {
                responseType = 0;
                modelFunctionName = string.Empty;
                modelFunctionId = 0;
            }
            return list[0].uuId;
        }



        /// <summary>
        /// 得到回复规则的纯文本信息
        /// </summary>
        /// <param name="rid">规则主键Id</param>
        /// <returns></returns>
        public string GetTxtContent(string rid)
        {
            
            RequestRuleContentEntity model =new RequestRuleContentEntity();
            var expression = ExtLinq.True<RequestRuleContentEntity>();
            expression = expression.And(t => t.RId==rid);
            model = rlcservice.IQueryable(expression).OrderBy(t => t.CreateDate).FirstOrDefault();
            if (model != null)
            {
                return  model.RContent;
            }
            return null;
        }


        /// <summary>
        /// 得到回复规则的语音信息
        /// </summary>
        /// <param name="rid">规则主键Id</param>
        /// <returns></returns>
        public RequestRuleContentEntity GetMusicContent(string rid)
        {
            RequestRuleContentEntity model = new RequestRuleContentEntity();
            var expression = ExtLinq.True<RequestRuleContentEntity>();
            expression = expression.And(t => t.RId == rid);
            model = rlcservice.IQueryable(expression).OrderBy(t => t.CreateDate).FirstOrDefault();
            
            return model;
        }

        /// <summary>
        /// 得到回复规则的【图文】信息
        /// </summary>
        /// <param name="rid">规则主键Id</param>
        /// <returns></returns>
        public IList<RequestRuleContentEntity> GetTuWenContent(string rid)
        {
            IList<RequestRuleContentEntity> rcList = new List<RequestRuleContentEntity>();
            var expression = ExtLinq.True<RequestRuleContentEntity>();
            expression = expression.And(t => t.RId == rid);
            rcList = rlcservice.IQueryable(expression).OrderBy(t => t.CreateDate).ToList();

            return rcList;
        }
        

        #endregion
    }
}
