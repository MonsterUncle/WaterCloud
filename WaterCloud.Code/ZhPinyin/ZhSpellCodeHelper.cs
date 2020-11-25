using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Code
{
    public class ZhSpellCodeHelper
    {
        #region 获取首字母
        /// <summary>
        /// 在指定的字符串列表CnStr中检索符合拼音索引字符串
        /// </summary>
        /// <param name="CnStr">汉字字符串</param>
        /// <returns>相对应的汉语拼音首字母串</returns>
        public static string GetFirstCode(string CnStr)
        {
            string Surname = CnStr.Substring(0, 1);
            string strTemp = GetSpellCode(Surname);
            return strTemp;
        }

        /// <summary>
        /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母
        /// </summary>
        /// <param name="CnChar">单个汉字</param>
        /// <returns>单个大写字母</returns>
        private static string GetSpellCode(string CnChar)
        {
            long iCnChar;
            byte[] arrCN = System.Text.Encoding.Default.GetBytes(CnChar);

            //如果是字母，则直接返回
            if (arrCN.Length == 1)
            {
                CnChar = CnChar.ToUpper();
            }
            else
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                iCnChar = (area << 8) + pos;

                // iCnChar match the constant
                string letter = "ABCDEFGHJKLMNOPQRSTWXYZ";
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614,
48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906,
51387, 51446, 52218, 52698, 52980, 53689, 54481, 55290 };
                for (int i = 0; i < 23; i++)
                {
                    if (areacode[i] <= iCnChar && iCnChar < areacode[i + 1])
                    {
                        CnChar = letter.Substring(i, 1);
                        break;
                    }
                }
            }
            return CnChar;
        }
        #endregion
    }
}
