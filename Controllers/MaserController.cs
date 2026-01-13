using MassageStudio.Data;
using MassageStudio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MassageStudio.Controllers 
{
    public class MaserController : Controller
    {
        private readonly MassageContext _context;

        public MaserController(MassageContext context)
        {
            _context = context;
        }

        // GET: /Maser
        public async Task<IActionResult> Index()
        {
            var maserji = await _context.Maserji.ToListAsync();
            
            // Vrne View: Views/Maser/Index.cshtml
            return View(maserji);
        }
    }
}