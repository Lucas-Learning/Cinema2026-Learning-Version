using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema2026.Repo.Models
{
    // En kunde. En billet peger på en Person via Ticket.PersonId.
    public class Person
    {
        public int Id { get; set; } // variable / property also primary key
        public string name { get; set; }
        public int age { get; set; }
    }
}
