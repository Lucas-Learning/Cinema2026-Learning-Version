using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema2026.Repo.Models
{
    // En biografsal. Sæder peger tilbage på salen via Seat.HallId.
    public class Hall
    {
        public int id { get; set; }
        public string Name { get; set; } = ""; // = "" undgår null-advarsler
    }
}
