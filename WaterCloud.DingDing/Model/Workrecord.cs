using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterCloud.DingTalk
{
    /// <summary>
    /// 待办事项实体
    /// </summary>
    public class Workrecord
    {
        public string RecordId { get; set; }
        public DateTime CreateTime { get; set; }
        public string Title { get; set; }
        public List<FormsModel> forms { get; set; }
        public class FormsModel
        {
            public string Title { get; set; }
            public string Content { get; set; }
        }
        
    }
    
}
