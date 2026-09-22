using Microsoft.AspNetCore.Mvc;
using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;

namespace Cinema2026.API.Controllers
{
    // URL: /api/Movie. Controlleren tager imod HTTP-kald og bruger repository'et
    // til at hente/gemme data — selve database-koden ligger altså ikke her.
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        // Controlleren kender kun interfacet, ikke databasen (løs kobling).
        IGenericRepository<Movie> genericRepo;
        public MovieController(IGenericRepository<Movie> r)
        {
            genericRepo = r;
        }

        // GET: api/Movie
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
        [HttpPost]
        public async Task<Movie> Post([FromBody] Movie movie)
        {
            var created = await genericRepo.Add(movie);
            return created;
        }

        // DELETE api/Movie?id=5
        // Her læses id fra query-strengen (?id=5), ikke fra ruten.
        [HttpDelete]
        public async Task DeleteMovie(int id)
        {
            await genericRepo.Delete(id);
        }
    }
}
