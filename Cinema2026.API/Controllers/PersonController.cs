using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;
using Cinema2026.Repo.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cinema2026.API.Controllers
{
    [Route("api/[controller]")] //https://localhost:7131/api/person
    [ApiController]
    public class PersonController : ControllerBase
    {
        // this class uses Repository. to do so we instance an objec
        // variable of type PersonRepositories

        //IPersonRepositories personRepo;// = new PersonRepositories();
        IGenericRepository<Person> genericRepo;
        public PersonController(IGenericRepository<Person> r)
        {
            genericRepo = r;
        }

        [HttpGet]
        public async Task<List<Person>> GetPersons()
        {
            return await genericRepo.GetAll();
        }
        [HttpGet("{id}")]
        public async Task<List<Person>> GetPersonById(int id)
        {
            var person = await genericRepo.GetById(id);
            return person != null ? new List<Person> { person } : new List<Person>();
        }
        [HttpDelete]
        public async Task DeletePerson(int id)
        {
            await genericRepo.Delete(id);
        }
        

        #region Firsttry
        //PersonRepositories personRepo;// = new PersonRepositories();
        //public PersonController(PersonRepositories r) {
        //    personRepo = r;
        //}

        //[HttpGet]
        //public List<Person> GetPersons()
        //{
        //    return personRepo.GetPersons();
        //}
        #endregion Firsttry






        //List<Person> persons = new List<Person>()
        //{
        //    new Person() { Id = 1, name = "John", age = 30 },
        //    new Person() { Id = 2, name = "Jane", age = 25 },
        //    new Person() { Id = 3, name = "Bob", age = 40 }
        //};
        // using my persons list
        //[HttpGet]
        //public List<Person> GetPersons()
        //{
        //    return persons;
        //}
        // using an object from the list
        //[HttpGet]
        //public 


        //// GET: api/<PersonController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<PersonController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<PersonController>
        [HttpPost]
        public async Task<Person> Post([FromBody] Person person)
        {
            var created = await genericRepo.Add(person);
            return created;
        }

        //// PUT api/<PersonController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<PersonController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}