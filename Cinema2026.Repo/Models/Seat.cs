using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema2026.Repo.Models
{
    // Et sæde hører til én sal. Række + nummer giver sædets navn, fx "B3".
    public class Seat
    {
        public int Id { get; set; }
        public int HallId { get; set; } // foreign key — fortæller hvilken sal sædet står i
        public char Row { get; set; }   // ét bogstav, fx 'B'
        public int Number { get; set; }
    }
}
