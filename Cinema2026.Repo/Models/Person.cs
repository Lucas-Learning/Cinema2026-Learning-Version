using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Cinema2026.Repo.Models
{
    // En kunde. En billet peger på en Person via Ticket.PersonId.
    public class Person
    {
        public int Id { get; set; } // variable / property also primary key
        public string name { get; set; }
        public int age { get; set; }

        // Login-felter. Nullable ("?"), fordi kunder oprettet FØR login fandtes
        // ikke har dem — de kan så bare ikke logge ind.
        public string? Username { get; set; }

        // Vi gemmer ALDRIG selve adgangskoden — kun en "hash": en envejs-omregning,
        // som ikke kan regnes tilbage til koden. Ved login hasher vi det indtastede
        // og sammenligner med det gemte.
        // [JsonIgnore] betyder, at feltet aldrig sendes ud af API'et som JSON
        // (og ignoreres, hvis nogen prøver at sende det ind). Så lækker ingen
        // af de eksisterende Person-endpoints hashen.
        [JsonIgnore]
        public string? PasswordHash { get; set; }

        // Admin-flag: kun admins kan komme ind på /admin i frontend. Det kan ikke
        // sættes via register (det er ikke med i RegisterDto) — kun via seed'en i Program.cs.
        public bool IsAdmin { get; set; }
    }
}
