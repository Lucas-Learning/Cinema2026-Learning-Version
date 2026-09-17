using Cinema2026.API.Dtos;
using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cinema2026.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        // Fast billetpris.
        const decimal TicketPrice = 95m;

        IGenericRepository<Ticket> genericRepo;
        public TicketsController(IGenericRepository<Ticket> r)
        {
            genericRepo = r;
        }

        // GET: api/Tickets              -> alle billetter
        // GET: api/Tickets?movieId=1    -> billetter til en bestemt film
        [HttpGet]
        public async Task<List<Ticket>> GetTickets([FromQuery] int? movieId)
        {
            var tickets = await genericRepo.GetAll();
            if (movieId.HasValue)
            {
                tickets = tickets.Where(t => t.MovieId == movieId.Value).ToList();
            }
            return tickets;
        }

        // GET: api/Tickets/5  -> én billet + 404
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicketById(int id)
        {
            var ticket = await genericRepo.GetById(id);
            if (ticket == null) return NotFound();
            return ticket;
        }

        // POST api/Tickets   body: { movieId, seatId, personId }
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket([FromBody] CreateTicketDto dto)
        {
            // Samme sæde kan ikke bookes to gange til samme film.
            var existing = await genericRepo.GetAll();
            bool seatTaken = existing.Any(t => t.MovieId == dto.MovieId && t.SeatId == dto.SeatId);
            if (seatTaken) return Conflict("Sædet er allerede booket til denne film.");

            var ticket = new Ticket
            {
                MovieId = dto.MovieId,
                SeatId = dto.SeatId,
                PersonId = dto.PersonId,
                price = TicketPrice,
                PurchaseDate = DateTime.Now,
            };

            var created = await genericRepo.Add(ticket);
            return CreatedAtAction(nameof(GetTicketById), new { id = created.Id }, created);
        }

        // DELETE api/Tickets/5  -> annullér booking
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await genericRepo.GetById(id);
            if (ticket == null) return NotFound();
            await genericRepo.Delete(id);
            return NoContent();
        }
    }
}
