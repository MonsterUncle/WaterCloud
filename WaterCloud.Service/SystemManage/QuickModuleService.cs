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
using System.Linq;

namespace WaterCloud.Service.SystemManage
{
    public class QuickModuleService: IDenpendency
    {
		private IQuickModuleRepository service;
        /// <summary>
        /// »º´æ²Ù×÷Àà
        /// </summary>

        private string cacheKey = "watercloud_quickmoduledata_";
        public QuickModuleService()
        {
            var currentuser = OperatorProvider.Provider.GetCurrent();
            service = currentuser != null ? new QuickModuleRepository(currentuser.DbString, currentuser.DBProvider) : new QuickModuleRepository();
        }
        public async Task<object> GetTransferList(string userId)
        {
            var data = await service.GetTransferList(userId);
            return data;
        }

        public async Task<List<QuickModuleExtend>> GetQuickModuleList(string userId)
        {
            var data= await service.GetQuickModuleList(userId);
            return data;
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
            var data =await CacheHelper.Get<Dictionary<string, List<QuickModuleExtend>>>(cacheKey + "list");
            if (data != null&&data.ContainsKey(OperatorProvider.Provider.GetCurrent().UserId))
            {
                data.Remove(OperatorProvider.Provider.GetCurrent().UserId);
            }
            await CacheHelper.Remove(cacheKey + "list");
            await CacheHelper.Set(cacheKey + "list",data);
        }

    }
}