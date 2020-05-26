using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Service.CommonService;
using WaterCloud.Service.SystemSecurity;

namespace WaterCloud.Service.AutoJob
{
    public class SaveServerStateJob : IJobTask
    {
        private IWebHostEnvironment _hostingEnvironment = GlobalContext.HostingEnvironment;
        private ServerStateService stateService = new ServerStateService();
        public async Task<AjaxResult> Start()
        {
            AjaxResult obj = new AjaxResult();
            try
            {
                ServerStateEntity entity = new ServerStateEntity();
                var computer = ComputerHelper.GetComputerInfo();
                entity.F_ARM = computer.RAMRate;
                entity.F_CPU = computer.CPURate;
                entity.F_IIS = "0";
                entity.F_WebSite = _hostingEnvironment.ContentRootPath;
                await stateService.SubmitForm(entity);
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
