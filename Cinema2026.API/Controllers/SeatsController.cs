using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cinema2026.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        // GET: api/<SeatsController>
        IGenericRepository<Seat> genericRepo;
        public SeatsController(IGenericRepository<Seat> r)
        {
            genericRepo = r;
        }
        // GET: api/<SeatsController>
        [HttpGet]
        public async Task<Seat> GetSeatById(int id)
        {
            var seat = await genericRepo.GetById(id);
            return seat != null ? seat : null;
        }
        [HttpPost]
        public async Task<Seat> PostSeat([FromBody] Seat seat)
        {
            var created = await genericRepo.Add(seat);
            return created;
        }

        //// PUT api/<SeatsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<SeatsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
