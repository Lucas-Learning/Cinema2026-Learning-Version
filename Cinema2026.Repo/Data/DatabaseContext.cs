using Cinema2026.Repo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema2026.Repo.Data
{
    // DatabaseContext er bindeleddet mellem C#-koden og SQL-databasen.
    // Hver DbSet<T> herunder svarer til én tabel i databasen.
    public class DatabaseContext : DbContext
    {

        //DETTE SKAL MAN GØR HVER GANG MAN TILFØJER EN NY CONTROLLER. HUSK OGSÅ AT SLET DATABASEN,MIGRATION FOLDER
        //HVIS DEN ALLEREDE FINDES.

        //add-migration name
        //update-database

        // EF core fejl =>
        // slet Migration mappe
        // slet database og vinke af slet forbindelse (keep alive)

        // Options (bl.a. connection string) kommer udefra fra Program.cs,
        // så klassen ikke selv skal vide hvor databasen ligger.
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) { }

        // VIGTIGT: en model uden en DbSet her findes ikke for EF Core.
        // Det var præcis derfor Tickets crashede, før Tickets-linjen blev tilføjet.
        public DbSet<Person> Persons { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Ticket> Tickets { get; set; }


    }
    //public class ApplicationDbContext : DbContext
    //{
    //    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    //        : base(options)
    //    {
    //    }
    //}
}
