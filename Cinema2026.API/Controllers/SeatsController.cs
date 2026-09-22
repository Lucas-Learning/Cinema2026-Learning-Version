using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cinema2026.API.Controllers
{
    // URL: /api/Seats
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        // Samme generiske repository som de andre controllers — bare med Seat som type.
        IGenericRepository<Seat> genericRepo;
        public SeatsController(IGenericRepository<Seat> r)
        {
            genericRepo = r;
        }

        // GET: api/Seats            -> alle sæder
        // GET: api/Seats?hallId=2   -> kun sæder i en bestemt sal
        [HttpGet]
        public async Task<List<Seat>> GetSeats([FromQuery] int? hallId)
        {
            var seats = await genericRepo.GetAll();
            // Valgfrit filter: uden hallId får man alle sæder.
            // .Where(...) er LINQ og beholder kun de elementer hvor betingelsen er sand.
            // "s => s.HallId == ..." er en lambda — en kort, unavngiven funktion.
            if (hallId.HasValue)
            {
                seats = seats.Where(s => s.HallId == hallId.Value).ToList();
            }
            return seats;
        }

        // GET: api/Seats/5  -> ét sæde + 404
        // 404 er bedre end at returnere null, fordi klienten så tydeligt kan se
        // forskel på "findes ikke" og "fandtes, men var tom".
        [HttpGet("{id}")]
        public async Task<ActionResult<Seat>> GetSeatById(int id)
        {
            var seat = await genericRepo.GetById(id);
            if (seat == null) return NotFound();
            return seat;
        }

        // POST api/Seats
        [HttpPost]
        public async Task<Seat> PostSeat([FromBody] Seat seat)
        {
            var created = await genericRepo.Add(seat);
            return created;
        }
    }
}
