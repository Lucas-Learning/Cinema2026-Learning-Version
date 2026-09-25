using Cinema2026.Repo.Data;
using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cinema2026.Repo.Repositories
{
    // Den konkrete implementering af kontrakten: her snakkes der rigtigt med databasen.
    // Fordi den er generisk (<T>), dækker denne ene klasse alle vores modeller.
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        // readonly = kan kun sættes i constructoren, så ingen kan bytte databasen ud senere.
        // "_" foran navnet er en almindelig konvention for private felter.
        private readonly DatabaseContext _context;

        // Databasen kommer ind udefra (Dependency Injection) i stedet for at
        // blive oprettet her — så deler alle repositories den samme forbindelse.
        public GenericRepository(DatabaseContext d)
        {
            _context = d;
        }

        // Set<T>() finder automatisk den rigtige tabel ud fra typen.
        // async/await bruges, så serveren ikke står stille og venter på databasen.
        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            if (id <= 0)
            {
                // throw stopper metoden med en fejl (exception), som kalderen kan fange.
                throw new ArgumentException("Invalid ID");
            }
            // FindAsync slår op på primærnøglen. Returnerer null hvis den ikke findes.
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Add(T entity)
        {
            _context.Set<T>().Add(entity);
            // Intet gemmes i databasen før SaveChangesAsync kaldes.
            await _context.SaveChangesAsync();
            // Returneres så kalderen får det tildelte Id med tilbage.
            return entity;
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            // Tjekker først om den findes, så vi undgår en fejl ved sletning.
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
