
using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace WaterCloud.Code
{
	/// <summary>
	/// 数值扩展类
	/// </summary>
	public static partial class Extensions
	{
        #region 进制转换
        /// <summary>
        /// 10进制转换到2-36进制
        /// </summary>
        /// <param name="this">10进制数字</param>
        /// <param name="radix">进制，范围2-36</param>
        /// <param name="digits">编码取值规则，最大转换位数不能大于该字符串的长度</param>
        /// <returns></returns>
        public static string ToBase(this long @this, int radix, string digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ")
        {
            const int BitsInLong = 64;

            if (radix < 2 || radix > digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " + digits.Length.ToString());

            if (@this == 0)
                return "0";

            var index = BitsInLong - 1;
            var currentNumber = Math.Abs(@this);
            var charArray = new char[BitsInLong];

            while (currentNumber != 0)
            {
                var remainder = (int)(currentNumber % radix);
                charArray[index--] = digits[remainder];
                currentNumber /= radix;
            }

            var result = new string(charArray, index + 1, BitsInLong - index - 1);
            if (@this < 0)
            {
                result = "-" + result;
            }

            return result;
        }

        /// <summary>
        /// byte转16进制
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string ToHex(this byte @this) => Convert.ToString(@this, 16);

        /// <summary>
        /// 2进制转16进制
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string ToHex(this string @this) => Convert.ToString(Convert.ToInt64(@this, 2), 16);

        /// <summary>
        /// 16进制转2进制
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string ToBinary(this string @this) => Convert.ToString(Convert.ToInt64(@this, 16), 2);

        /// <summary>
        /// 2进制/16进制转8进制
        /// </summary>
        /// <param name="this"></param>
        /// <param name="fromBase">2或者16，表示2进制或者16进制；</param>
        /// <returns></returns>
        public static string ToOctal(this string @this, int fromBase) => Convert.ToString(Convert.ToInt64(@this, fromBase), 8);

        /// <summary>
        /// 2进制/16进制转10进制
        /// </summary>
        /// <param name="this"></param>
        /// <param name="fromBase">2或者16，表示2进制或者16进制；</param>
        /// <returns></returns>
        public static string ToDecimalism(this string @this, int fromBase)
        {
            if (fromBase == 16)
                return Convert.ToInt32(@this, 16).ToString();
            else
                return Convert.ToString(Convert.ToInt64(@this, 2), 10);
        }
        #endregion
    }
}
