using Microsoft.AspNetCore.Mvc;

namespace MasazeBooking.Controllers
{
    public class StuffController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
