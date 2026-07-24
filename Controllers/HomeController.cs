using Microsoft.AspNetCore.Mvc;

namespace CartaDeclaratoriaApp.Controllers
{
    public class HomeController : Controller
    {
        [Route("/Home/Error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
