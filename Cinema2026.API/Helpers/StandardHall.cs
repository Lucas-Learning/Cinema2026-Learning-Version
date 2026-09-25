using Cinema2026.Repo.Models;

namespace Cinema2026.API.Helpers
{
    // Standard-opsætningen af en sal: 3 rækker (A-C) med 6 sæder i hver = 18 sæder.
    // Layoutet er defineret ÉT sted her, og bruges både når en sal oprettes
    // (HallController) og af seed'en i Program.cs — så de aldrig kommer ud af trit.
    // "static" = man kalder metoderne direkte på klassen uden at oprette et objekt.
    public static class StandardHall
    {
        // readonly bruges til arrayet (selve listen kan ikke byttes ud); const bruges
        // til simple værdier som tal og tekst.
        public static readonly char[] Rows = { 'A', 'B', 'C' };
        public const int SeatsPerRow = 6;

        // Alle sæder til en sal. hallId skal være kendt (salen skal være gemt først),
        // fordi hvert sæde peger på sin sal via Seat.HallId.
        public static List<Seat> NewSeats(int hallId)
        {
            var seats = new List<Seat>();
            foreach (var row in Rows)
            {
                for (int number = 1; number <= SeatsPerRow; number++)
                {
                    seats.Add(new Seat { HallId = hallId, Row = row, Number = number });
                }
            }
            return seats;
        }
    }
}
