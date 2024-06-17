namespace ASP_WebApp.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int IdReservedResource { get; set; }
        public string? token { get; set; }

    }
}
