using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema2026.Repo.Models
{
    // En billet = én person har booket ét sæde til én afspilning.
    // Vi gemmer kun Id'er (fremmednøgler) i stedet for hele objekter — sådan
    // hænger tabeller sammen i en database.
    public class Ticket
    {
        public int Id { get; set; }
        public int ScreeningId { get; set; } // FK -> Screening (film + sal + tidspunkt)
        public int SeatId { get; set; }      // FK -> Seat
        public int PersonId { get; set; }    // FK -> Person
        public decimal price { get; set; }   // decimal (ikke double) pga. præcision ved penge
        public DateTime PurchaseDate { get; set; }
    }
}
