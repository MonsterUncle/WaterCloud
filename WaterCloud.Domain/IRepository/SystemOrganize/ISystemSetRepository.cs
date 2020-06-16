using System.Threading.Tasks;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-12 13:50
    /// 描 述：系统设置数据映射接口
    /// </summary>
    public interface ISystemSetRepository : IRepositoryBase<SystemSetEntity>
    {
        Task UpdateForm(SystemSetEntity entity);
    }
}
