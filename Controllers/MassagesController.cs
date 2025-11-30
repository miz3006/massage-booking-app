 using Microsoft.AspNetCore.Mvc;

namespace MasazeBooking.Controllers
{
    public class MassagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
