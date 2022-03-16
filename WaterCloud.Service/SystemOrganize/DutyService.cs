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
            query = query.Where(a => a.Category == 2 && a.DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.FullName.Contains(keyword) || a.EnCode.Contains(keyword));
            }
            return await query.OrderBy(a => a.SortCode).ToListAsync();
        }
        public async Task<List<RoleExtend>> GetLookList(SoulPage<RoleExtend> pagination, string keyword = "")
        {
            //反格式化显示只能用"等于"，其他不支持
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> enabledTemp = new Dictionary<string, string>();
            enabledTemp.Add("1", "有效");
            enabledTemp.Add("0", "无效");
            dic.Add("EnabledMark", enabledTemp);
            var setList =await setApp.GetList();
            Dictionary<string, string> orgizeTemp = new Dictionary<string, string>();
            foreach (var item in setList)
            {
                orgizeTemp.Add(item.Id, item.CompanyName);
            }
            dic.Add("OrganizeId", orgizeTemp);
            pagination = ChangeSoulData(dic, pagination);
            var query= GetQuery();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.FullName.Contains(keyword) || a.EnCode.Contains(keyword));
            }
            query = query.Where(a => a.DeleteMark == false&& a.Category == 2);
            query = GetDataPrivilege("a", "", query);
            return await repository.OrderList(query, pagination);
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
                JoinType.Left, a.OrganizeId == b.Id

                )).Where(a => a.DeleteMark == false && a.Category == 2)
                .Select((a,b)=>new RoleExtend
                {
                    Id = a.Id.SelectAll(),
                    CompanyName = b.CompanyName,
                }).MergeTable();
            return query;
        }
        public async Task DeleteForm(string keyValue)
        {
            if (await repository.Db.Queryable<UserEntity>().Where(a => a.DutyId == keyValue).AnyAsync())
            {
                throw new Exception("岗位使用中，无法删除");
            }
            await repository.Delete(a => a.Id == keyValue);
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
                roleEntity.DeleteMark = false;
                roleEntity.AllowEdit = false;
                roleEntity.AllowDelete = false;
                roleEntity.Create();
                roleEntity.Category = 2;
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
                item.Id = Utils.GuId();
                item.EnabledMark = true;
                item.DeleteMark = false;
                item.OrganizeId = currentuser.CompanyId;
                item.SortCode = 1;
                item.Category = 2;
                item.AllowEdit = false;
                item.AllowDelete = false;
                List<string> str = new List<string>();
                if (string.IsNullOrEmpty(item.EnCode))
                {
                    item.EnabledMark = false;
                    item.ErrorMsg = "编号不存在";
                    continue;
                }
                else if (await repository.IQueryable(a => a.EnCode == item.EnCode).AnyAsync() || list.Where(a => a.EnCode == item.EnCode).Count() > 1)
                {
                    str.Add("编号重复");
                    item.EnabledMark = false;
                }
                if (string.IsNullOrEmpty(item.FullName))
                {
                    str.Add("名称不存在");
                    item.EnabledMark = false;
                }
                if (item.EnabledMark == false)
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
