namespace Cinema2026.API.Dtos
{
    // Det klienten sender, når en ny kunde opretter sig.
    // Password kommer ind som klartekst her (over HTTPS), men gemmes kun som hash.
    public class RegisterDto
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
