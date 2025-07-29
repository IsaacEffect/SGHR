using Microsoft.AspNetCore.Mvc;

namespace SGHR.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
