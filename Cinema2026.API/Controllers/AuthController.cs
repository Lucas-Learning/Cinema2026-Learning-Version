using Cinema2026.API.Dtos;
using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cinema2026.API.Controllers
{
    // URL: /api/Auth — opret bruger og log ind.
    // Kunden ER en Person; vi genbruger altså Persons-tabellen i stedet for at
    // lave en separat bruger-tabel.
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IGenericRepository<Person> personRepo;

        // PasswordHasher er indbygget i ASP.NET. Den laver en sikker hash af en
        // adgangskode og kan senere tjekke, om en indtastet kode matcher hashen.
        // Vi skal altså ikke selv opfinde kryptering — det går som regel galt.
        private readonly PasswordHasher<Person> hasher = new();

        public AuthController(IGenericRepository<Person> r)
        {
            personRepo = r;
        }

        // Fælles fejlbesked: samme tekst uanset om brugernavn eller kode er forkert,
        // så man ikke kan gætte sig frem til hvilke brugernavne der findes.
        private const string LoginFailed = "Forkert brugernavn eller adgangskode.";

        // POST api/Auth/register   body: { name, age, username, password }
        // [HttpPost("register")] lægger "register" oven i controllerens rute.
        [HttpPost("register")]
        public async Task<ActionResult<Person>> Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Brugernavn og adgangskode skal udfyldes.");
            }

            // Brugernavne skal være unikke — ellers ved vi ikke hvem der logger ind.
            // OrdinalIgnoreCase = sammenlign uden at skelne mellem store/små bogstaver.
            var all = await personRepo.GetAll();
            bool taken = all.Any(p => p.Username != null
                && p.Username.Equals(dto.Username, StringComparison.OrdinalIgnoreCase));
            if (taken) return Conflict("Brugernavnet er allerede taget.");

            var person = new Person { name = dto.Name, age = dto.Age, Username = dto.Username };
            person.PasswordHash = hasher.HashPassword(person, dto.Password);

            var created = await personRepo.Add(person);
            // PasswordHash sendes ikke med tilbage — det sørger [JsonIgnore] på modellen for.
            return created;
        }

        // POST api/Auth/login   body: { username, password }
        [HttpPost("login")]
        public async Task<ActionResult<Person>> Login([FromBody] LoginDto dto)
        {
            var all = await personRepo.GetAll();
            var person = all.FirstOrDefault(p => p.Username != null
                && p.Username.Equals(dto.Username, StringComparison.OrdinalIgnoreCase));

            // 401 Unauthorized = "du er ikke logget ind / forkerte oplysninger".
            if (person == null || person.PasswordHash == null) return Unauthorized(LoginFailed);

            // Hasher det indtastede og sammenligner med det gemte.
            var result = hasher.VerifyHashedPassword(person, person.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed) return Unauthorized(LoginFailed);

            return person;
        }
    }
}
