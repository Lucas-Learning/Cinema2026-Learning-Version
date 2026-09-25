using Cinema2026.API.Helpers;
using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cinema2026.API.Controllers
{
    // URL: /api/Hall — en sal indeholder de sæder man kan booke.
    [Route("api/[controller]")]
    [ApiController]
    public class HallController : ControllerBase
    {
        IGenericRepository<Hall> genericRepo;
        // Skal også kunne oprette sæder, når en sal oprettes.
        IGenericRepository<Seat> seatRepo;

        public HallController(IGenericRepository<Hall> r, IGenericRepository<Seat> s)
        {
            genericRepo = r;
            seatRepo = s;
        }

        // GET: api/Hall
        [HttpGet]
        public async Task<List<Hall>> GetAll()
        {
            return await genericRepo.GetAll();
        }

        // GET: api/Hall/5  -> én sal + 404
        [HttpGet("{id}")]
        public async Task<ActionResult<Hall>> GetHallById(int id)
        {
            var hall = await genericRepo.GetById(id);
            if (hall == null) return NotFound();
            return hall;
        }

        // POST api/Hall   body: { name }
        // En ny sal får automatisk standard-sæderne (række A-C x 6), så den kan
        // bruges til afspilninger med det samme. En sal uden sæder kan ikke bookes.
        [HttpPost]
        public async Task<Hall> PostHall([FromBody] Hall hall)
        {
            var created = await genericRepo.Add(hall); // gem salen først, så den får et id ...
            foreach (var seat in StandardHall.NewSeats(created.id)) // ... som sæderne peger på
            {
                await seatRepo.Add(seat);
            }
            return created;
        }
    }
}
