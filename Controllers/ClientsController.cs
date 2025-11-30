using Microsoft.AspNetCore.Mvc;
using MassageStudio.Data;
using MassageStudio.Models;
using System.Linq;

namespace MasazeBooking.Controllers
{
    public class ClientsController : Controller
    {
        private readonly MassageContext _context;

        public ClientsController(MassageContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var clients = _context.Users.ToList();
            return View(clients);
        }
    }
}
