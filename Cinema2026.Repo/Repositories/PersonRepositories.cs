using Cinema2026.Repo.Interfaces;
using Cinema2026.Repo.Models;
using Cinema2026.Repo.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Cinema2026.Repo.Repositories
{
    public class PersonRepositories : IPersonRepositories
    {
        private readonly DatabaseContext _context;
        public PersonRepositories(DatabaseContext d)
        {
            _context = d;
        }
        // create 
        // get
        List<Person> persons = new List<Person>()
        {
            new Person() { Id = 1, name = "John", age = 30 },
            new Person() { Id = 2, name = "Jane", age = 25 },
            new Person() { Id = 3, name = "Bob", age = 40 }
        };
        // using my persons list
        /*public List<Person> GetPersons()
        {
            return persons;
        }*/
        public async Task<List<Person>> GetPersonsFromDb()
        {
            return await _context.Persons.ToListAsync();
        }
        public async Task Delete(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person != null)
            {
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Person> CreatePerson(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }
    }
}