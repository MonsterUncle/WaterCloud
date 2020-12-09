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
        [IgnoreAntiforgeryToken]
        public void Do()
        {
            ue.DoAction(HttpContext);
        }
    }
}
