using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.Entities.TemplateMessage;
using WaterCloud.Code;
using WaterCloud.Application.WeixinManage;
using WaterCloud.Entity.WeixinManage;

namespace WaterCloud.Application.CommonService
{
    public class WxTemplateMessageServer
    {
        /// <summary>
        /// 公众号相关信息
        /// </summary>
        private static string AppId = "wxabcde123456xyz";
        private static string AppSecret = "25633c23232d23gc0000d93428b1234";

        public static void SendTemplateMessage(string openId, string templateId,  object data,  string url = null)
        {
            WxBaseConfigApp app = new WxBaseConfigApp();
            WxBaseConfigEntity weixin = app.GetDefaultConfig();
            AccessTokenResult token = AccessTokenContainer.GetAccessTokenResult(weixin.AppId, true);
            if (token.errcode != Senparc.Weixin.ReturnCode.请求成功)
            {
                throw new Exception(token.errmsg);
            }
            if (!string.IsNullOrEmpty(templateId))
            {
                //获取AccessToken
                try
                {
                    var result = TemplateApi.SendTemplateMessage(token.access_token, openId, templateId, url, data);
                    //new BLL.wx_message_template_sendlog().AddLog(AppId, templateId, temp_title, openId, string.Format("【微信公众号发送模板消息接口调用】返回参数:errcode:{0},erromsg:{1},misgid:{2}", result.errcode, result.errmsg, result.msgid));
                }
                catch (Exception ex)
                {
                    throw new Exception("微信模板消息："+ex.Message);
                    //new BLL.wx_message_template_sendlog().AddLog(AppId, templateId, temp_title, openId, string.Format("【微信公众号发送模板消息接口调用】错误：{0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// 模板消息发送
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="openId"></param>
        /// <param name="data"></param>
        /// <param name="temp_title"></param>
        /// <param name="url"></param>
        public static OperateResult<string> SendTemplateMessage(string accessToken, string openId, string templateId, object data, string temp_title, string url = null)
        {
            if (!string.IsNullOrEmpty(templateId))
            {
                //获取AccessToken
                try
                {
                    //var accessToken = AccessTokenContainer.GetAccessToken(AppId);
                    var result = TemplateApi.SendTemplateMessage(AppId, openId, templateId, url, data);
                    // new BLL.wx_message_template_sendlog().AddLog(AppId, templateId, temp_title, openId, string.Format("【微信公众号发送模板消息接口调用】返回参数:errcode:{0},erromsg:{1},misgid:{2}", result.errcode, result.errmsg, result.msgid));
                    OperateResult.CreateSuccessResult(string.Format("【微信公众号发送模板消息接口调用】返回参数:errcode:{0},erromsg:{1},misgid:{2}", result.errcode, result.errmsg, result.msgid));
                }
                catch (Exception ex)
                {
                    //new BLL.wx_message_template_sendlog().AddLog(AppId, templateId, temp_title, openId, string.Format("【微信公众号发送模板消息接口调用】错误：{0}", ex.Message));
                    return  new OperateResult<string>(string.Format("【微信公众号发送模板消息接口调用】错误：{0}", ex.Message));
                }
            }
            return new OperateResult<string>("【微信公众号发送模板消息接口调用】错误：模板消息Id不能为空");
        }
    }
}
