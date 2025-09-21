namespace BookingSystem.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "A";  
        public int PhoneNumber { get; set; }

    }
}
