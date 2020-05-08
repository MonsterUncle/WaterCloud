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

namespace WaterCloud.Domain.SystemManage
{
    public interface IQuickModuleRepository : IRepositoryBase<QuickModuleEntity>
    {
        List<QuickModuleExtend> GetQuickModuleList(string userId);
        void SubmitForm(List<QuickModuleEntity> list);
        List<ModuleEntity> GetTransferList(string userId);
    }
}