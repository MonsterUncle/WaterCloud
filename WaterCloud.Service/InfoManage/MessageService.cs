using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using SqlSugar;
using WaterCloud.Domain.InfoManage;
using Microsoft.AspNetCore.SignalR;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Service.SystemManage;
using System.Net.Http;
using WaterCloud.Domain.SystemManage;
using System.Text;
using WaterCloud.DataBase;

namespace WaterCloud.Service.InfoManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-07-29 16:41
    /// 描 述：通知管理服务类
    /// </summary>
    public class MessageService : DataFilterService<MessageEntity>, IDenpendency
    {
        public ItemsDataService itemsApp { get; set; }
        public RabbitMqHelper rabbitMqHelper { get; set; }
        public MessageService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #region 获取数据
        public async Task<List<MessageEntity>> GetList(string keyword = "")
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.MessageInfo.Contains(keyword) || a.CreatorUserName.Contains(keyword));
            }
            return await query.Where(a => a.EnabledMark == true).OrderBy(a => a.CreatorTime,OrderByType.Desc).ToListAsync();
        }

        public async Task<List<MessageEntity>> GetLookList(string keyword = "")
        {
            var query = repository.IQueryable().Where(a => a.EnabledMark == true);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.MessageInfo.Contains(keyword) || a.CreatorUserName.Contains(keyword));
            }
            query = GetDataPrivilege("a","", query);
            return await query.OrderBy(a => a.CreatorTime,OrderByType.Desc).ToListAsync();
        }

        public async Task<List<MessageEntity>> GetUnReadListJson()
        {
            var hisquery = repository.Db.Queryable<MessageHistoryEntity>().Where(a => a.CreatorUserId == currentuser.UserId).Select(a => a.MessageId).ToList();
            var tempList= repository.Db.Queryable<MessageEntity, MessageHistoryEntity>((a,b) => new JoinQueryInfos(
                JoinType.Inner,a.Id==b.MessageId&&a.MessageType==2
                )).Select(a => a.Id).ToList();
            hisquery.AddRange(tempList);
            var query = repository.IQueryable(a => (a.ToUserId.Contains(currentuser.UserId) || a.ToUserId == "") && a.EnabledMark == true).Where(a => !hisquery.Contains(a.Id));
            return await GetFieldsFilterDataNew("a", query.OrderBy(a => a.CreatorTime,OrderByType.Desc)).ToListAsync();
        }

        public async Task<List<MessageEntity>> GetLookList(SoulPage<MessageEntity> pagination, string keyword = "")
        {
            //反格式化显示只能用"等于"，其他不支持
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            var setList = await itemsApp.GetItemList("MessageType");
            Dictionary<string, string> messageTypeTemp = new Dictionary<string, string>();
            foreach (var item in setList)
            {
                messageTypeTemp.Add(item.ItemCode,item.ItemName);
            }
            dic.Add("MessageType", messageTypeTemp);
            pagination = ChangeSoulData(dic, pagination);
            //获取数据权限
            var query = repository.IQueryable().Where(a => a.EnabledMark == true);
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(a => a.MessageInfo.Contains(keyword) || a.CreatorUserName.Contains(keyword));
            }
            query = GetDataPrivilege("a","",query);
            return await query.ToPageListAsync(pagination);
        }

        public async Task<MessageEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        #endregion

        public async Task<MessageEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }

        #region 提交数据
        public async Task SubmitForm(MessageEntity entity)
        {
            entity.Create();
            entity.EnabledMark = true;
            entity.CreatorUserName = currentuser.UserName;
            MessageEntity messageEntity = new MessageEntity();
            if (string.IsNullOrEmpty(entity.ToUserId))
            {
                string msg = entity.ToJson();
                entity.ToUserName = "所有人";
                entity.ToUserId = "";
                messageEntity = await repository.Insert(entity);
            }
            else
            {
                var users = entity.ToUserId.Split(",");
                entity.ToUserName = string.Join(",", repository.Db.Queryable<UserEntity>().Where(a => users.Contains(a.Id)).Select(a => a.RealName).ToList());
                messageEntity= await repository.Insert(entity);
            }
            //通过http发送消息
            messageEntity.companyId = currentuser.CompanyId;
            rabbitMqHelper.Publish(messageEntity);
        }
        public async Task ReadAllMsgForm(int type)
        {
            var unList=await GetUnReadListJson();
            var strList = unList.Where(a => a.MessageType == type&&a.ClickRead==true).Select(a=>a.Id).ToList();
            unitofwork.CurrentBeginTrans();
            foreach (var item in strList)
            {
               await ReadMsgForm(item);
            }
            unitofwork.CurrentCommit();
        }

        public async Task ReadMsgForm(string keyValue)
        {            
            MessageHistoryEntity msghis = new MessageHistoryEntity();
            msghis.Create();
            msghis.CreatorUserName = currentuser.UserName;
            msghis.MessageId = keyValue;
            await repository.Db.Insertable(msghis).ExecuteCommandAsync();
        }

        public async Task<bool> CheckMsg(string keyValue)
        {
            var msg = await repository.FindEntity(keyValue);
            if (msg==null)
            {
                return true;
            }
            if (msg.ClickRead==false)
            {
                return true;
            }
            if (await repository.Db.Queryable<MessageHistoryEntity>().Where(a => a.MessageId == keyValue && a.CreatorUserId == currentuser.UserId).AnyAsync())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task DeleteForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            await repository.Update(a => ids.Contains(a.Id), a=>new MessageEntity { 
                EnabledMark=false         
            });
        }
        #endregion

    }
}
