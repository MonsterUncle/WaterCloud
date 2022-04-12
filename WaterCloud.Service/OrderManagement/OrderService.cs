using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using SqlSugar;
using WaterCloud.DataBase;
using WaterCloud.Domain.OrderManagement;

namespace WaterCloud.Service.OrderManagement
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-07-12 20:41
    /// 描 述：订单管理服务类
    /// </summary>
    public class OrderService : DataFilterService<OrderEntity>, IDenpendency
    {
        public OrderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #region 获取数据
        public async Task<List<OrderEntity>> GetList(string keyword = "")
        {
            var data = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.Where(a => a.F_OrderCode.Contains(keyword)
                || a.F_Description.Contains(keyword));
            }
            return await data.Where(a => a.F_DeleteMark == false).OrderBy(a => a.F_NeedTime).ToListAsync();
        }

        public async Task<List<OrderEntity>> GetLookList(string keyword = "")
        {
            var query = repository.IQueryable().Where(a => a.F_DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(a => a.F_OrderCode.Contains(keyword)
                || a.F_Description.Contains(keyword));
            }
            //权限过滤
            query = GetDataPrivilege("a", "", query);
            return await query.OrderBy(a => a.F_NeedTime).ToListAsync();
        }

        public async Task<List<OrderEntity>> GetLookList(SoulPage<OrderEntity> pagination, string keyword = "", string id = "")
        {
            var query = IQuery().Where(a => a.F_DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_OrderCode.Contains(keyword)
                || a.F_Description.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(id))
            {
                query = query.Where(a => a.F_Id == id);
            }
            //权限过滤
            query = GetDataPrivilege("a", "", query);
            return await query.ToPageListAsync(pagination);
        }

        private ISugarQueryable<OrderEntity> IQuery()
        {
            var details = repository.Db.Queryable<OrderDetailEntity>().GroupBy(a => a.F_OrderId).Select<Object>(a => new {
                a.F_OrderId,
                F_NeedNum = SqlFunc.AggregateSum(a.F_NeedNum),
                F_ActualNum = SqlFunc.AggregateSum(a.F_ActualNum),
            });
            var order = repository.Db.Queryable<OrderEntity>();
            var query = repository.Db.Queryable(order, details, JoinType.Inner, (a, b) => a.F_Id == SqlFunc.MappingColumn(default(string), "b.F_OrderId")).Select((a, b) => new OrderEntity {
                F_Id = a.F_Id,
                F_ActualTime = a.F_ActualTime,
                F_CreatorTime = a.F_CreatorTime,
                F_CreatorUserId = a.F_CreatorUserId,
                F_CreatorUserName = a.F_CreatorUserName,
                F_DeleteMark = a.F_DeleteMark,
                F_DeleteTime = a.F_DeleteTime,
                F_DeleteUserId = a.F_DeleteUserId,
                F_Description = a.F_Description,
                F_EnabledMark = a.F_EnabledMark,
                F_LastModifyTime = a.F_LastModifyTime,
                F_LastModifyUserId = a.F_LastModifyUserId,
                F_NeedTime = a.F_NeedTime,
                F_OrderCode = a.F_OrderCode,
                F_OrderState = a.F_OrderState,
                F_NeedNum = SqlFunc.MappingColumn(default(int), "b.F_NeedNum"),
                F_ActualNum = SqlFunc.MappingColumn(default(int), "b.F_ActualNum"),
            }).MergeTable();
            return query;
        }

        public async Task<OrderEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            data.list = await repository.Db.Queryable<OrderDetailEntity>().Where(a => a.F_OrderId == keyValue).ToListAsync();
            return data;
        }
        #endregion

        public async Task<OrderEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            data.list = await repository.Db.Queryable<OrderDetailEntity>().Where(a => a.F_OrderId == keyValue).ToListAsync();
            return GetFieldsFilterData(data);
        }

        #region 提交数据
        public async Task SubmitForm(OrderEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.Create();
                entity.F_DeleteMark = false;
                entity.F_CreatorUserName = currentuser.UserName;
            }
            else
            {
                entity.Modify(keyValue);
            }
            bool isdone = true;
            var dataList = new List<OrderDetailEntity>();
            if (entity.list == null || entity.list.Count == 0)
            {
                throw new Exception("产品明细不能为空!");
            }
            foreach (var item in entity.list)
            {
                item.Create();
                item.F_CreatorUserName = currentuser.UserName;
                item.F_CreatorTime = entity.F_CreatorTime;
                item.F_NeedTime = entity.F_NeedTime;
                item.F_OrderId = entity.F_Id;
                item.F_OrderState = item.F_OrderState == null ? 0 : item.F_OrderState;
                if (item.F_OrderState == 0)
                {
                    isdone = false;
                }
                else if (item.F_ActualTime == null)
                {
                    item.F_ActualTime = entity.F_NeedTime;
                }
                if (string.IsNullOrEmpty(item.F_ProductName))
                {
                    throw new Exception("请输入明细的产品名称");
                }
                dataList.Add(item);
            }
            entity.F_OrderState = isdone ? 1 : 0;
            entity.F_ActualTime = isdone ?dataList.Max(a=>a.F_ActualTime):null;
            unitOfWork.CurrentBeginTrans();
            if (string.IsNullOrEmpty(keyValue))
            {
                await repository.Insert(entity);
            }
            else
            {
                await repository.Update(entity);
            }
            await repository.Db.Deleteable<OrderDetailEntity>().Where(a => a.F_OrderId == entity.F_Id).ExecuteCommandAsync();
            await repository.Db.Insertable(dataList).ExecuteCommandAsync();
            unitOfWork.CurrentCommit();
        }

        public async Task DeleteForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            await repository.Update(a => ids.Contains(a.F_Id),a=>new OrderEntity { 
                F_DeleteMark=true,
                F_DeleteTime=DateTime.Now,
                F_DeleteUserId=currentuser.UserId
            });
        }
        #endregion

    }
}
