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
using Chloe;
using System.IO;

namespace WaterCloud.Service.SystemOrganize
{
    public class DutyService : DataFilterService<RoleEntity>, IDenpendency
    {
        public DutyService(IDbContext context) :base(context)
        {
        }
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private string cacheKey = "watercloud_dutydata_";// 岗位
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];

        public async Task<List<RoleEntity>> GetList(string keyword = "")
        {
            var cachedata =await repository.CheckCacheList(cacheKey + "list");
            cachedata = cachedata.Where(t => t.F_Category == 2&&t.F_DeleteMark==false).ToList();
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_FullName.Contains(keyword) || t.F_EnCode.Contains(keyword)).ToList();
            }
            return cachedata.OrderBy(t => t.F_SortCode).ToList();
        }
        public async Task<List<RoleEntity>> GetLookList(SoulPage<RoleEntity> pagination, string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(u => u.F_FullName.Contains(keyword) || u.F_EnCode.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark == false&& u.F_Category == 2);
            return GetFieldsFilterData(await repository.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }
        public async Task<RoleEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata, className.Substring(0, className.Length - 7));
        }
        public async Task<RoleEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        public async Task DeleteForm(string keyValue)
        {
            if (uniwork.IQueryable<UserEntity>(a => a.F_DutyId == keyValue).Count() > 0)
            {
                throw new Exception("岗位使用中，无法删除");
            }
            await repository.Delete(t => t.F_Id == keyValue);
            await CacheHelper.Remove(cacheKey + keyValue);
            await CacheHelper.Remove(cacheKey + "list");
        }
        public async Task SubmitForm(RoleEntity roleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                roleEntity.Modify(keyValue);
                await repository.Update(roleEntity);
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                roleEntity.Create();
                roleEntity.F_Category = 2;
                await repository.Insert(roleEntity);
                await CacheHelper.Remove(cacheKey + "list");
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
                else if (repository.IQueryable(a => a.F_EnCode == item.F_EnCode).Count() > 0 || list.Where(a => a.F_EnCode == item.F_EnCode).Count() > 2)
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
            await uniwork.Insert(filterList);
        }
    }
}
