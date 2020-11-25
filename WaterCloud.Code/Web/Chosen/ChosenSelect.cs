/*******************************************************************************
 * Copyright © 2016 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System.Collections.Generic;
using System.Text;

namespace WaterCloud.Code
{
    public static class ChosenSelect
    {
        public static string ChosenSelectJson(this List<ChosenSelectModel> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(ChosenSelectJson(data, "0", ""));
            sb.Append("]");
            return sb.ToString();
        }
        private static string ChosenSelectJson(List<ChosenSelectModel> data, string parentId, string blank)
        {
            StringBuilder sb = new StringBuilder();
            
            foreach (ChosenSelectModel entity in data)
            {
                string strJson = entity.ToJson();
                sb.Append(strJson);
            }
            return sb.ToString().Replace("}{", "},{");
        }
    }
}