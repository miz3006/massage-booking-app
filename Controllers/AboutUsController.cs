using Microsoft.AspNetCore.Mvc;

namespace MasazeBooking.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
