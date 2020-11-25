using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;

namespace WaterCloud.DingTalk
{
    /// <summary>
    /// 自定义机器人
    /// 宜宾机电一体化研究所：吴远德
    /// 相关API开发文档见：https://open-doc.dingtalk.com/microapp/serverapi2/qf2nxq
    /// 需要结合钉钉API接口.Net版本动态库文件TopSdk，引用 DingTalk.Api
    /// 创建时间：2018-12-15
    /// </summary>
    public class DingTalkRobotApi
    {

        /// <summary>
        /// 发送自定义机器人消息（text类型）
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="content"></param>
        /// <param name="mobiles"></param>
        public static bool SendTextMsg(string access_token,string content,List<string> mobiles)
        {
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/robot/send?access_token="+ access_token);
            OapiRobotSendRequest request = new OapiRobotSendRequest();
            request.Msgtype="text";
            OapiRobotSendRequest.TextDomain text = new OapiRobotSendRequest.TextDomain();
            text.Content= content;
            request.Text_=text;
            OapiRobotSendRequest.AtDomain at = new OapiRobotSendRequest.AtDomain();
            at.AtMobiles= mobiles;
            request.At_=at;
            OapiRobotSendResponse response = client.Execute(request);
            if (response.Errcode == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 发送自定义机器人消息（link类型）
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="linkUrl"></param>
        /// <param name="picUrl"></param>
        /// <param name="mobiles"></param>
        public static bool SendLinkMsg(string access_token,string title, string content,string linkUrl,string picUrl, List<string> mobiles)
        {
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/robot/send?access_token=" + access_token);
            OapiRobotSendRequest request = new OapiRobotSendRequest();
            request.Msgtype = "link";
            OapiRobotSendRequest.LinkDomain link = new OapiRobotSendRequest.LinkDomain();
            link.MessageUrl= linkUrl;
            link.PicUrl= picUrl;
            link.Title= title;
            link.Text= content;
            request.Link_=link;
            OapiRobotSendRequest.AtDomain at = new OapiRobotSendRequest.AtDomain();
            at.AtMobiles = mobiles;
            request.At_ = at;
            OapiRobotSendResponse response = client.Execute(request);
            if (response.Errcode == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 发送自定义机器人消息（markdown类型）
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="linkUrl"></param>
        /// <param name="picUrl"></param>
        /// <param name="mobiles"></param>
        public static bool SendMarkdownMsg(string access_token, string title, string content, string linkUrl, List<string> mobiles)
        {
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/robot/send?access_token=" + access_token);
            OapiRobotSendRequest request = new OapiRobotSendRequest();
            request.Msgtype = "markdown";
            OapiRobotSendRequest.MarkdownDomain markdown = new OapiRobotSendRequest.MarkdownDomain();
            markdown.Title = title;
            markdown.Text = content;
            request.Markdown_=markdown;
            OapiRobotSendRequest.AtDomain at = new OapiRobotSendRequest.AtDomain();
            at.AtMobiles = mobiles;
            request.At_ = at;
            OapiRobotSendResponse response = client.Execute(request);
            if (response.Errcode == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 发送自定义机器人消息（Actioncard类型）
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="linkUrl"></param>
        /// <param name="picUrl"></param>
        /// <param name="mobiles"></param>
        /// <returns></returns>
        public static bool SendActionCardMsg(string access_token, string title, string content, string linkUrl, string picUrl, List<string> mobiles)
        {
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/robot/send?access_token=" + access_token);
            OapiRobotSendRequest request = new OapiRobotSendRequest();
            request.Msgtype = "actionCard";
            OapiRobotSendRequest.ActioncardDomain actionCard = new OapiRobotSendRequest.ActioncardDomain();
            actionCard.Title = title;
            actionCard.Text = content;
            actionCard.SingleTitle = "查看详情";
            actionCard.SingleURL = linkUrl;
            actionCard.BtnOrientation = "1";//0-按钮竖直排列，1-按钮横向排列
            actionCard.HideAvatar = "1";//0-正常发消息者头像,1-隐藏发消息者头像
            request.ActionCard_ = actionCard;
            if (mobiles!=null && mobiles.Count>0)
            {
                OapiRobotSendRequest.AtDomain at = new OapiRobotSendRequest.AtDomain();
                at.AtMobiles = mobiles;
                request.At_ = at;
            }
            OapiRobotSendResponse response = client.Execute(request);
            if (response.Errcode == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 发送自定义机器人消息（FeedCard类型:公号文章列表）
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="linkUrl"></param>
        /// <param name="picUrl"></param>
        /// <param name="mobiles"></param>
        /// <returns></returns>
        public static bool SendFeedCardMsg(string access_token, string title, string content, List<FeedCard> feedcardList)
        {
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/robot/send?access_token=" + access_token);
            OapiRobotSendRequest request = new OapiRobotSendRequest();
            request.Msgtype = "feedCard";
            OapiRobotSendRequest.FeedcardDomain feedCard = new OapiRobotSendRequest.FeedcardDomain();
            List<OapiRobotSendRequest.LinksDomain> links = new List<OapiRobotSendRequest.LinksDomain>();
            foreach(FeedCard card in feedcardList)
            {
                OapiRobotSendRequest.LinksDomain link = new OapiRobotSendRequest.LinksDomain();
                link.Title = card.Title;
                link.MessageURL = card.MessageURL;
                link.PicURL = card.PicURL;
                links.Add(link);
            }
            feedCard.Links = links;
            OapiRobotSendResponse response = client.Execute(request);
            if (response.Errcode == 0)
            {
                return true;
            }
            return false;
        }
    }
}
