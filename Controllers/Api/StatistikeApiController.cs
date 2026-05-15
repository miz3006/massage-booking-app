using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MassageStudio.Data;
using MassageStudio.Filters;

namespace MasazeBooking.Controllers.Api
{
    [Route("api/statistike")]
    [ApiController]
    [ApiKeyAuth]
    public class StatistikeApiController : ControllerBase
    {
        private readonly MassageContext _context;

        public StatistikeApiController(MassageContext context)
        {
            _context = context;
        }

        // GET: api/statistike
        [HttpGet]
        public async Task<ActionResult<StatistikeResponse>> GetStatistike()
        {
            var skupaj = await _context.Rezervacije.CountAsync();

            var danes = DateTime.Today;
            var mesec = danes.Month;
            var leto = danes.Year;

            var rezervacijeDanes = await _context.Rezervacije
                .CountAsync(r => r.Datum.Date == danes);

            var rezervacijeMesec = await _context.Rezervacije
                .CountAsync(r => r.Datum.Month == mesec && r.Datum.Year == leto);

            // Rezervacije po vrsti masaže
            var masazeDict = await _context.Masaze
                .ToDictionaryAsync(m => m.IdMasaze, m => m.NazivMasaze);

            var poMasaziRaw = await _context.Rezervacije
                .GroupBy(r => r.MasazaID)
                .Select(g => new { MasazaID = g.Key, Stevilo = g.Count() })
                .OrderByDescending(g => g.Stevilo)
                .ToListAsync();

            var poMasazi = poMasaziRaw.Select(s => new PoMasaziStat
            {
                IdMasaze = s.MasazaID,
                Naziv = masazeDict.TryGetValue(s.MasazaID, out var n) ? n : "Neznana",
                Stevilo = s.Stevilo
            }).ToList();

            // Zasedenost po maserju
            var maserjiDict = await _context.Maserji
                .ToDictionaryAsync(m => m.IdMaserja,
                    m => $"{m.imeMaserja} {m.priimekMaserja}");

            var poMaserjuRaw = await _context.Rezervacije
                .Where(r => r.MaserID != null)
                .GroupBy(r => r.MaserID!.Value)
                .Select(g => new { MaserID = g.Key, Stevilo = g.Count() })
                .OrderByDescending(g => g.Stevilo)
                .ToListAsync();

            var poMaserju = poMaserjuRaw.Select(s => new PoMaserjuStat
            {
                IdMaserja = s.MaserID,
                Ime = maserjiDict.TryGetValue(s.MaserID, out var ime) ? ime : "Neznan",
                Stevilo = s.Stevilo
            }).ToList();

            return Ok(new StatistikeResponse
            {
                SkupajRezervacij = skupaj,
                RezervacijeDanes = rezervacijeDanes,
                RezervacijeMesec = rezervacijeMesec,
                PoMasazi = poMasazi,
                PoMaserju = poMaserju
            });
        }

        public class StatistikeResponse
        {
            public int SkupajRezervacij { get; set; }
            public int RezervacijeDanes { get; set; }
            public int RezervacijeMesec { get; set; }
            public List<PoMasaziStat> PoMasazi { get; set; } = new();
            public List<PoMaserjuStat> PoMaserju { get; set; } = new();
        }

        public class PoMasaziStat
        {
            public int IdMasaze { get; set; }
            public string Naziv { get; set; } = "";
            public int Stevilo { get; set; }
        }

        public class PoMaserjuStat
        {
            public int IdMaserja { get; set; }
            public string Ime { get; set; } = "";
            public int Stevilo { get; set; }
        }
    }
}
