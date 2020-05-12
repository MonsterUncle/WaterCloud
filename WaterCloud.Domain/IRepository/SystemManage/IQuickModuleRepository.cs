//-----------------------------------------------------------------------
// <copyright file=" QuickModule.cs" company="WaterCloud">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: QuickModule.cs
// * history : Created by T4 04/13/2020 16:51:14 
// </copyright>
//-----------------------------------------------------------------------
using WaterCloud.DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WaterCloud.Domain.SystemManage
{
    public interface IQuickModuleRepository : IRepositoryBase<QuickModuleEntity>
    {
        Task<List<QuickModuleExtend>> GetQuickModuleList(string userId);
        Task SubmitForm(List<QuickModuleEntity> list);
        Task<List<ModuleEntity>> GetTransferList(string userId);
    }
}