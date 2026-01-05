using MassageStudio.Data;
using MassageStudio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MassageStudio.Controllers
{
    public class MaserController : Controller
    {
        private readonly MassageContext _context;

        public MaserController(MassageContext context)
        {
            _context = context;
        }

        // ==========================
        // JAVNO – OSEBJE
        // ==========================

        // GET: /Maser
        public async Task<IActionResult> Index()
        {
            var maserji = await _context.Maserji.ToListAsync();
            return View(maserji);
        }

        // GET: /Maser/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var maser = await _context.Maserji.FindAsync(id);
            if (maser == null)
                return NotFound();

            return View(maser);
        }
    }
}
