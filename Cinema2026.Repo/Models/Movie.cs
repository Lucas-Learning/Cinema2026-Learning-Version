using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema2026.Repo.Models
{
    // En film. Klassen svarer til tabellen "Movies" i databasen,
    // og hver property svarer til en kolonne.
    // En film har IKKE en sal — det er afspilningen (Screening), der kobler
    // film + sal + tidspunkt. Samme film kan derfor vises i flere sale.
    public class Movie
    {
        // EF Core bruger automatisk en property der hedder Id som primærnøgle.
        public int Id { get; set; } // variable / property
        public string title { get; set; }
        public int year { get; set; }
        public string genre { get; set; }

        // "?" betyder at værdien må være null — felterne er valgfrie, så
        // gamle film i databasen ikke skal have dem udfyldt.
        public string? Description { get; set; }
        public string? AgeRating { get; set; }
    }
}
