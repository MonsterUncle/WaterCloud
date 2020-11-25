/*******************************************************************************
 * Copyright © 2019 JR.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud开发平台

*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Code
{
    public class CacheFactory
    {
        public static ICache CaChe()
        {
            return new CacheByRedis();
        }
    }
}
