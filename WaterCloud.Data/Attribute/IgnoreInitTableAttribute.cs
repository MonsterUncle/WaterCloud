using System;

namespace WaterCloud.DataBase
{
    [AttributeUsage(AttributeTargets.Class)]
    public class IgnoreInitTableAttribute : Attribute
    {
    }

    /// <summary>
    /// 种子数据忽略修改
    /// </summary>
    public class IgnoreSeedDataUpdateAttribute : Attribute
    {
    }
}