namespace ASP_WebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Date { get; set; }
        public string Destination { get; set; }
        public string Password { get; set; }
        public string? Token { get; set; }
    }
}
