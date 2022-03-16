/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System.Threading.Tasks;
using WaterCloud.Code;
using SqlSugar;
using WaterCloud.Domain.FlowManage;
using WaterCloud.DataBase;

namespace WaterCloud.Service.FlowManage
{
    public class FormTestService : DataFilterService<FormTestEntity>, IDenpendency,ICustomerForm
    {
        public FormTestService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task Add(string flowInstanceId, string frmData)
        {
            currentuser = OperatorProvider.Provider.GetCurrent();
            var req = frmData.ToObject<FormTestEntity>();
            req.FlowInstanceId = flowInstanceId;
            req.Create();
            req.CreatorUserName = currentuser.UserName;
            await repository.Insert(req);
        }
        public async Task Edit(string flowInstanceId, string frmData)
        {
            currentuser = OperatorProvider.Provider.GetCurrent();
            var req = frmData.ToObject<FormTestEntity>();
            req.FlowInstanceId = flowInstanceId;
            await repository.Update(a => a.FlowInstanceId == req.FlowInstanceId, a => new FormTestEntity
            {
                Attachment = req.Attachment,
                EndTime = req.EndTime,
                StartTime = a.StartTime,
                RequestComment = a.RequestComment,
                RequestType = a.RequestType,
                UserName = a.UserName

            });
        }
    }
}
