//-----------------------------------------------------------------------
// <copyright file=" Notice.cs" company="WaterCloud">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: Notice.cs
// * history : Created by T4 04/13/2020 16:51:21 
// </copyright>
//-----------------------------------------------------------------------
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemManage
{
    public interface INoticeRepository : IRepositoryBase<NoticeEntity>
    {
		void DeleteForm(string keyValue);
    }
}