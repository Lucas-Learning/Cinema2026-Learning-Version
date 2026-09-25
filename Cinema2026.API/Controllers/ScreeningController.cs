using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cinema2026.API.Controllers
{
    // URL: /api/Screening — afspilninger (film + sal + tidspunkt).
    // Admin opretter dem; kunder vælger én af dem, når de booker.
    [Route("api/[controller]")]
    [ApiController]
    public class ScreeningController : ControllerBase
    {
        IGenericRepository<Screening> genericRepo;
        public ScreeningController(IGenericRepository<Screening> r)
        {
            genericRepo = r;
        }

        // GET: api/Screening             -> alle afspilninger
        // GET: api/Screening?movieId=1   -> kun afspilninger af én film (film-siden)
        [HttpGet]
        public async Task<List<Screening>> GetScreenings([FromQuery] int? movieId)
        {
            var screenings = await genericRepo.GetAll();
            if (movieId.HasValue)
            {
                screenings = screenings.Where(s => s.MovieId == movieId.Value).ToList();
            }
            // Sorteret efter tidspunkt, så den tidligste kommer først.
            return screenings.OrderBy(s => s.StartsAt).ToList();
        }

        // GET: api/Screening/5  -> én afspilning + 404
        [HttpGet("{id}")]
        public async Task<ActionResult<Screening>> GetScreeningById(int id)
        {
            var screening = await genericRepo.GetById(id);
            if (screening == null) return NotFound();
            return screening;
        }

        // POST api/Screening   body: { movieId, hallId, startsAt }
        [HttpPost]
        public async Task<ActionResult<Screening>> Post([FromBody] Screening screening)
        {
            var created = await genericRepo.Add(screening);
            return CreatedAtAction(nameof(GetScreeningById), new { id = created.Id }, created);
        }

        // DELETE api/Screening/5
        // Billetter til afspilningen bliver liggende — godt nok til projektet.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var screening = await genericRepo.GetById(id);
            if (screening == null) return NotFound();
            await genericRepo.Delete(id);
            return NoContent();
        }
    }
}
