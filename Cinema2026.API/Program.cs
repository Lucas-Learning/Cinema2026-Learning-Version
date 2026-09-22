using Cinema2026.Repo.Data;
using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Repositories;
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
