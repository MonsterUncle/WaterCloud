using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.Repository.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-05-21 14:38
    /// 描 述：字段管理数据实现类
    /// </summary>
    public class ModuleFieldsRepository : RepositoryBase<ModuleFieldsEntity>,IModuleFieldsRepository
    {
        private string ConnectStr;
        private string providerName;
        public ModuleFieldsRepository()
        {
        }
        public ModuleFieldsRepository(string ConnectStr, string providerName)
             : base(ConnectStr, providerName)
        {
             this.ConnectStr = ConnectStr;
             this.providerName = providerName;
        }
    }
}
