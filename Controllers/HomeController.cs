using Microsoft.AspNetCore.Mvc;
using MassageStudio.Data;

namespace MasazeBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly MassageContext _context;

        public HomeController(MassageContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.ServicesCount = _context.Services.Count();
            ViewBag.ClientsCount  = _context.Users.Count();
            return View();
        }
    }
}
