/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using SqlSugar;
using System.IO;
using WaterCloud.Service.SystemManage;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemOrganize
{
    public class DutyService : DataFilterService<RoleEntity>, IDenpendency
    {
        public SystemSetService setApp { get; set; }
        public DutyService(IUnitOfWork unitOfWork) :base(unitOfWork)
        {
        }

        public async Task<List<RoleEntity>> GetList(string keyword = "")
        {
            var query = repository.IQueryable();
            query = query.Where(a => a.F_Category == 2 && a.F_DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_FullName.Contains(keyword) || a.F_EnCode.Contains(keyword));
            }
            return await query.OrderBy(a => a.F_SortCode).ToListAsync();
        }
        public async Task<List<RoleExtend>> GetLookList(SoulPage<RoleExtend> pagination, string keyword = "")
        {
            //反格式化显示只能用"等于"，其他不支持
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> enabledTemp = new Dictionary<string, string>();
            enabledTemp.Add("1", "有效");
            enabledTemp.Add("0", "无效");
            dic.Add("F_EnabledMark", enabledTemp);
            var setList =await setApp.GetList();
            Dictionary<string, string> orgizeTemp = new Dictionary<string, string>();
            foreach (var item in setList)
            {
                orgizeTemp.Add(item.F_Id, item.F_CompanyName);
            }
            dic.Add("F_OrganizeId", orgizeTemp);
            pagination = ChangeSoulData(dic, pagination);
            var query= GetQuery();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_FullName.Contains(keyword) || a.F_EnCode.Contains(keyword));
            }
            query = query.Where(a => a.F_DeleteMark == false&& a.F_Category == 2);
            query = GetDataPrivilege("a", "", query);
            return await query.ToPageListAsync(pagination);
        }
        public async Task<RoleEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }
        public async Task<RoleEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        private ISugarQueryable<RoleExtend> GetQuery()
        {
            var query = repository.Db.Queryable<RoleEntity,SystemSetEntity>((a,b)=>new JoinQueryInfos(
                JoinType.Left, a.F_OrganizeId == b.F_Id

                )).Where(a => a.F_DeleteMark == false && a.F_Category == 2)
                .Select((a,b)=>new RoleExtend
                {
                    F_Id = a.F_Id.SelectAll(),
                    F_CompanyName = b.F_CompanyName,
                }).MergeTable();
            return query;
        }
        public async Task DeleteForm(string keyValue)
        {
            if (await repository.Db.Queryable<UserEntity>().Where(a => a.F_DutyId == keyValue).AnyAsync())
            {
                throw new Exception("岗位使用中，无法删除");
            }
            await repository.Delete(a => a.F_Id == keyValue);
        }
        public async Task SubmitForm(RoleEntity roleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                roleEntity.Modify(keyValue);
                await repository.Update(roleEntity);
            }
            else
            {
                roleEntity.F_DeleteMark = false;
                roleEntity.F_AllowEdit = false;
                roleEntity.F_AllowDelete = false;
                roleEntity.Create();
                roleEntity.F_Category = 2;
                await repository.Insert(roleEntity);
            }
        }

        public async Task<List<RoleExtend>> CheckFile(string fileFullName)
        {
            if (!FileHelper.IsExcel(fileFullName))
            {
                throw new Exception("文件不是有效的Excel文件!");
            }
            //文件解析
            var list = new ExcelHelper<RoleExtend>().ImportFromExcel(fileFullName);
            //删除文件
            File.Delete(fileFullName);
            foreach (var item in list)
            {
                item.F_Id = Utils.GuId();
                item.F_EnabledMark = true;
                item.F_DeleteMark = false;
                item.F_OrganizeId = currentuser.CompanyId;
                item.F_SortCode = 1;
                item.F_Category = 2;
                item.F_AllowEdit = false;
                item.F_AllowDelete = false;
                List<string> str = new List<string>();
                if (string.IsNullOrEmpty(item.F_EnCode))
                {
                    item.F_EnabledMark = false;
                    item.ErrorMsg = "编号不存在";
                    continue;
                }
                else if (await repository.IQueryable(a => a.F_EnCode == item.F_EnCode).AnyAsync() || list.Where(a => a.F_EnCode == item.F_EnCode).Count() > 1)
                {
                    str.Add("编号重复");
                    item.F_EnabledMark = false;
                }
                if (string.IsNullOrEmpty(item.F_FullName))
                {
                    str.Add("名称不存在");
                    item.F_EnabledMark = false;
                }
                if (item.F_EnabledMark == false)
                {
                    item.ErrorMsg = string.Join(',', str.ToArray());
                }
            }
            return list;
        }

        public async Task ImportForm(List<RoleEntity> filterList)
        {
            foreach (var item in filterList)
            {
                item.Create();
            }
            await repository.Db.Insertable(filterList).ExecuteCommandAsync();
        }
    }
}
