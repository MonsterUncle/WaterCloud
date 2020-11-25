/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Entity.DingTalkManage;
using WaterCloud.Repository.DingTalkManage;
using System.Collections.Generic;
using System.Linq;

namespace WaterCloud.Application.DingTalkManage
{
    public class DepartmentApp
    {
        private DepartmentRepository service = new DepartmentRepository();

        public List<DepartmentEntity> GetList(string keyword = "")
        {
            var expression = ExtLinq.True<DepartmentEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.Name.Contains(keyword));
            }
            return service.IQueryable(expression).OrderBy(t => t.Name).ToList();
        }
        public DepartmentEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.Id == keyValue);
        }
        public void SubmitForm(DepartmentEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue)==false)
            {
                service.Update(entity);
            }
            else
            {
                service.Insert(entity);
            }
        }
    }
}
