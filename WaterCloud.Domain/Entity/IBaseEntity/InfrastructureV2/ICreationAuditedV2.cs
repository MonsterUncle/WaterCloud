﻿/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System;

namespace WaterCloud.Entity
{
    public interface ICreationAuditedV2
    {
        string uuId { get; set; }
        string CreatorId { get; set; }
        DateTime? CreateTime { get; set; }
    }
}