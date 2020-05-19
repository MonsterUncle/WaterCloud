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
using System.Threading.Tasks;

namespace WaterCloud.Service.SystemManage
{
    public class QuickModuleService: IDenpendency
    {
		private IQuickModuleRepository service = new QuickModuleRepository();
        /// <summary>
        /// ���������
        /// </summary>

        private string cacheKey = "watercloud_quickmoduledata_";

        public async Task<object> GetTransferList(string userId)
        {
            return await service.GetTransferList(userId);
        }

        public async Task<List<QuickModuleExtend>> GetQuickModuleList(string userId)
        {
            return await service.GetQuickModuleList(userId);
        }

        public async Task SubmitForm(string[] permissionIds)
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
            await service.SubmitForm(list);
            var data =await RedisHelper.GetAsync<Dictionary<string, List<QuickModuleExtend>>>(cacheKey + "list");
            if (data != null&&data.ContainsKey(OperatorProvider.Provider.GetCurrent().UserId))
            {
                data.Remove(OperatorProvider.Provider.GetCurrent().UserId);
            }
            await RedisHelper.DelAsync(cacheKey + "list");
            await RedisHelper.SetAsync(cacheKey + "list",data);
        }

    }
}