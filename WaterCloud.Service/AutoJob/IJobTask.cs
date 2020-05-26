using System.Threading.Tasks;
using WaterCloud.Code;

namespace WaterCloud.Service.AutoJob
{
    public interface IJobTask
    {
        Task<AjaxResult> Start();
    }
}
