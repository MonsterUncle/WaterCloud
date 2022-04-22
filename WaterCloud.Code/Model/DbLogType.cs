/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System;
using System.ComponentModel;

namespace WaterCloud.Code
{
	public enum DbLogType
    {
        [Description("其他")]
        Other = 0,
        [Description("登录")]
        Login = 1,
        [Description("退出")]
        Exit = 2,
        [Description("访问")]
        Visit = 3,
        [Description("新增")]
        Create = 4,
        [Description("删除")]
        Delete = 5,
        [Description("修改")]
        Update = 6,
        [Description("提交")]
        Submit = 7,
        [Description("异常")]
        Exception = 8,
    }
}
