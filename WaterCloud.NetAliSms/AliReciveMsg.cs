using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;

using Aliyun.Acs.Dybaseapi.Model.V20170525;
//using Aliyun.Acs.Dybaseapi.Model.V20170525.QueryTokenForMnsQueueResponse.MessageTokenDTO_;
using Aliyun.MNS;
using Aliyun.MNS.Model;
using System.Threading;
using System.Threading.Tasks;

namespace WaterCloud.Net.Sms
{
    public class AliReciveMsg
    {
        private static int maxThread = 2;
        static String accessId = "";
        static String accessSecret = "";
        static String messageType = "SmsReport";//消息类型目前有4种. 短信回执:SmsReport; 短信上行:SmsUp; 语音回执:VoiceReport; 流量回执:FlowReport;
        static String queueName = "Alicom-Queue-17888888794453-SmsReport";// 短信上行的队列名称. 格式是 "前缀(Alicom-Queue-)+阿里云uid+消息类型"
        static String domainForPop = "dybaseapi.aliyuncs.com";
        static String regionIdForPop = "cn-hangzhou";
        static String productName = "Dybaseapi";
        static IAcsClient acsClient = null;

        public static IAcsClient InitAcsClient(String regionIdForPop, String accessId, String accessSecret, String productName, String domainForPop)
        {
            IClientProfile profile = DefaultProfile.GetProfile(regionIdForPop, accessId, accessSecret);
            DefaultProfile.AddEndpoint(regionIdForPop, regionIdForPop, productName, domainForPop);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            return acsClient;
        }

        // 初始化环境
        private static void InitData()
        {
            acsClient = InitAcsClient(regionIdForPop, accessId, accessSecret, productName, domainForPop);
        }
    }

    class TestTask
    {
        private object o = new object();
        private int sleepTime = 50;
        public String name { get; private set; }
        public String messageType { get; private set; }
        public String queueName { get; private set; }
        public int TaskID { get; private set; }
        public IAcsClient acsClient { get; private set; }

        public TestTask(String name, String messageType, String queueName, IAcsClient acsClient)
        {
            this.name = name;
            this.messageType = messageType;
            this.queueName = queueName;
            this.acsClient = acsClient;
        }
        long bufferTime = 60 * 2;//过期时间小于2分钟则重新获取，防止服务器时间误差
        String mnsAccountEndpoint = "https://1943695596114318.mns.cn-hangzhou.aliyuncs.com/";//阿里通信消息的endpoint,固定
        Dictionary<string, QueryTokenForMnsQueueResponse.QueryTokenForMnsQueue_MessageTokenDTO> tokenMap = new Dictionary<string, QueryTokenForMnsQueueResponse.QueryTokenForMnsQueue_MessageTokenDTO>();
        Dictionary<string, Queue> queueMap = new Dictionary<string, Queue>();

        public QueryTokenForMnsQueueResponse.QueryTokenForMnsQueue_MessageTokenDTO GetTokenByMessageType(IAcsClient acsClient, String messageType)
        {

            QueryTokenForMnsQueueRequest request = new QueryTokenForMnsQueueRequest();
            request.MessageType = messageType;
            QueryTokenForMnsQueueResponse queryTokenForMnsQueueResponse = acsClient.GetAcsResponse(request);
            QueryTokenForMnsQueueResponse.QueryTokenForMnsQueue_MessageTokenDTO token = queryTokenForMnsQueueResponse.MessageTokenDTO;
            return token;
        }

        /// 处理消息
        public void Handle()
        {
            while (true)
            {
                try
                {
                    QueryTokenForMnsQueueResponse.QueryTokenForMnsQueue_MessageTokenDTO token = null;
                    Queue queue = null;
                    lock (o)
                    {
                        if (tokenMap.ContainsKey(messageType))
                        {
                            token = tokenMap[messageType];
                        }
                        if (queueMap.ContainsKey(queueName))
                        {
                            queue = queueMap[queueName];
                        }
                        TimeSpan ts = new TimeSpan(0);
                        if (token != null)
                        {
                            DateTime b = Convert.ToDateTime(token.ExpireTime);
                            DateTime c = Convert.ToDateTime(DateTime.Now);
                            ts = b - c;
                        }
                        if (token == null || ts.TotalSeconds < bufferTime || queue == null)
                        {
                            token = GetTokenByMessageType(acsClient, messageType);
                            IMNS client = new Aliyun.MNS.MNSClient(token.AccessKeyId, token.AccessKeySecret, mnsAccountEndpoint, token.SecurityToken);
                            queue = client.GetNativeQueue(queueName);
                            if (tokenMap.ContainsKey(messageType))
                            {
                                tokenMap.Remove(messageType);
                            }
                            if (queueMap.ContainsKey(queueName))
                            {
                                queueMap.Remove(queueName);
                            }
                            tokenMap.Add(messageType, token);
                            queueMap.Add(queueName, queue);
                        }
                    }
                    BatchReceiveMessageResponse batchReceiveMessageResponse = queue.BatchReceiveMessage(16);
                    List<Message> messages = batchReceiveMessageResponse.Messages;
                    for (int i = 0; i <= messages.Count - 1; i++)
                    {
                        try
                        {
                            byte[] outputb = Convert.FromBase64String(messages[i].Body);
                            string orgStr = Encoding.UTF8.GetString(outputb);
                            System.Console.WriteLine(orgStr);
                            //TODO 具体消费逻辑,待客户自己实现.

                            //消费成功的前提下删除消息
                            queue.DeleteMessage(messages[i].ReceiptHandle);
                        }
                        catch (Exception e)
                        {
                            System.Console.WriteLine("Hello World!", e);
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Handle exception", e);
                }
                Thread.Sleep(sleepTime);
            }
        }


    }

}
