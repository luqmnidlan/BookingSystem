namespace BookingSystem.Models
{
    public class BookingModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; } = string.Empty;
        public string BarberName { get; set; } = string.Empty;
        public string SpecialRequests { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

