using Microsoft.AspNetCore.Mvc;
using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;
using Cinema2026.Repo.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cinema2026.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        IGenericRepository<Movie> genericRepo;
        public MovieController(IGenericRepository<Movie> r)
        {
            genericRepo = r;
        }
        // GET: api/<MoviesController>
        [HttpGet]
        public async Task<List<Movie>> GetAll()
        {
            return await genericRepo.GetAll();
        }

        // POST api/<MoviesController>
        [HttpPost]
        public async Task<Movie> Post([FromBody] Movie movie)
        {
            var created = await genericRepo.Add(movie);
            return created;
        }

        [HttpDelete]
        public async Task DeleteMovie(int id)
        {
            await genericRepo.Delete(id);
        }

    }
}
