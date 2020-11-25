using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;

namespace WaterCloud.SystemTask
{
    public  class PublicSystemTask
    {
        /// <summary>
        /// 启动定时任务
        /// </summary>
        public void StartUp()
        {
            // 初始化待处理的作业
            JobManager.Initialize(new ApiJobFactory());
        }
        
        /// <summary>
        /// 停止定时任务
        /// </summary>
        public static void Stop()
        {
            JobManager.Stop();
        }

        /// <summary>
        /// 待处理的作业工厂，在构造函数中设置好各个Job的执行计划。
        /// 参考【https://github.com/fluentscheduler/FluentScheduler】
        /// </summary>
        public class ApiJobFactory : Registry
        {
            public ApiJobFactory()
            {
                // 立即执行每分钟一次的计划任务。（指定一个时间间隔运行，根据自己需求，可以是秒、分、时、天、月、年等。）
                //Schedule<DingTalkRemindMsgTask>().ToRunNow().AndEvery(1).Minutes();

                //// 延迟一个指定时间间隔执行一次计划任务。（当然，这个间隔依然可以是秒、分、时、天、月、年等。）
                //Schedule<Demo>().ToRunOnceIn(5).Seconds();

                //// 在一个指定时间执行计划任务（最常用。这里是在每天的下午 1:10 分执行）
                //Schedule(() => Trace.WriteLine("It's 1:10 PM now.")).ToRunEvery(1).Days().At(13, 10);

                //// 立即执行一个在每月的星期一 3:00 的计划任务（可以看出来这个一个比较复杂点的时间，它意思是它也能做到！）
                //Schedule<Demo>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);

                //// 在同一个计划中执行两个（多个）任务
                //Schedule<Demo>().AndThen<Demo>().ToRunNow().AndEvery(5).Minutes();

                // 实际任务
                //Schedule<EveryDayRemindTask>().ToRunNow().AndEvery(1).Minutes(); // 每分钟执行一次
                //Schedule<EveryDayRemindTask>().ToRunNow().AndEvery(1).Days().At(0, 0); // 每天固定时间执行一次
                //Schedule<EveryDayRemindTask>().ToRunNow().AndEvery(1).Weeks().On(DayOfWeek.Monday).At(9, 0);// 每周周一9点执行一次
                Schedule<EveryMinRemindTask>().ToRunNow().AndEvery(5).Minutes(); // 每5分钟执行一次

            }
        }
    }
}
