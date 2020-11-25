/*******************************************************************************
 * Copyright © 2016 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/

namespace WaterCloud.Code
{
    public class ComboSelectModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public string value { get; set; }
        public string parentid { get; set; }
        public object children { get; set; }
    }
}
