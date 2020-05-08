//-----------------------------------------------------------------------
// <copyright file=" QuickModule.cs" company="JR">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: QuickModule.cs
// * history : Created by T4 04/13/2020 16:51:13 
// </copyright>
//-----------------------------------------------------------------------
using WaterCloud.Domain.SystemManage;
using WaterCloud.Repository.SystemManage;
using System.Collections.Generic;
using WaterCloud.Code;
namespace WaterCloud.Service.SystemManage
{
    public class QuickModuleService: IDenpendency
    {
		private IQuickModuleRepository service = new QuickModuleRepository();
        /// <summary>
        /// »º´æ²Ù×÷Àà
        /// </summary>

        private string cacheKey = "watercloud_quickmoduledata_";

        public object GetTransferList(string userId)
        {
            return service.GetTransferList(userId);
        }

        public List<QuickModuleExtend> GetQuickModuleList(string userId)
        {
            return service.GetQuickModuleList(userId);
        }

        public void SubmitForm(string[] permissionIds)
        {
            List<QuickModuleEntity> list = new List<QuickModuleEntity>();
            foreach (var itemId in permissionIds)
            {
                QuickModuleEntity entity = new QuickModuleEntity();
                entity.Create();
                entity.F_ModuleId = itemId;
                entity.F_EnabledMark = true;
                entity.F_DeleteMark = false;
                list.Add(entity);
            }
            service.SubmitForm(list);
            var data = RedisHelper.Get<Dictionary<string, List<QuickModuleExtend>>>(cacheKey + "list");
            if (data != null&&data.ContainsKey(OperatorProvider.Provider.GetCurrent().UserId))
            {
                data.Remove(OperatorProvider.Provider.GetCurrent().UserId);
            }
            RedisHelper.Del(cacheKey + "list");
            RedisHelper.Set(cacheKey + "list",data);
        }

    }
}