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
using WaterCloud.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Domain.IRepository.SystemManage
{
    public interface IQuickModuleRepository : IRepositoryBase<QuickModuleEntity>
    {
        List<QuickModuleExtend> GetQuickModuleList(string userId);
        void SubmitForm(List<QuickModuleEntity> list);
        List<ModuleEntity> GetTransferList(string userId);
    }
}