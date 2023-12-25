using SqlSugar;

namespace WaterCloud.Domain.SystemOrganize
{
	/// <summary>
	/// 创 建：超级管理员
	/// 日 期：2023-12-25 08:54
	/// 描 述：组织实体扩展类
	/// </summary>
	[SugarTable("sys_organize")]
	public class OrganizeExtend : OrganizeEntity
    {
		//使用导入错误信息
		public string ErrorMsg { get; set; }
		public string F_ManagerName { get; set; }
    }
}