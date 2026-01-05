using MassageStudio.Data;
using MassageStudio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;


[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly MassageContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(
        MassageContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // 🔹 ADMIN DASHBOARD
    public IActionResult Index()
    {
        return View();
    }

    // =========================
    // TERMINI
    // =========================
    public IActionResult CreateTermin()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTermin(Termin model)
    {
        if (!ModelState.IsValid)
            return View(model);

        _context.Termini.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // =========================
    // STRANKE (UPORABNIKI)
    // =========================
    public IActionResult Stranke()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        // zaščita admina
        if (await _userManager.IsInRoleAsync(user, "Admin"))
            return BadRequest("Admin uporabnika ni dovoljeno izbrisati.");

        // zaščita samega sebe
        if (user.Id == _userManager.GetUserId(User))
            return BadRequest("Ne moreš izbrisati samega sebe.");

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Napaka pri brisanju uporabnika.");
        }

        return RedirectToAction(nameof(Stranke));
    }

    // =========================
    // REZERVACIJE
    // =========================
    public async Task<IActionResult> Rezervacije()
    {
        var rezervacije = await _context.Rezervacije
            .Include(r => r.Maser) // virtual property bo potrebno dodati v model
            .Include(r => r.Masaza) // virtual property bo potrebno dodati v model
            .OrderBy(r => r.Datum)
            .ThenBy(r => r.CasPrihoda)
            .ToListAsync();

        return View(rezervacije);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteRezervacija(int id)
    {
        var r = await _context.Rezervacije.FindAsync(id);
        if (r != null)
        {
            _context.Rezervacije.Remove(r);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Rezervacije));
    }

    public async Task<IActionResult> Termini()
    {
        var termini = await _context.Termini
            .Include(t => t.Maser)
            .OrderBy(t => t.Datum)
            .ThenBy(t => t.Cas_prihoda)
            .ToListAsync();

        return View(termini);
    }
    // GET
    // GET: UstvariTermin
    // GET: UstvariTermin
    public IActionResult UstvariTermin()
    {
        // SelectList maserjev z imenom in priimkom
        var maserji = _context.Maserji
            .Select(m => new { m.IdMaserja, FullName = m.imeMaserja + " " + m.priimekMaserja })
            .ToList();
        ViewBag.Maserji = new SelectList(maserji, "IdMaserja", "FullName");

        // SelectList masaž
        ViewBag.Masaze = new SelectList(_context.Masaze.ToList(), "IdMasaze", "NazivMasaze");

        var model = new Termin
        {
            Datum = DateTime.Today,
            Cas_prihoda = new TimeSpan(10, 0, 0)
        };

        return View(model);
    }

    // POST: UstvariTermin
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UstvariTermin(Termin model)
    {

        // vedno napolnimo dropdown, da View ne crkne
        var maserji = _context.Maserji
            .Select(m => new
            {
                m.IdMaserja,
                FullName = m.imeMaserja + " " + m.priimekMaserja
            })
            .ToList();

        ViewBag.Maserji = new SelectList(maserji, "IdMaserja", "FullName", model.MaserID);

        // ======================
        // MODELSTATE DEBUG
        // ======================
        if (!ModelState.IsValid)
        {
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    Console.WriteLine($" - {key}: {error.ErrorMessage}");
                }
            }

            return View(model);
        }

        try
        {
            // ======================
            // PREVERJANJE PODVOJITEV
            // ======================
            bool obstaja = await _context.Termini.AnyAsync(t =>
                t.Datum == model.Datum &&
                t.Cas_prihoda == model.Cas_prihoda &&
                t.MaserID == model.MaserID
            );

            Console.WriteLine($"Obstaja termin: {obstaja}");

            if (obstaja)
            {
                ModelState.AddModelError("", "Ta termin že obstaja.");
                return View(model);
            }

            // ======================
            // SHRANJEVANJE
            // ======================
            model.Zaseden = false;

            _context.Termini.Add(model);
            var saved = await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Termini));
        }
        catch (Exception ex)
        {
            Console.WriteLine("🔥 EXCEPTION PRI SHRANJEVANJU");
            Console.WriteLine(ex.ToString());

            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTermin(int id)
    {
        var termin = await _context.Termini.FindAsync(id);
        if (termin != null)
        {
            _context.Termini.Remove(termin);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Termini));
    }
    ///MASERJI
    // ==========================
    // ADMIN – UPRAVLJANJE
    // ==========================
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Maserji()
    {
        var maserji = await _context.Maserji.ToListAsync();
        return View(maserji); // poišče Views/Admin/Maserji.cshtml
    }
    // GET: /Maser/Create
    [Authorize(Roles = "Admin")]
    public IActionResult UstvariMaserja()
    {
        return View();
    }

    // POST: /Maser/Create
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UstvariMaserja(Maser model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // ročni "auto increment"
        var maxId = await _context.Maserji.MaxAsync(m => (int?)m.IdMaserja) ?? 0;
        model.IdMaserja = maxId + 1;

        _context.Maserji.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Maserji));
    }


    // GET: /Maser/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var maser = await _context.Maserji.FindAsync(id);
        if (maser == null)
            return NotFound();

        return View(maser);
    }

    // POST: /Maser/Delete/5
    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var maser = await _context.Maserji.FindAsync(id);
        if (maser != null)
        {
            _context.Maserji.Remove(maser);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Maserji));
    }

    // ==========================
    // MASAŽE
    // ==========================

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Masaze()
    {
        var masaze = await _context.Masaze
            .OrderBy(m => m.IdMasaze)
            .ToListAsync();

        return View(masaze);
    }

    // GET
    [Authorize(Roles = "Admin")]
    public IActionResult UstvariMasazo()
    {
        return View();
    }

    // POST
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UstvariMasazo(Masaza model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // ročni auto-increment (ker imaš DatabaseGenerated.None)
        var maxId = await _context.Masaze.MaxAsync(m => (int?)m.IdMasaze) ?? 0;
        model.IdMasaze = maxId + 1;

        _context.Masaze.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Masaze));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMasaza(int id)
    {
        var masaza = await _context.Masaze.FindAsync(id);
        if (masaza != null)
        {
            _context.Masaze.Remove(masaza);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Masaze));
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Obvestila()
    {
        var obvestila = await _context.Obvestila
            .Include(o => o.User)          // da dobimo email uporabnika
            .OrderByDescending(o => o.DatumObjave)
            .ToListAsync();

        return View(obvestila);
    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteObvestilo(int id)
    {
        var obvestilo = await _context.Obvestila.FindAsync(id);

        if (obvestilo != null)
        {
            _context.Obvestila.Remove(obvestilo);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Obvestila));
    }


}
