using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Models
{
    public class AppointmentFeedbackModel
    {
        public int Id { get; set; }
        
        [Required]
        public int BookingId { get; set; }
        
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }
        
        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
        public string? Comment { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Finish, Cancel, Pending
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        // Navigation property
        public BookingModel? Booking { get; set; }
    }
}

