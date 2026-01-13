using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MassageStudio.Data;
using MassageStudio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MassageStudio.Controllers.Api
{
    [ApiController]
    [Route("api/v1/maser")]
    public class MasersApiController : ControllerBase
    {
        private readonly MassageContext _context;
        private readonly IWebHostEnvironment _env;

        public MasersApiController(MassageContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // =========================
        // GET: api/v1/maser
        // =========================
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Maserji.AsNoTracking().ToListAsync();
            return Ok(list);
        }

        // =========================
        // GET: api/v1/maser/5
        // =========================
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOne(int id)
        {
            var maser = await _context.Maserji.AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdMaserja == id);

            if (maser == null) return NotFound();
            return Ok(maser);
        }

        // =========================
        // POST: api/v1/maser
        // =========================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Maser maser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Ker imaš DatabaseGeneratedOption.None:
            // če Id ni podan, mu ga nastavimo (da "deluje")
            if (maser.IdMaserja == 0)
            {
                var maxId = await _context.Maserji.AnyAsync()
                    ? await _context.Maserji.MaxAsync(x => x.IdMaserja)
                    : 0;
                maser.IdMaserja = maxId + 1;
            }
            else
            {
                var exists = await _context.Maserji.AnyAsync(x => x.IdMaserja == maser.IdMaserja);
                if (exists) return Conflict("Maser z istim IdMaserja že obstaja.");
            }

            _context.Maserji.Add(maser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOne), new { id = maser.IdMaserja }, maser);
        }

        // =========================
        // PUT: api/v1/maser/5
        // =========================
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Maser dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.IdMaserja) return BadRequest("ID v URL in body se ne ujemata.");

            var maser = await _context.Maserji.FirstOrDefaultAsync(x => x.IdMaserja == id);
            if (maser == null) return NotFound();

            maser.imeMaserja = dto.imeMaserja;
            maser.priimekMaserja = dto.priimekMaserja;
            maser.eMail = dto.eMail;
            maser.opis = dto.opis;
            maser.datumRojstva = dto.datumRojstva;

        

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // =========================
        // DELETE: api/v1/maser/5
        // =========================
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var maser = await _context.Maserji.FirstOrDefaultAsync(x => x.IdMaserja == id);
            if (maser == null) return NotFound();

            _context.Maserji.Remove(maser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // =========================
        // POST: api/v1/maser/5/slika  (UPLOAD)
        // =========================
        [HttpPost("{id:int}/slika")]
        [RequestSizeLimit(10_000_000)] // 10MB
        public async Task<IActionResult> UploadImage(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Manjka datoteka (file).");

            var maser = await _context.Maserji.FirstOrDefaultAsync(x => x.IdMaserja == id);
            if (maser == null) return NotFound();

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowed.Contains(ext))
                return BadRequest("Dovoljene so samo slike: .jpg, .jpeg, .png, .webp");

            var folder = Path.Combine(_env.WebRootPath, "images", "maserji");
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(folder, fileName);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // v bazo shrani samo filename 
            maser.slika = fileName;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                id = maser.IdMaserja,
                fileName,
                url = $"/images/maserji/{fileName}"
            });
        }
    }
}
