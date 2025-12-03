using MassageStudio.Data;
using MassageStudio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MasazeBooking.Controllers
{
    public class MassagesController : Controller
    {
        private readonly MassageContext _context;

        public MassagesController(MassageContext context)
        {
            _context = context;
        }

        // GET: /Massages
        public async Task<IActionResult> Index()
        {
            var masaze = await _context.Masaze.ToListAsync();
            return View(masaze);
        }

        // GET: /Massages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Massages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Masaza masaza)
        {
            if (!ModelState.IsValid)
            {
                return View(masaza);
            }

            
            int nextId = _context.Masaze.Any()
                ? _context.Masaze.Max(m => m.IdMasaze) + 1
                : 1;

            masaza.IdMasaze = nextId;

            _context.Masaze.Add(masaza);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
