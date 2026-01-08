using MassageStudio.Data;
using MassageStudio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MassageStudio.Controllers // Preveri, če je namespace pravi
{
    public class MaserController : Controller
    {
        private readonly MassageContext _context;

        public MaserController(MassageContext context)
        {
            _context = context;
        }

        // GET: /Maser
        // Ta metoda manjka, zato dobiš 404 napako!
        public async Task<IActionResult> Index()
        {
            // Pridobi vse maserje iz baze
            var maserji = await _context.Maserji.ToListAsync();
            
            // Vrne View: Views/Maser/Index.cshtml
            return View(maserji);
        }
    }
}