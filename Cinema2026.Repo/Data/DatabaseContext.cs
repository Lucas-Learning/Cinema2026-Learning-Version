using Cinema2026.Repo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema2026.Repo.Data
{
    public class DatabaseContext : DbContext
    {
        //add-migration name
        //update-database

        // EF core fejl =>
        // slet Migration mappe
        // slet database og vinke af slet forbindelse (keep alive)
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) { }

        public DbSet<Person> Persons { get; set; }


    }
    //public class ApplicationDbContext : DbContext
    //{
    //    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    //        : base(options)
    //    {
    //    }
    //}
}