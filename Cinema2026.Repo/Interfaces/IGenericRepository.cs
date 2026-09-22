using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema2026.Repo.Interfaces
{
    // En "kontrakt": her står HVAD man kan gøre med data — ikke hvordan.
    // Controllerne kender kun denne interface, ikke databasen. Fordelen er,
    // at vi kan skifte hvordan data gemmes (fx til test) uden at røre controllerne.
    //
    // <T> betyder at den virker for enhver model (Movie, Seat, Ticket...),
    // så vi slipper for at skrive næsten ens repository-kode for hver model.
    public interface IGenericRepository <T> where T : class
    {
        public Task<List<T>> GetAll();
        public Task<T?> GetById(int id); // T? = kan være null hvis intet findes
        public Task<T> Add(T entity);
        public Task Update(T entity);
        public Task Delete(int id);
    }
}
