﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;
using Chloe;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-05-21 14:38
    /// 描 述：字段管理服务类
    /// </summary>
    public class ModuleFieldsService : DataFilterService<ModuleFieldsEntity>, IDenpendency
    {
        //获取类名

        public ModuleFieldsService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #region 获取数据
        public async Task<List<ModuleFieldsEntity>> GetList(string keyword = "")
        {
            var data =  repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                data = data.Where(t => t.F_FullName.Contains(keyword) || t.F_EnCode.Contains(keyword));
            }
            return await data.Where(a => a.F_DeleteMark == false).OrderByDesc(t => t.F_CreatorTime).ToListAsync();
        }

        public async Task<List<ModuleFieldsEntity>> GetLookList(Pagination pagination, string moduleId, string keyword = "")
        {
            //获取数据权限
            var query = repository.IQueryable().Where(u => u.F_DeleteMark == false && u.F_ModuleId == moduleId);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u => u.F_FullName.Contains(keyword) || u.F_EnCode.Contains(keyword));
            }
            query = GetDataPrivilege("u","", query);
            return await repository.OrderList(query, pagination);

        }

        public async Task<ModuleFieldsEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }
        public async Task<ModuleFieldsEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        #endregion

        #region 提交数据
        public async Task SubmitForm(ModuleFieldsEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.F_DeleteMark = false;
                entity.Create();
                await repository.Insert(entity);
            }
            else
            {
                entity.Modify(keyValue);
                await repository.Update(entity);
            }
        }

        public async Task DeleteForm(string keyValue)
        {
            await repository.Delete(t => t.F_Id == keyValue);
        }

        public async Task SubmitCloneFields(string moduleId, string ids)
        {
            string[] ArrayId = ids.Split(',');
            var data = await this.GetList();
            List<ModuleFieldsEntity> entitys = new List<ModuleFieldsEntity>();
            var module = await unitwork.FindEntity<ModuleEntity>(a => a.F_Id == moduleId);
            if (string.IsNullOrEmpty(module.F_UrlAddress) || module.F_Target != "iframe")
            {
                throw new Exception("框架页才能创建按钮");
            }
            foreach (string item in ArrayId)
            {
                ModuleFieldsEntity moduleFieldsEntity = data.Find(t => t.F_Id == item);
                moduleFieldsEntity.Create();
                moduleFieldsEntity.F_ModuleId = moduleId;
                entitys.Add(moduleFieldsEntity);
            }
            await repository.Insert(entitys);
        }

        public async Task<List<ModuleFieldsEntity>> GetListByRole(string roleid)
        {
            var moduleList = unitwork.IQueryable<RoleAuthorizeEntity>(a => a.F_ObjectId == roleid && a.F_ItemType == 3).Select(a => a.F_ItemId).ToList();
            var query = repository.IQueryable().Where(a => (moduleList.Contains(a.F_Id) || a.F_IsPublic == true) && a.F_DeleteMark == false && a.F_EnabledMark == true);
            return await query.OrderByDesc(a => a.F_CreatorTime).ToListAsync();
        }

        internal async Task<List<ModuleFieldsEntity>> GetListNew(string moduleId = "")
        {
            var query = repository.IQueryable(a => a.F_EnabledMark == true)
            .InnerJoin<ModuleEntity>((a, b) => a.F_ModuleId == b.F_Id && b.F_EnabledMark == true)
            .Select((a, b) => new ModuleFieldsEntity
            {
                F_Id = a.F_Id,
                F_CreatorTime = a.F_CreatorTime,
                F_CreatorUserId = a.F_CreatorUserId,
                F_DeleteMark = a.F_DeleteMark,
                F_DeleteTime = a.F_DeleteTime,
                F_DeleteUserId = a.F_DeleteUserId,
                F_Description = a.F_Description,
                F_EnabledMark = a.F_EnabledMark,
                F_EnCode = a.F_EnCode,
                F_FullName = a.F_FullName,
                F_LastModifyTime = a.F_LastModifyTime,
                F_LastModifyUserId = a.F_LastModifyUserId,
                F_ModuleId = b.F_UrlAddress,
                F_IsPublic = a.F_IsPublic
            });
            if (!string.IsNullOrEmpty(moduleId))
            {
                query = query.Where(a => a.F_ModuleId == moduleId);
            }
            return await query.OrderByDesc(a => a.F_CreatorTime).ToListAsync();
        }
        #endregion

    }
}
