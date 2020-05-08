/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.Repository.SystemManage
{
    public class ItemsRepository : RepositoryBase<ItemsEntity>, IItemsRepository
    {
        private string ConnectStr;
        private string providerName;
        public ItemsRepository()
        {
        }
        public ItemsRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            this.ConnectStr = ConnectStr;
            this.providerName = providerName;
        }
    }
}
