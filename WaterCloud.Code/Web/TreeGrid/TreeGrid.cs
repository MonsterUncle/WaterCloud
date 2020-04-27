/*******************************************************************************
 * Copyright © 2016 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using Serenity;
using System.Collections.Generic;
using System.Text;

namespace WaterCloud.Code
{
    public static class TreeGrid
    {
        public static string TreeGridJson(this List<TreeGridModel> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TreeGridJson(data, "0"));
            return sb.ToString();
        }
        private static string TreeGridJson(List<TreeGridModel> data,  string parentId)
        {
            StringBuilder sb = new StringBuilder();
            var ChildNodeList = data.FindAll(t => t.parentId == parentId);
            sb.Append("[");
            if (ChildNodeList.Count > 0) {
                foreach (TreeGridModel entity in ChildNodeList)
                {
                    string strJson = entity.ToJson()+",";
                    strJson = strJson.Insert(1, "\"children\":" + TreeGridJson(data, entity.id) + ",");
                    sb.Append(strJson);
                }
                sb = sb.Remove(sb.Length - 1, 1);
            }

            sb.Append("]");
            return sb.ToString().Replace("}{", "},{");

        }
        public static List<TreeGridModel> TreeList(this List<TreeGridModel> data)
        {
            return TreeList(data, "0");
        }
        private static List<TreeGridModel> TreeList(List<TreeGridModel> data, string parentId)
        {
            var ChildNodeList = data.FindAll(t => t.parentId == parentId);
            if (ChildNodeList.Count > 0)
            {
                foreach (TreeGridModel entity in ChildNodeList)
                {
                    entity.children = TreeList(data, entity.id);
                }
            }
            return ChildNodeList;
        }
    }
}
