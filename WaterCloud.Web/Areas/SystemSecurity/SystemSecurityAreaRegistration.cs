using System.Web.Mvc;

namespace WaterCloud.Web.Areas.SystemSecurity
{
    public class SystemSecurityAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SystemSecurity";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                 this.AreaName + "_Default",
                 this.AreaName + "/{controller}/{action}/{id}",
                 new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
                 new string[] { "WaterCloud.Web.Areas." + this.AreaName + ".Controllers" }
           );
        }
    }
}
