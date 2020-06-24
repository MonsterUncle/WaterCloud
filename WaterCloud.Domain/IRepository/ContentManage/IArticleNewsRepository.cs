using System.Collections.Generic;
using System.Threading.Tasks;
using WaterCloud.DataBase;

namespace WaterCloud.Domain.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-09 19:42
    /// 描 述：新闻管理数据映射接口
    /// </summary>
    public interface IArticleNewsRepository : IRepositoryBase<ArticleNewsEntity>
    {
        /// <summary>
        /// 查询新闻列表
        /// </summary>
        /// <param name="keyword">搜索条件</param>
        /// <param name="CategoryId">新闻类别</param>
        /// <returns></returns>
        Chloe.IQuery<ArticleNewsEntity> GetList(string keyword, string CategoryId);

        /// <summary>
        /// 查询新闻详情
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        Chloe.IQuery<ArticleNewsEntity> GetForm(string keyValue);
    }
}
