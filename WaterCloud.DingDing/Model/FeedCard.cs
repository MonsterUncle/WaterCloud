using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterCloud.DingTalk
{
    /// <summary>
    /// 订阅消息内容（类似于微信公众号推送消息模板）
    /// </summary>
    public class FeedCard
    {
        public string Title { get; set; }
        public string MessageURL { get; set; }
        public string PicURL { get; set; }
    }
}
