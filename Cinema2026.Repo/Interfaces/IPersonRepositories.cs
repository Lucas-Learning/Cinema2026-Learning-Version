using Cinema2026.Repo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema2026.Repo.Interfaces
{
    public interface IPersonRepositories
    {
        //public List<Person> GetPersons();
        public Task<List<Person>> GetPersonsFromDb();
        public Task<Person> CreatePerson(Person person);
        public Task Delete (int id);
    }
}