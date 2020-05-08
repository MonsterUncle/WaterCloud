//-----------------------------------------------------------------------
// <copyright file=" ServerState.cs" company="WaterCloud">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: ServerState.cs
// * history : Created by T4 04/13/2020 11:54:49 
// </copyright>
//-----------------------------------------------------------------------
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemSecurity
{
    public interface IServerStateRepository : IRepositoryBase<ServerStateEntity>
    {
		void DeleteForm(string keyValue);
    }
}