using Microsoft.AspNetCore.Hosting;
using SqlSugar;
using System;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Service.SystemSecurity;

namespace WaterCloud.Service.AutoJob
{
    [ServiceDescription("服务器监控")]
	public class SaveServerStateJob : IJobTask
    {
        private IWebHostEnvironment _hostingEnvironment;
        private ServerStateService _server;
        public SaveServerStateJob(IUnitOfWork unitOfWork)
        {
            _hostingEnvironment = GlobalContext.HostingEnvironment;
            _server = new ServerStateService(unitOfWork);
         }
        public async Task<AlwaysResult> Start()
        {
            AlwaysResult obj = new AlwaysResult();
            try
            {
                ServerStateEntity entity = new ServerStateEntity();
                var computer = ComputerHelper.GetComputerInfo();
                entity.ARM = computer.RAMRate;
                entity.CPU = computer.CPURate;
                entity.IIS = "0";
                entity.WebSite = _hostingEnvironment.ContentRootPath;
                await _server.SubmitForm(entity);
                obj.state = ResultType.success.ToString();
                obj.message = "服务器状态更新成功！";
            }
            catch (Exception ex)
            {
                obj.state = ResultType.error.ToString();
                obj.message = "服务器状态更新失败！"+ex.Message;
            }
            return obj;
        }
    }
}
