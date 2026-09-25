using System;

namespace Cinema2026.Repo.Models
{
    // En afspilning: én film vises i én sal på ét tidspunkt (fx "Avatar, Sal 2, kl. 10").
    // Det er afspilningen, man køber billet til — ikke filmen — fordi det er den,
    // der bestemmer hvilken sal (og dermed hvilke sæder) der gælder.
    public class Screening
    {
        public int Id { get; set; }
        public int MovieId { get; set; } // FK -> Movie
        public int HallId { get; set; }  // FK -> Hall
        public DateTime StartsAt { get; set; }
    }
}
