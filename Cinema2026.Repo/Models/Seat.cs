using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema2026.Repo.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int HallId { get; set; } // foreign key
        public char Row { get; set; }
        public int Number { get; set; }
    }
}
