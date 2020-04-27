using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Entity.SystemSecurity;
using WaterCloud.Service.SystemSecurity;

namespace WaterCloud.Service.TimeService
{

    /// <summary>
    /// 定时任务触发器
    /// </summary>
    public class TimedService : BackgroundService
    {
        /**
         *  
            需要在Startup.cs 里面注册
         *  public void ConfigureServices(IServiceCollection services)
            { 
                services.AddHostedService<TimedService>();
                或者
                services.AddTransient<IHostedService, TimedService>();
            }
         * */

        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        //private Timer _timer;

        public TimedService(ILogger<TimedService> logger,IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment=hostingEnvironment;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("servers start");
            while (!stoppingToken.IsCancellationRequested)
            {
                //执行任务
                ServerStateEntity entity = new ServerStateEntity();
                var computer = ComputerHelper.GetComputerInfo();
                entity.F_ARM = computer.RAMRate;
                entity.F_CPU = computer.CPURate;
                entity.F_IIS = "0";
                entity.F_WebSite = _hostingEnvironment.ContentRootPath;
                new ServerStateService().SubmitForm(entity);
                await Task.Delay(30000, stoppingToken); //延迟暂停30秒
            }
            _logger.LogInformation("servers end");
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}

