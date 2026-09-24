using Cinema2026.API.Helpers;
using Cinema2026.Repo.Data;
using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;
using Cinema2026.Repo.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

// Her sættes hele API'et op. Først registreres de "services" appen skal bruge
// (builder), derefter bestemmes rækkefølgen requests behandles i (app).
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers(); // finder alle vores Controller-klasser

// Swagger = testside hvor man kan kalde API'et uden en frontend.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Fortæller EF Core hvilken database vi bruger, og hvor den ligger.
// Connection string'en hentes fra appsettings, så den kan være forskellig
// fra maskine til maskine uden at ændre koden.
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VoresDatabase")));

//builder.Services.AddScoped<IPersonRepositories, PersonRepositories>();
//allow the Angular dev server to call the API.
// Uden CORS blokerer browseren kald fra localhost:4200 til localhost:7131,
// fordi det er to forskellige "origins" (porte).
const string AngularCorsPolicy = "AngularDev";
builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularCorsPolicy, policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Dependency Injection: når en controller beder om IGenericRepository<Movie>,
// laver .NET automatisk en GenericRepository<Movie>. Vi skriver typeof(...<>),
// fordi den samme regel skal gælde for ALLE modeller (Movie, Seat, Ticket...).
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped<Interface,class> ();

var app = builder.Build();

// Seed: sørg for at der altid findes en admin-bruger (josef / 1234), så man kan
// komme ind på /admin. Koden kører hver gang API'et starter, men opretter kun
// brugeren, hvis den mangler — så den laver aldrig dubletter.
// DatabaseContext er "scoped" (én pr. HTTP-request). Her er vi UDEN FOR et request,
// så vi laver selv et scope med CreateScope() for at kunne få fat i den.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    if (!db.Persons.Any(p => p.Username == "josef"))
    {
        var admin = new Person { name = "Josef", age = 25, Username = "josef", IsAdmin = true };
        admin.PasswordHash = new PasswordHasher<Person>().HashPassword(admin, "1234");
        db.Persons.Add(admin);
        db.SaveChanges();
    }

    // Seed 2: film uden sal (oprettet før "automatisk sal" fandtes) får en
    // standard-sal, så de også kan bookes. Nye film får salen i MovieController.
    // .ToList() henter listen færdig, før vi begynder at ændre i databasen.
    foreach (var movie in db.Movies.Where(m => m.HallId == null).ToList())
    {
        var hall = StandardHall.NewHall(movie.title);
        db.Halls.Add(hall);
        db.SaveChanges(); // gem salen først, så hall.id bliver sat

        db.Seats.AddRange(StandardHall.NewSeats(hall.id));
        movie.HallId = hall.id;
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
// Swagger kun i udvikling — den skal ikke være offentlig i produktion.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); // kobler URL'er som /api/Movie til vores controllers

app.Run();
