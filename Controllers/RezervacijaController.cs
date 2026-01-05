using MassageStudio.Data;
using MassageStudio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace MassageStudio.Controllers
{
    [Authorize]
    public class RezervacijaController : Controller
    {
        private readonly MassageContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RezervacijaController(
            MassageContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // API za FullCalendar
        [HttpGet]
        public async Task<IActionResult> GetEvents(int idMasaze, int? maserID)
        {
            var events = await _context.Termini
                .Where(t => t.Zaseden == false && t.MaserID == maserID.Value)
                .Select(t => new
                {
                    id = t.IdTermin,
                    title = "Prosto",
                    start = t.Datum.Add(t.Cas_prihoda),
                    allDay = false
                })
                .ToListAsync();

            return Json(events);
        }

        // GET: /Rezervacija/Create/5
        public async Task<IActionResult> Create(int idMasaze)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // Dobimo vse maserje
            ViewBag.Maserji = await _context.Maserji.ToListAsync();

            var model = new Rezervacija
            {
                MasazaID = idMasaze,
                Ime = user.FirstName,
                Priimek = user.LastName,
                Email = user.Email,
                Telefon = user.Phone,
                Datum = DateTime.Today,
                CasPrihoda = TimeSpan.FromHours(10) // privzeta ura
            };

            return View(model);
        }

        // POST: /Rezervacija/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rezervacija model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // Pripravimo seznam maserjev za ViewBag
            ViewBag.Maserji = await _context.Maserji.ToListAsync();

            // Pretvori čas iz select boxa v TimeSpan
            if (!Request.Form.TryGetValue("CasPrihoda", out var casStr))
            {
                ModelState.AddModelError("CasPrihoda", "Izberi veljaven čas.");
                return View(model);
            }

            if (!TimeSpan.TryParse(casStr, out var cas))
            {
                ModelState.AddModelError("CasPrihoda", "Izberi veljaven čas.");
                return View(model);
            }
            model.CasPrihoda = cas;

            // Osnovna validacija modela
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Nastavimo podatke o uporabniku
            model.Ime = user.FirstName;
            model.Priimek = user.LastName;
            model.Email = user.Email;
            model.Telefon = user.Phone;
            model.UserId = user.Id;

            // Določimo IdRez za novo rezervacijo
            var maxId = await _context.Rezervacije.MaxAsync(r => (int?)r.IdRez) ?? 0;
            model.IdRez = maxId + 1;

            // POIŠČI vse prosti termine za izbrano masažo in datum
            var prostiTermini = await _context.Termini
                .Where(t => t.Datum.Date == model.Datum.Date &&
                            !t.Zaseden)
                .ToListAsync();

            // Filtriramo po času (ure in minute)
            prostiTermini = prostiTermini
                .Where(t => t.Cas_prihoda.Hours == model.CasPrihoda.Hours &&
                            t.Cas_prihoda.Minutes == model.CasPrihoda.Minutes)
                .ToList();

            // Če je izbran maser, filtriramo še po njem
            if (model.MaserID != null)
            {
                prostiTermini = prostiTermini
                    .Where(t => t.MaserID == model.MaserID)
                    .ToList();
            }

            // Če ni nobenega prostega termina
            if (!prostiTermini.Any())
            {
                ModelState.AddModelError("", "Izbran termin ali maser ni več na voljo.");
                return View(model);
            }

            // Vzamemo prvi prost termin
            var termin = prostiTermini.First();

            // Blokiramo termin
            termin.Zaseden = true;
            _context.Termini.Update(termin);

            // Shrani rezervacijo
            await _context.Rezervacije.AddAsync(model);
            await _context.SaveChangesAsync();

            // Preusmeri na potrjeno
            return RedirectToAction("Potrjeno");
        }


        public IActionResult Potrjeno()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
