using Microsoft.AspNetCore.Mvc;
using Cinema2026.API.Helpers;
using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;

namespace Cinema2026.API.Controllers
{
    // URL: /api/Movie. Controlleren tager imod HTTP-kald og bruger repository'et
    // til at hente/gemme data — selve database-koden ligger altså ikke her.
    // [controller] i [Route] bliver automatisk til klassens navn uden "Controller" (altså "Movie").
    // [ApiController] slår bl.a. automatisk validering af input til.
    // ControllerBase giver adgang til hjælpe-metoder som NotFound() og CreatedAtAction().
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        // Controlleren kender kun interfacet, ikke databasen (løs kobling).
        IGenericRepository<Movie> genericRepo;
        // Skal også kunne oprette sal + sæder, når en film oprettes.
        IGenericRepository<Hall> hallRepo;
        IGenericRepository<Seat> seatRepo;

        public MovieController(
            IGenericRepository<Movie> r,
            IGenericRepository<Hall> h,
            IGenericRepository<Seat> s)
        {
            genericRepo = r;
            hallRepo = h;
            seatRepo = s;
        }

        // GET: api/Movie
        // async Task<T>: metoden kører asynkront (venter ikke og blokerer serveren)
        // og ender med at levere en List<Movie>, når databasekaldet er færdigt.
        [HttpGet]
        public async Task<List<Movie>> GetAll()
        {
            return await genericRepo.GetAll();
        }

        // GET: api/Movie/5  -> ét objekt + 404
        // actionresult return type is used to return a 404 if the object is not found
        // {id} i ruten betyder at værdien læses fra selve URL'en.
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovieById(int id)
        {
            var movie = await genericRepo.GetById(id);
            if (movie == null) return NotFound();
            return movie;
        }

        // POST api/Movie
        // [FromBody] = filmen sendes som JSON i request-bodyen.
        // Hver ny film får automatisk sin egen standard-sal med sæder,
        // så den kan bookes med det samme.
        [HttpPost]
        public async Task<Movie> Post([FromBody] Movie movie)
        {
            var created = await genericRepo.Add(movie);

            // 1) Gem salen først, så den får et id ...
            var hall = await hallRepo.Add(StandardHall.NewHall(created.title));

            // 2) ... som sæderne kan pege på.
            foreach (var seat in StandardHall.NewSeats(hall.id))
            {
                await seatRepo.Add(seat);
            }

            // 3) Kobl salen på filmen.
            created.HallId = hall.id;
            await genericRepo.Update(created);

            return created;
        }

        // DELETE api/Movie?id=5
        // Her læses id fra query-strengen (?id=5), ikke fra ruten.
        // Filmens sal og sæder bliver liggende — det er godt nok til projektet.
        [HttpDelete]
        public async Task DeleteMovie(int id)
        {
            await genericRepo.Delete(id);
        }
    }
}
