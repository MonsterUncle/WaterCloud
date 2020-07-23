using System.Threading.Tasks;

namespace WaterCloud.Service
{
    public interface  ICustomerForm
    {
        Task Add(string flowInstanceId, string frmData);
        Task Edit(string flowInstanceId, string frmData);
    }
}
