using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema2026.Repo.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int SeatId { get; set; } // foreign key
        public int MovieId { get; set; }
        public int PersonId { get; set; } // foreign key
        public decimal price { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
