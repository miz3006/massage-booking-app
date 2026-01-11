using MassageStudio.Data;
using MassageStudio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MassageStudio.Controllers
{
    [Authorize]
    public class RezervacijaController : Controller
    {
        private readonly MassageContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RezervacijaController(MassageContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Pomožna metoda za trenutni čas v Sloveniji
        private static DateTime NowLj()
        {
            var utcNow = DateTime.UtcNow;
            TimeZoneInfo tz;
            try {
                tz = TimeZoneInfo.FindSystemTimeZoneById("Europe/Ljubljana");
            }
            catch {
                try {
                    tz = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                }
                catch {
                    return DateTime.Now;
                }
            }
            return TimeZoneInfo.ConvertTimeFromUtc(utcNow, tz);
        }

        // API za FullCalendar: vrne samo prihodnje proste termine
        [HttpGet]
        public async Task<IActionResult> GetEvents(int idMasaze, int? maserID)
        {
            var now = NowLj();

            var query = _context.Termini
                .Where(t => !t.Zaseden);

            // Filtriranje: Termin mora biti v prihodnosti
            // t.Datum.Date + t.Cas_prihoda mora biti > now
            var events = await query.ToListAsync();
            
            var filteredEvents = events
                .Where(t => t.Datum.Date.Add(t.Cas_prihoda) > now)
                .Where(t => maserID == null || t.MaserID == maserID)
                .Select(t => new
                {
                    id = t.IdTermin,
                    title = "Prosto",
                    start = t.Datum.Date.Add(t.Cas_prihoda).ToString("yyyy-MM-ddTHH:mm:ss"),
                    allDay = false
                })
                .ToList();

            return Json(filteredEvents);
        }

        // GET: /Rezervacija/Create/5
        public async Task<IActionResult> Create(int idMasaze)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            ViewBag.Maserji = await _context.Maserji.ToListAsync();
            var now = NowLj();

            var model = new Rezervacija
            {
                MasazaID = idMasaze,
                Ime = user.FirstName,
                Priimek = user.LastName,
                Email = user.Email,
                Telefon = user.Phone ?? user.PhoneNumber,
                // Nastavimo privzeti datum na danes, če so še prosti termini, sicer na jutri
                Datum = now.Hour >= 20 ? now.Date.AddDays(1) : now.Date,
                CasPrihoda = TimeSpan.FromHours(now.Hour + 1) // predlagamo naslednjo polno uro
            };

            return View(model);
        }

        // POST: /Rezervacija/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rezervacija model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            ViewBag.Maserji = await _context.Maserji.ToListAsync();

            // 1. Ročno pridobivanje časa, če ni v modelu
            if (model.CasPrihoda == TimeSpan.Zero && Request.Form.TryGetValue("CasPrihoda", out var casStr))
            {
                if (TimeSpan.TryParse(casStr, out var cas)) model.CasPrihoda = cas;
            }

            // 2. Preverjanje, če je termin v preteklosti
            var now = NowLj();
            var selectedDateTime = model.Datum.Date.Add(model.CasPrihoda);

            if (selectedDateTime <= now)
            {
                ModelState.AddModelError("", "Izbrani termin je že potekel. Prosimo, izberite čas v prihodnosti.");
                return View(model);
            }

            if (!ModelState.IsValid) return View(model);

            // Nastavitev fiksnih podatkov
            model.Ime = user.FirstName;
            model.Priimek = user.LastName;
            model.Email = user.Email;
            model.UserId = user.Id;

            // 3. Iskanje prostega termina v bazi
            // Pomembno: Primerjamo samo Date del datuma
            var termin = await _context.Termini
                .Where(t => t.Datum.Date == model.Datum.Date)
                .Where(t => t.Cas_prihoda == model.CasPrihoda)
                .Where(t => !t.Zaseden)
                .Where(t => model.MaserID == null || t.MaserID == model.MaserID)
                .FirstOrDefaultAsync();

            if (termin == null)
            {
                ModelState.AddModelError("", "Žal ta termin ni več na voljo ali pa je že zaseden.");
                return View(model);
            }

            // 4. Izvedba rezervacije v transakciji
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Posodobi termin
                termin.Zaseden = true;
                _context.Termini.Update(termin);

                // Ustvari rezervacijo
                var maxId = await _context.Rezervacije.AnyAsync() ? await _context.Rezervacije.MaxAsync(r => r.IdRez) : 0;
                model.IdRez = maxId + 1;
                
                _context.Rezervacije.Add(model);
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction("Potrjeno");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "Prišlo je do napake pri shranjevanju. Poskusite znova.");
                return View(model);
            }
        }

        public IActionResult Potrjeno() => View();
    }
}