using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;

using Aliyun.Acs.Dybaseapi.Model.V20170525;
//using Aliyun.Acs.Dybaseapi.Model.V20170525.QueryTokenForMnsQueueResponse.MessageTokenDTO_;
using Aliyun.MNS;
using Aliyun.MNS.Model;
using System.Threading;

using Aliyun.Acs.Dysmsapi.Model.V20170525;


namespace WaterCloud.Net.Sms
{
    public class AliSmsServer
    {
        private static String product = "Dysmsapi";//短信API产品名称（短信产品名固定，无需修改）
        private static String domain = "dysmsapi.aliyuncs.com";//短信API产品域名（接口地址固定，无需修改）
        private static String accessId = "";
        private static String accessSecret = "";
        private static String regionIdForPop = "cn-hangzhou";

        public static void InitAliSms(string _AccessKeyId, string _AccessSecret)
        {
            accessId = _AccessKeyId;
            accessSecret = _AccessSecret;
        }
        /// <summary>
        ///  签名字符
        /// </summary>
        public static String SignName { get;  set; }
        /// <summary>
        ///  短信模板Code
        /// </summary>
        public static String TemplateCode { get;  set; }
        /// <summary>
        ///  提供给业务方扩展字段
        /// </summary>
        public String OutId { get; private set; }
        
        public static bool SendSms(string phoneNumbers,string signName,string templateCode,Dictionary<string,string> templateParam)
        {
            IClientProfile profile = DefaultProfile.GetProfile(regionIdForPop, accessId, accessSecret);
            DefaultProfile.AddEndpoint(regionIdForPop, regionIdForPop, product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            try
            {
                StringBuilder templateParamStr =new StringBuilder();
                if (templateParam.Count > 0)
                {
                    templateParamStr.Append("{");
                    foreach (KeyValuePair<string, string> kvp in templateParam)
                    {
                        templateParamStr.Append("\"" + kvp.Key+"\":\""+kvp.Value+"\",");
                    }
                    templateParamStr.Append("}");
                    templateParamStr.Replace("\",}", "\"}");
                }
                //request.SignName = "上云预发测试";//"管理控制台中配置的短信签名（状态必须是验证通过）"
                //request.TemplateCode = "SMS_71130001";//管理控制台中配置的审核通过的短信模板的模板CODE（状态必须是验证通过）"
                //request.RecNum = "13567939485";//"接收号码，多个号码可以逗号分隔"
                //request.ParamString = "{\"name\":\"Tom\", \"code\":\"123\"}";//短信模板中的变量；数字需要转换为字符串；个人用户每个变量长度必须小于15个字符。"
                //request.TemplateCode = "SMS_xxxxxxx"; // 必填:短信模板-可在短信控制台中找到，发送国际/港澳台消息时，请使用国际/港澳台短信模版
                //request.TemplateParam = "{\"code\":\"123\"}"; //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                //SingleSendSmsResponse httpResponse = client.GetAcsResponse(request);


                request.PhoneNumbers = phoneNumbers;
                request.SignName = signName;
                request.TemplateCode = templateCode;
                request.TemplateParam = templateParamStr.ToString();
                //request.OutId = "xxxxxxxx"; //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                if(sendSmsResponse.Code != null && sendSmsResponse.Code.Equals("OK"))
                {
                    return true;
                }
                System.Console.WriteLine(sendSmsResponse.Message);
                return false;
            }
            catch (ServerException e)
            {
                System.Console.WriteLine("ServerException");
                return false;
            }
            catch (ClientException e)
            {
                System.Console.WriteLine("ClientException");
                return false;
            }
        }

        public static void ReciveMsg()
        {

        }
    }
}
