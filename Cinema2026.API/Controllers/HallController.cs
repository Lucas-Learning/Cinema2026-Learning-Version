using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cinema2026.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallController : ControllerBase
    {
        IGenericRepository<Hall> genericRepo;
        public HallController(IGenericRepository<Hall> r)
        {
            genericRepo = r;
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

        // POST api/Hall
        [HttpPost]
        public async Task<Hall> PostHall([FromBody] Hall hall)
        {
            var created = await genericRepo.Add(hall);
            return created;
        }
    }
}
