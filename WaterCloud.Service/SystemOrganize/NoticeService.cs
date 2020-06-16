//-----------------------------------------------------------------------
// <copyright file=" Notice.cs" company="JR">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: Notice.cs
// * history : Created by T4 04/13/2020 16:51:21 
// </copyright>
//-----------------------------------------------------------------------
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Repository.SystemOrganize;
using System.Collections.Generic;
using System.Linq;
using WaterCloud.Code;
using System.Threading.Tasks;

namespace WaterCloud.Service.SystemOrganize
{
    public class NoticeService: DataFilterService<NoticeEntity>,IDenpendency
    {
		private INoticeRepository service;
        //��ȡ����
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        /// <summary>
        /// ���������
        /// </summary>

        private string cacheKey = "watercloud_noticedata_";
        public NoticeService()
        {
            var currentuser = OperatorProvider.Provider.GetCurrent();
            service = currentuser != null ? new NoticeRepository(currentuser.DbString, currentuser.DBProvider) : new NoticeRepository();
        }
        public async Task<List<NoticeEntity>> GetList(string keyword)
        {
            List<NoticeEntity> list = new List<NoticeEntity>();
            list = await service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(t => t.F_Title.Contains(keyword) || t.F_Content.Contains(keyword)).ToList();
            }
            //�������ﲻ����
            //����Ȩ�޺ͻ����ȡ��ͻ
            //if (!CheckDataPrivilege("u", className.Substring(0, className.Length - 7)))
            //{
            //    list = await service.CheckCacheList(cacheKey + "list");
            //}
            //else
            //{
            //    var forms = GetDataPrivilege(className.Substring(0, className.Length - 7));
            //    list = list.ToList();
            //}
            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    list = list.Where(t => t.F_Title.Contains(keyword) || t.F_Content.Contains(keyword)).ToList();
            //}
            return list.Where(a => a.F_DeleteMark == false).ToList();
        }
        public async Task<List<NoticeEntity>> GetLookList(Pagination pagination, string keyword = "")
        {
            //��ȡ����Ȩ��
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(u => u.F_Title.Contains(keyword) || u.F_Content.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark==false);
            return GetFieldsFilterData(await service.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }
        public async Task<NoticeEntity> GetLookForm(string keyValue)
        {
            var cachedata =await service.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata,className.Substring(0, className.Length - 7));
        }
        public async Task<NoticeEntity> GetForm(string keyValue)
        {
            var cachedata = await service.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        public async Task SubmitForm(NoticeEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                await service.Update(entity);
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                entity.Create();
                await service.Insert(entity);
                await CacheHelper.Remove(cacheKey + "list");
            }
        }

		public async Task DeleteForm(string keyValue)
        {
            await service.DeleteForm(keyValue);
            await CacheHelper.Remove(cacheKey + keyValue);
            await CacheHelper.Remove(cacheKey + "list");
        }

    }
}