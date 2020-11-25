using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.WeixinMP
{
    public class ResponseBaseMgr
    {
        /// <summary>
        /// 将用户请求的数据和系统回复的数据保存到数据库，数据落地
        /// </summary>
        /// <param name="wid">微帐号主键Id，apiid</param>
        /// <param name="openid">请求的用户openid</param>
        /// <param name="requestType">用户请求的类型：文本消息：text 图片消息:image 地理位置消息:location 链接消息:link 事件:event</param>
        /// <param name="requestContent">用户请求的数据内容</param>
        /// <param name="responseType"> 系统回复的类型：文本消息：text ,图文消息:txtpic ,语音music, 地理位置消息:location 链接消息:link,未取到数据none</param>
        /// <param name="responseContent">系统回复的内容</param>
        /// <param name="ToUserName">由于取不到xml内容，我们将toUserName存入</param>
        /// <returns></returns>
        public static int Add(string apiid, string openid, string requestType, string requestContent, string responseType, string responseContent, string ToUserName)
        {
            int ret = 0;
            try
            {
                ResponseBaseData model = new ResponseBaseData();
                model.openId = openid;
                model.RequestType = requestType;
                model.RequestContent = requestContent;
                model.ResponseType = responseType;
                model.ReponseContent = responseContent;
                model.XmlContent = ToUserName;
                model.CreateDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                ret = 0;
            }


            return ret;
        }
    }
}
