using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema2026.Repo.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int MovieId { get; set; }  // FK -> Movie
        public int SeatId { get; set; }   // FK -> Seat
        public int PersonId { get; set; } // FK -> Person
        public decimal price { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
