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
    private readonly IWebHostEnvironment _env;

    public AdminController(
        MassageContext context,
        UserManager<ApplicationUser> userManager,
        IWebHostEnvironment env)
    {
        _context = context;
        _userManager = userManager;
        _env = env;
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

    // =========================
    // STRANKE (UPORABNIKI) - DODATEK ZA UREJANJE
    // =========================

    // GET: /Admin/UrediUporabnika/id
    [HttpGet]
    public async Task<IActionResult> UrediUporabnika(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        return View(user);
    }
    

    // POST: /Admin/UrediUporabnika
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UrediUporabnika(ApplicationUser model)
    {
        // Poiščemo obstoječega uporabnika, da ne povoziš gesla in ostalih sistemskih polj
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null)
            return NotFound();

        // Posodobimo samo dovoljena polja
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;

        // Izvedemo posodobitev preko UserManagerja
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return RedirectToAction(nameof(Stranke)); // Preusmeritev nazaj na seznam strank
        }

        // Če pride do napak (npr. neveljaven telefon), jih izpišemo v View
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }

    public IActionResult Stranke()
    {
        var users = _userManager.Users.ToList();
        ViewBag.ClientsCount = users.Count;
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

    // GET: /Admin/EditMaserja/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditMaserja(int id)
    {
        var maser = await _context.Maserji.FindAsync(id);
        if (maser == null)
            return NotFound();

        return View(maser); // Views/Admin/EditMaserja.cshtml
    }

    // POST: /Admin/EditMaserja
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditMaserja(Maser model, IFormFile? slikaFile)
    {
        
        // 1. Preverimo validacijo (npr. če so vsa obvezna polja izpolnjena)
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // 2. Poiščemo obstoječega maserja v bazi po ID-ju
        var dbMaser = await _context.Maserji.FindAsync(model.IdMaserja);
        
        if (dbMaser == null)
        {
            return NotFound();
        }

        // 3. POSODOBITEV VSEH TEKSTOVNIH IN DATUMSKIH POLJ
        dbMaser.imeMaserja = model.imeMaserja;
        dbMaser.priimekMaserja = model.priimekMaserja;
        dbMaser.eMail = model.eMail;
        dbMaser.opis = model.opis;
        dbMaser.datumRojstva = model.datumRojstva;

        // 4. POSEBNA LOGIKA ZA SLIKO (posodobi se samo, če je naložena nova datoteka)
        if (slikaFile != null && slikaFile.Length > 0)
        {
            // Priprava poti za shranjevanje
            var folder = Path.Combine(_env.WebRootPath, "images", "maserji");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            // Generiranje unikatnega imena
            var ext = Path.GetExtension(slikaFile.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(folder, fileName);

            // Shranjevanje nove datoteke na disk
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await slikaFile.CopyToAsync(stream);
            }

            // Če je maser že imel staro sliko, jo izbrišemo iz diska (da ne smetimo)
            if (!string.IsNullOrEmpty(dbMaser.slika))
            {
                var oldPath = Path.Combine(folder, dbMaser.slika);
                if (System.IO.File.Exists(oldPath)) 
                {
                    System.IO.File.Delete(oldPath);
                }
            }

            // V bazo zapišemo novo ime slike
            dbMaser.slika = fileName;
        }
        // Če slikaFile NI naložena, dbMaser.slika ostane takšna, kot je bila prej v bazi.

        try
        {
            // 5. Shranimo vse spremembe v bazo
            _context.Maserji.Update(dbMaser);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Podatki so bili uspešno posodobljeni.";
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Prišlo je do napake pri shranjevanju: " + ex.Message);
            return View(model);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Maserji));
    }



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
    // POST: /Maser/Create
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UstvariMaserja(Maser model, IFormFile? slikaFile) // <--- DODAJ parameter slikaFile
    {
        if (!ModelState.IsValid)
            return View(model);

        // --- 1. LOGIKA ZA NALAGANJE SLIKE ---
        if (slikaFile != null && slikaFile.Length > 0)
        {
            // Preveri končnico
            var ext = Path.GetExtension(slikaFile.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            if (!allowed.Contains(ext))
            {
                ModelState.AddModelError("slika", "Dovoljene so samo slike: .jpg, .jpeg, .png, .webp");
                return View(model);
            }

            // Pripravi pot: wwwroot/images/maserji
            var folder = Path.Combine(_env.WebRootPath, "images", "maserji");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // Ustvari unikatno ime datoteke (da ne povozimo starih slik z istim imenom)
            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(folder, fileName);

            // Shrani datoteko na disk
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await slikaFile.CopyToAsync(stream);
            }

            // V bazo shranimo samo ime datoteke (npr. "guid-123.jpg")
            // Tvoj View za prikaz (Osebje) zna to prebrati, ker imaš logiko za "else { ... maser.slika }"
            model.slika = fileName;
        }

        // --- 2. LOGIKA ZA ID (Obstoječe) ---
        // ročni "auto increment"
        var maxId = await _context.Maserji.MaxAsync(m => (int?)m.IdMaserja) ?? 0;
        model.IdMaserja = maxId + 1;

        // --- 3. SHRANJEVANJE V BAZO ---
        _context.Maserji.Add(model);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Maser uspešno dodan s sliko."; // Opcijsko obvestilo
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

    // ==========================
    // UREJANJE
    // ==========================

    // GET: /Admin/UrediMasazo/5
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> UrediMasazo(int id)
    {
        var masaza = await _context.Masaze.FindAsync(id);
        if (masaza == null)
            return NotFound();

        return View(masaza); // Views/Admin/UrediMasazo.cshtml
    }

    // POST: /Admin/UrediMasazo
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UrediMasazo(Masaza model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Najbolj varno: naloži iz baze in prepiši polja (da ne povoziš ID-ja ipd.)
        var dbMasaza = await _context.Masaze.FindAsync(model.IdMasaze);
        if (dbMasaza == null)
            return NotFound();

        dbMasaza.NazivMasaze = model.NazivMasaze;
        dbMasaza.Opis = model.Opis;
        dbMasaza.TrajanjeMasaze = model.TrajanjeMasaze;
        dbMasaza.Cena = model.Cena;
        dbMasaza.Vrsta = model.Vrsta;
        dbMasaza.Kratica = model.Kratica;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Masaze));
    }


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
