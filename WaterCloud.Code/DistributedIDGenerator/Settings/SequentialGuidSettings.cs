﻿// -----------------------------------------------------------------------------
// 让 .NET 开发更简单，更通用，更流行。
// Copyright © 2020-2021 Furion, 百小僧, Baiqian Co.,Ltd.
//
// 框架名称：Furion
// 框架作者：百小僧
// 框架版本：2.7.9
// 源码地址：Gitee： https://gitee.com/dotnetchina/Furion
//          Github：https://github.com/monksoul/Furion
// 开源协议：Apache-2.0（https://gitee.com/dotnetchina/Furion/blob/master/LICENSE）
// -----------------------------------------------------------------------------

using System;

namespace WaterCloud.Code
{
    /// <summary>
    /// 连续 GUID 配置
    /// </summary>
    public sealed class SequentialGuidSettings
    {
        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTimeOffset TimeNow { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// LittleEndianBinary 16 格式化
        /// </summary>
        public bool LittleEndianBinary16Format { get; set; } = false;
    }
}