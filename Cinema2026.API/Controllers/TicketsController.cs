using Cinema2026.API.Dtos;
using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cinema2026.API.Controllers
{
    // [Route] bestemmer URL'en: [controller] bliver til "Tickets" -> /api/Tickets
    // [ApiController] giver bl.a. automatisk validering af input.
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        // Fast billetpris.
        // Ligger på serveren, så en klient ikke selv kan bestemme prisen.
        // const = en konstant, der aldrig kan ændres. "m" gør 95 til en decimal (præcis til penge).
        const decimal TicketPrice = 95m;

        // Repository'et kommer ind via constructoren (Dependency Injection).
        IGenericRepository<Ticket> genericRepo;
        public TicketsController(IGenericRepository<Ticket> r)
        {
            genericRepo = r;
        }

        // GET: api/Tickets                  -> alle billetter
        // GET: api/Tickets?screeningId=3    -> billetter til én afspilning (= optagne sæder)
        // GET: api/Tickets?personId=2       -> én kundes billetter ("Mine billetter")
        [HttpGet]
        public async Task<List<Ticket>> GetTickets([FromQuery] int? screeningId, [FromQuery] int? personId)
        {
            var tickets = await genericRepo.GetAll();
            // int? (nullable) gør parametrene valgfrie: uden dem får man alle.
            if (screeningId.HasValue)
            {
                tickets = tickets.Where(t => t.ScreeningId == screeningId.Value).ToList();
            }
            if (personId.HasValue)
            {
                tickets = tickets.Where(t => t.PersonId == personId.Value).ToList();
            }
            return tickets;
        }

        // GET: api/Tickets/5  -> én billet + 404
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicketById(int id)
        {
            var ticket = await genericRepo.GetById(id);
            // ActionResult gør det muligt at svare med en statuskode i stedet for data.
            if (ticket == null) return NotFound();
            return ticket;
        }

        // POST api/Tickets   body: { screeningId, seatId, personId }
        // Vi tager imod en DTO i stedet for en hel Ticket, så klienten ikke kan
        // sende sin egen pris eller sit eget Id.
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket([FromBody] CreateTicketDto dto)
        {
            // Samme sæde kan ikke bookes to gange til samme afspilning
            // (men gerne til en anden afspilning — også af samme film).
            var existing = await genericRepo.GetAll();
            // Any() = "findes der mindst ét element, hvor betingelsen er sand?" -> true/false.
            bool seatTaken = existing.Any(t => t.ScreeningId == dto.ScreeningId && t.SeatId == dto.SeatId);
            // 409 Conflict betyder "det kan ikke lade sig gøre lige nu" — frontend
            // viser så en besked om at vælge et andet sæde.
            if (seatTaken) return Conflict("Sædet er allerede booket til denne afspilning.");

            // Pris og dato sættes her på serveren — aldrig af klienten.
            var ticket = new Ticket
            {
                ScreeningId = dto.ScreeningId,
                SeatId = dto.SeatId,
                PersonId = dto.PersonId,
                price = TicketPrice,
                PurchaseDate = DateTime.Now,
            };

            var created = await genericRepo.Add(ticket);
            // 201 Created + hvor den nye billet kan hentes henne.
            // nameof(GetTicketById) giver metodens navn som tekst — så det følger med,
            // hvis metoden en dag omdøbes (i stedet for at skrive "GetTicketById" direkte).
            return CreatedAtAction(nameof(GetTicketById), new { id = created.Id }, created);
        }

        // DELETE api/Tickets/5  -> annullér booking
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await genericRepo.GetById(id);
            if (ticket == null) return NotFound();
            await genericRepo.Delete(id);
            // 204 NoContent = det lykkedes, men der er ikke noget at sende tilbage.
            return NoContent();
        }
    }
}
