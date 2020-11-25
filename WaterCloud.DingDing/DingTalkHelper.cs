using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using WaterCloud.DingTalk.Entities;
using System.Web;
using DingTalk.Api;

namespace WaterCloud.DingTalk
{
    /// <summary>
    /// 钉钉AccessToken相关业务接口
    /// 宜宾机电一体化研究所：吴远德
    /// 相关API开发文档见：https://open-doc.dingtalk.com/microapp/serverapi2/pgoxpy
    /// 需要结合钉钉API接口.Net版本动态库文件TopSdk，引用 DingTalk.Api
    /// 创建时间：2018-12-15
    /// </summary>
    public class DingTalkHelper
    {
        /// <summary>
        /// 定义一个公共的钉钉密钥存储类
        /// </summary>
        public static DingTalkCorpConfig CorpConfig = new DingTalkCorpConfig();
        public static DingTalkAppConfig AppConfig = new DingTalkAppConfig();
        /// <summary>
        /// 企业内部通过本企业的CorpId和CorpSecret，来换取access_token
        /// </summary>
        /// <param name="corpId"></param>
        /// <param name="corpSecret"></param>
        /// <returns></returns>
        public static string GetAccessToken(string corpId, string corpSecret)
        {
            /*
             * https://oapi.dingtalk.com/gettoken?corpid={corpid}&corpsecret={corpSecret或开发授权码}
             * "errcode": 0,
             * "errmsg": "ok",
             * "access_token": "fw8ef8we8f76e6f7s8df8s"
             */
            try
            {
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/gettoken");
                OapiGettokenRequest request = new OapiGettokenRequest();
                request.Corpid = corpId;
                request.Corpsecret = corpSecret;
                request.SetHttpMethod("GET");
                OapiGettokenResponse response = client.Execute(request);
                if (response != null && response.Errcode == 0)
                {
                    return response.AccessToken;
                }
                else
                {
                    throw new Exception(response.ErrMsg);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return string.Empty;
        }

        /// <summary>
        /// 企业开发内部应用通过appkey和appsecret获取access_token
        /// </summary>
        /// <param name="corpId"></param>
        /// <param name="corpSecret"></param>
        /// <returns></returns>
        public static string GetAccessToken2(string appkey, string appsecret)
        {
            /*
             * https://oapi.dingtalk.com/gettoken?appkey=id&appsecret=secret
             * "errcode": 0,
             * "errmsg": "ok",
             * "access_token": "fw8ef8we8f76e6f7s8df8s"
             */
            try
            {
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/gettoken");
                OapiGettokenRequest request = new OapiGettokenRequest();
                request.Appkey= appkey;
                request.Appsecret= appsecret;
                request.SetHttpMethod("GET");
                OapiGettokenResponse response = client.Execute(request);

                if (response.Errcode ==0)
                {
                    return response.AccessToken;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return string.Empty;
        }


        /// <summary> 
        /// 获取 JsApiTicket 
        /// </summary> 
        /// <returns></returns> 
        public static string GetJsApiTicket(string accessToken)
        {
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/get_jsapi_ticket");
            OapiGetJsapiTicketRequest req = new OapiGetJsapiTicketRequest();
            req.SetHttpMethod("GET");
            OapiGetJsapiTicketResponse response = client.Execute(req, accessToken);
            if (response != null && response.Errcode == 0)
            {
                return response.Ticket;
            }
            return null;
        }


        /// <summary>
        ///开发者在web页面使用钉钉容器提供的jsapi时，需要验证调用权限，并以参数signature标识合法性
        /// </summary>
        /// <param name="noncestr">随机字符串，自己随便填写即可</param>
        /// <param name="sTimeStamp">当前时间戳，具体值为当前时间到1970年1月1号的秒数</param>
        /// <param name="jsapi_ticket">获取的jsapi_ticket</param>
        /// <param name="url">当前网页的URL，不包含#及其后面部分</param>
        /// <param name="signature">生成的签名</param>
        /// <returns>0 成功，null 失败</returns>
        public static string GetAccessSignature(string noncestr, string sTimeStamp, string jsapi_ticket, string url)
        {

            //步骤1.sort()含义为对所有待签名参数按照字段名的ASCII 码从小到大排序（字典序）
            //注意，此处是是按照【字段名】的ASCII字典序，而不是参数值的字典序（这个细节折磨我很久了)
            //0:jsapi_ticket 1:noncestr 2:timestamp 3:url;

            //步骤2.assemble()含义为根据步骤1中获的参数字段的顺序，使用URL键值对的格式（即key1 = value1 & key2 = value2…）拼接成字符串
            //string assemble = "jsapi_ticket=3fOo5UfWhmvRKnRGMmm6cWwmIxDMCnniyVYL2fqcz1I4GNU4054IOlif0dZjDaXUScEjoOnJWOVrdwTCkYrwSl&noncestr=CUMT1987wlrrlw&timestamp=1461565921&url=https://jackwangcumt.github.io/home.html";
            string assemble = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsapi_ticket, noncestr, sTimeStamp, url);
            //步骤2.sha1()的含义为对在步骤2拼接好的字符串进行sha1加密。
            SHA1 sha;
            ASCIIEncoding enc;
            string hash = "";
            string signature = string.Empty;
            try
            {
                sha = new SHA1CryptoServiceProvider();
                enc = new ASCIIEncoding();
                byte[] dataToHash = enc.GetBytes(assemble);
                byte[] dataHashed = sha.ComputeHash(dataToHash);
                hash = BitConverter.ToString(dataHashed).Replace("-", "");
                hash = hash.ToLower();
            }
            catch (Exception)
            {
                return string.Empty;
            }
            signature = hash;
            return signature;

        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="nonceStr"></param>
        /// <param name="timeStamp"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetAccessSignature(string nonceStr, long timeStamp, string ticket, string url)
        {
            string plain = "jsapi_ticket=" + ticket + "&noncestr=" + nonceStr + "&timestamp=" + timeStamp.ToString()
                + "&url=" + url;
            string hash = "";
            try
            {
                hash=MD5Helper.Sha1(plain);
                return hash;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #region  拓展应用

        /// <summary>
        /// 获时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary> 
        /// 根据GUID获取16位的唯一字符串
        /// </summary> 
        /// <param name=\"guid\"></param>
        /// <returns></returns> 
        public static string GuidTo16String()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 字典排序
        /// </summary>
        public class DictionarySort : System.Collections.IComparer
        {
            public int Compare(object oLeft, object oRight)
            {
                string sLeft = oLeft as string;
                string sRight = oRight as string;
                int iLeftLength = sLeft.Length;
                int iRightLength = sRight.Length;
                int index = 0;
                while (index < iLeftLength && index < iRightLength)
                {
                    if (sLeft[index] < sRight[index])
                        return -1;
                    else if (sLeft[index] > sRight[index])
                        return 1;
                    else
                        index++;
                }
                return iLeftLength - iRightLength;
            }
        }

        #endregion
    }
}
