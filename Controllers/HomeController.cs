using Microsoft.AspNetCore.Mvc;
using MassageStudio.Data;
using Microsoft.EntityFrameworkCore; // (ni nujno, ampak je ok)
using System.Linq;

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
            // št. masaž iz tabele dbo.masaze
            ViewBag.ServicesCount = _context.Masaze.Count();

            // št. uporabnikov iz AspNetUsers
            ViewBag.ClientsCount = _context.Users.Count();

            return View();
        }
    }
}
