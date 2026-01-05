using MassageStudio.Data;
using MassageStudio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MassageStudio.Controllers
{
    public class ContactController : Controller
    {
        private readonly MassageContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ContactController(MassageContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(string naslov, string vsebina)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(naslov) || string.IsNullOrWhiteSpace(vsebina))
            {
                ModelState.AddModelError("", "Naslov in vsebina ne smeta biti prazna.");
                return View("Index");
            }
            var maxId = await _context.Obvestila.MaxAsync(o => (int?)o.IdObvestila) ?? 0;
            var obvestilo = new Obvestilo
            {
                IdObvestila = maxId + 1,
                Naslov = naslov,
                Vsebina = vsebina,
                DatumObjave = DateTime.Now,
                UserId = user.Id
            };

            _context.Obvestila.Add(obvestilo);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Sporočilo je bilo poslano adminu.";
            return RedirectToAction("Index");
        }
    }
}
