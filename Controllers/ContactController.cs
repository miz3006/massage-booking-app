using Microsoft.AspNetCore.Mvc;

namespace MasazeBooking.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
