namespace Cinema2026.API.Dtos
{
    // Det klienten sender ved login.
    public class LoginDto
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
