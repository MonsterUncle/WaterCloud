using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UEditor.Core;
namespace WaterCloud.Web.Controllers
{
    [Route("api/[controller]")]
    public class UEditorController : Controller
    {
        private UEditorService ue;
        
        public UEditorController(UEditorService ue)
        {
            this.ue = ue;
        }

        [ServiceFilter(typeof(HandlerLoginAttribute))]
        public void Do()
        {
            ue.DoAction(HttpContext);
        }
    }
}
