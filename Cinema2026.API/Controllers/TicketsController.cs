using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cinema2026.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        IGenericRepository<Ticket> genericRepo;
        public TicketsController(IGenericRepository<Ticket> r)
        {
            genericRepo = r;
        }
        // GET: api/<TicketsController>
        [HttpGet]
        public async Task<Ticket> GetTicketByOrderId(int orderId)
        {
            var ticket = await genericRepo.GetById(orderId);
            return ticket != null ? ticket : null;
        }
        [HttpPost]
        public async Task<Ticket> PostTicket([FromBody] Ticket ticket)
        {
            var created = await genericRepo.Add(ticket);
            return created;
        }

        //// GET api/<TicketsController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<TicketsController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<TicketsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<TicketsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
