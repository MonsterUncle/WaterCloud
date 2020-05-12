/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WaterCloud.Domain.SystemManage
{
    public interface IItemsDetailRepository : IRepositoryBase<ItemsDetailEntity>
    {
        Task<List<ItemsDetailEntity>> GetItemList(string enCode);
    }
}
