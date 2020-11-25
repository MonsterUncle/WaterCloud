using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WaterCloud.Application.SystemManage;
using WaterCloud.Entity.SystemManage;

namespace WaterCloud.WebAPI.Controllers
{
    public class TestController : ApiController
    {
        private AreaApp areaApp = new AreaApp();
        /// <summary>
        /// 获取列表方法
        /// </summary>
        /// <returns>区域列表</returns>
        public List<AreaEntity> Get()
        {
            var data = areaApp.GetList();
            return data;
        }

        /// <summary>
        /// 获取单个方法
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns>区域</returns>
        public AreaEntity GetForm(string keyValue)
        {
            var data = areaApp.GetForm(keyValue);
            return data;
        }

        /// <summary>
        /// 提交方法
        /// </summary>
        /// <param name="entity">区域</param>
        public void Post([FromBody]AreaEntity entity)
        {
            areaApp.SubmitForm(entity,"");
        }

        /// <summary>
        /// 编辑方法
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">区域</param>
        public void Put(string keyValue, [FromBody]AreaEntity entity)
        {
            areaApp.SubmitForm(entity, keyValue);
        }

        /// <summary>
        /// 删除方法
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void Delete(string keyValue)
        {
            areaApp.DeleteForm(keyValue);
        }
    }
}
