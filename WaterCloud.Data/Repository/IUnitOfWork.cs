/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using SqlSugar;

namespace WaterCloud.DataBase
{
	public interface IUnitOfWork
	{
		SqlSugarClient GetDbClient();
		void BeginTrans();

		void Commit();
		void Rollback();
	}
}