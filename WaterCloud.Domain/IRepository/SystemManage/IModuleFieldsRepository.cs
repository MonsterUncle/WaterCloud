using System.Collections.Generic;
using System.Threading.Tasks;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-05-21 14:38
    /// 描 述：字段管理数据映射接口
    /// </summary>
    public interface IModuleFieldsRepository : IRepositoryBase<ModuleFieldsEntity>
    {
        Task<List<ModuleFieldsEntity>> GetListByRole(string roleid);
        Task<List<ModuleFieldsEntity>> GetListNew(string moduleId);
    }
}
