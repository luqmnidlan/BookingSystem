using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Service name is required")]
        [StringLength(100, ErrorMessage = "Service name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000.00, ErrorMessage = "Price must be between $0.01 and $1000.00")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(5, 300, ErrorMessage = "Duration must be between 5 and 300 minutes")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        public string Category { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }
    }
}

