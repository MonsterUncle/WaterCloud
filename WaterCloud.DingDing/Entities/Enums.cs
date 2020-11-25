using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterCloud.DingTalk.Entities
{
    public enum MsgTypeEnums
    {
        text,
        file,
        link,
        markdown,
        oa,
        action_card
    }

    /// <summary>
    /// 消息接收类型
    /// </summary>
    public enum MsgUserType
    {
        UserList,
        DepartList,
        AllUser
    }
}
