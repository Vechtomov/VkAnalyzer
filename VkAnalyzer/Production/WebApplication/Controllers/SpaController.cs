using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class SpaController : Controller
    {
        // GET: Spa
        public IActionResult Index()
        {
            return View("~/Pages/Index.cshtml");
        }
    }
}
