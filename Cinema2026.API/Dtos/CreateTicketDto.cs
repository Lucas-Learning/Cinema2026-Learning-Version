namespace Cinema2026.API.Dtos
{
    //model = sådan ser data ud i databasen; DTO = sådan ser data ud på vej ind/ud af API'et.
    //DTO = Data Transfer Object
    public class CreateTicketDto
    {
        public int MovieId { get; set; }
        public int SeatId { get; set; }
        public int PersonId { get; set; }
    }
}
