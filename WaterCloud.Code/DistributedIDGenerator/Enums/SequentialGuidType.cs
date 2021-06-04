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

using System.ComponentModel;

namespace WaterCloud.Code
{
    /// <summary>
    /// 连续 GUID 类型选项
    /// </summary>
    public enum SequentialGuidType
    {
        /// <summary>
        /// 标准连续 GUID 字符串
        /// </summary>
        [Description("标准连续 GUID 字符串")]
        SequentialAsString,

        /// <summary>
        /// Byte 数组类型的连续 `GUID` 字符串
        /// </summary>
        [Description("Byte 数组类型的连续 `GUID` 字符串")]
        SequentialAsBinary,

        /// <summary>
        /// 连续部分在末尾展示
        /// </summary>
        [Description("连续部分在末尾展示")]
        SequentialAtEnd
    }
}