namespace Cinema2026.API.Dtos
{
    //model = sådan ser data ud i databasen; DTO = sådan ser data ud på vej ind/ud af API'et.
    //DTO = Data Transfer Object
    // Kun de felter klienten må bestemme. Id, price og PurchaseDate mangler
    // med vilje — dem sætter serveren selv, så de ikke kan forfalskes.
    public class CreateTicketDto
    {
        public int ScreeningId { get; set; } // hvilken afspilning (film + sal + tid)
        public int SeatId { get; set; }
        public int PersonId { get; set; }
    }
}
