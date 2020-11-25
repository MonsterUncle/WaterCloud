using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterCloud.DingTalk.Entities
{
    /// <summary>
    /// 钉钉通用枚举
    /// </summary>
    public class EnumClass
    {
        /// <summary>
        /// 通知消息类型
        /// </summary>
        public enum MsgType
        {

            text,
            image,
            file,
            link,
            markdown,
            oa,
            action_card
        }
        /// <summary>
        /// 消息接收者类型
        /// </summary>
        public enum RecvType
        {
            UseridList,
            DeptList,
            AllUser
        }
    }

}
