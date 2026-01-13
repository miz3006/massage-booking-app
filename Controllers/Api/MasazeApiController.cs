using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MassageStudio.Data;
using MassageStudio.Models;
using MassageStudio.Filters;


namespace MasazeBooking.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class MasazeApiController : ControllerBase
    {
        private readonly MassageContext _context;

        public MasazeApiController(MassageContext context)
        {
            _context = context;
        }

        public class MasazaDto
        {
            public string NazivMasaze { get; set; } = "";
            public int TrajanjeMasaze { get; set; }
            public string? Opis { get; set; }
            public decimal Cena { get; set; }
            public string? Kratica { get; set; }
            public string? Vrsta { get; set; }
        }

        // GET: api/MasazeApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Masaza>>> GetMasaze()
        {
            return await _context.Masaze
                .AsNoTracking()
                .OrderBy(m => m.IdMasaze)
                .ToListAsync();
        }

        // GET: api/MasazeApi/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Masaza>> GetMasaza(int id)
        {
            var masaza = await _context.Masaze.FindAsync(id);
            if (masaza == null) return NotFound();
            return masaza;
        }

        // PUT: api/MasazeApi/5  (posodobitev brez overpostinga)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutMasaza(int id, [FromBody] MasazaDto dto)
        {
            var db = await _context.Masaze.FindAsync(id);
            if (db == null) return NotFound();

            db.NazivMasaze = dto.NazivMasaze;
            db.TrajanjeMasaze = dto.TrajanjeMasaze;
            db.Opis = dto.Opis;
            db.Cena = dto.Cena;
            db.Kratica = dto.Kratica;
            db.Vrsta = dto.Vrsta;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/MasazeApi  (ID se dodeli na serverju)
        [HttpPost]
        public async Task<ActionResult<Masaza>> PostMasaza([FromBody] MasazaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NazivMasaze))
                return BadRequest("NazivMasaze je obvezen.");

          
            var maxId = await _context.Masaze.MaxAsync(m => (int?)m.IdMasaze) ?? 0;
            var newId = maxId + 1;

            var masaza = new Masaza
            {
                IdMasaze = newId,
                NazivMasaze = dto.NazivMasaze,
                TrajanjeMasaze = dto.TrajanjeMasaze,
                Opis = dto.Opis,
                Cena = dto.Cena,
                Kratica = dto.Kratica,
                Vrsta = dto.Vrsta
            };

            _context.Masaze.Add(masaza);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMasaza), new { id = masaza.IdMasaze }, masaza);
        }

        // DELETE: api/MasazeApi/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMasaza(int id)
        {
            var masaza = await _context.Masaze.FindAsync(id);
            if (masaza == null) return NotFound();

            _context.Masaze.Remove(masaza);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
