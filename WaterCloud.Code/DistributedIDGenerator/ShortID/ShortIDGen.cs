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
using System.Linq;
using System.Text;

namespace WaterCloud.Code
{
    /// <summary>
    /// 短 ID 生成核心代码
    /// <para>代码参考自：https://github.com/bolorundurowb/shortid </para>
    /// </summary>
    public static class ShortIDGen
    {
        /// <summary>
        /// 短 ID 生成器期初数据
        /// </summary>
        private static Random _random = new();

        private const string Bigs = "ABCDEFGHIJKLMNPQRSTUVWXY";
        private const string Smalls = "abcdefghjklmnopqrstuvwxyz";
        private const string Numbers = "0123456789";
        private const string Specials = "_-";
        private static string _pool = $"{Smalls}{Bigs}";

        /// <summary>
        /// 线程安全锁
        /// </summary>
        private static readonly object ThreadLock = new();

        /// <summary>
        /// 生成目前比较主流的短 ID
        /// <para>包含字母、数字，不包含特殊字符</para>
        /// <para>默认生成 8 位</para>
        /// </summary>
        /// <returns></returns>
        public static string NextID()
        {
            return NextID(new GenerationOptions
            {
                UseNumbers = true,
                UseSpecialCharacters = false,
                Length = 8
            });
        }

        /// <summary>
        /// 生成短 ID
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string NextID(GenerationOptions options)
        {
            // 配置必填
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // 判断生成的长度是否小于规定的长度，规定为 8
            if (options.Length < Constants.MinimumAutoLength)
            {
                throw new ArgumentException(
                    $"The specified length of {options.Length} is less than the lower limit of {Constants.MinimumAutoLength} to avoid conflicts.");
            }

            var characterPool = _pool;
            var poolBuilder = new StringBuilder(characterPool);

            // 是否包含数字
            if (options.UseNumbers)
            {
                poolBuilder.Append(Numbers);
            }

            // 是否包含特殊字符
            if (options.UseSpecialCharacters)
            {
                poolBuilder.Append(Specials);
            }

            var pool = poolBuilder.ToString();

            // 生成拼接
            var output = new char[options.Length];
            for (var i = 0; i < options.Length; i++)
            {
                lock (ThreadLock)
                {
                    var charIndex = _random.Next(0, pool.Length);
                    output[i] = pool[charIndex];
                }
            }

            return new string(output);
        }

        /// <summary>
        /// 设置参与运算的字符，最少 50 位
        /// </summary>
        /// <param name="characters"></param>
        public static void SetCharacters(string characters)
        {
            if (string.IsNullOrWhiteSpace(characters))
            {
                throw new ArgumentException("The replacement characters must not be null or empty.");
            }

            var charSet = characters
                .ToCharArray()
                .Where(x => !char.IsWhiteSpace(x))
                .Distinct()
                .ToArray();

            if (charSet.Length < Constants.MinimumCharacterSetLength)
            {
                throw new InvalidOperationException(
                    $"The replacement characters must be at least {Constants.MinimumCharacterSetLength} letters in length and without whitespace.");
            }

            lock (ThreadLock)
            {
                _pool = new string(charSet);
            }
        }

        /// <summary>
        /// 设置种子步长
        /// </summary>
        /// <param name="seed"></param>
        public static void SetSeed(int seed)
        {
            lock (ThreadLock)
            {
                _random = new Random(seed);
            }
        }

        /// <summary>
        /// 重置所有配置
        /// </summary>
        public static void Reset()
        {
            lock (ThreadLock)
            {
                _random = new Random();
                _pool = $"{Smalls}{Bigs}";
            }
        }
    }
}