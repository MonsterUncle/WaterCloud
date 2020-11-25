using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCloud.Entity
{
    public class InitEntity
    {
        public HomeInfoEntity homeInfo { get; set; }
        public LogoInfoEntity logoInfo { get; set; }
        public List<MenuInfoEntity> menuInfo { get; set; }
    }
}