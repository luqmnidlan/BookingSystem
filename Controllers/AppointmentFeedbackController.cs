using Microsoft.AspNetCore.Mvc;
using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentFeedbackController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AppointmentFeedbackController> _logger;

        public AppointmentFeedbackController(ApplicationDbContext context, ILogger<AppointmentFeedbackController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitFeedback([FromBody] AppointmentFeedbackModel feedback)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data provided" });
                }

                // Check if booking exists
                var booking = await _context.Bookings.FindAsync(feedback.BookingId);
                if (booking == null)
                {
                    return NotFound(new { success = false, message = "Booking not found" });
                }

                // Check if feedback already exists for this booking
                var existingFeedback = await _context.AppointmentFeedbacks
                    .FirstOrDefaultAsync(f => f.BookingId == feedback.BookingId);

                if (existingFeedback != null)
                {
                    // Update existing feedback
                    existingFeedback.Rating = feedback.Rating;
                    existingFeedback.Comment = feedback.Comment;
                    existingFeedback.Status = feedback.Status;
                    existingFeedback.UpdatedAt = DateTime.Now;
                    
                    _context.AppointmentFeedbacks.Update(existingFeedback);
                }
                else
                {
                    // Create new feedback
                    feedback.CreatedAt = DateTime.Now;
                    feedback.UpdatedAt = DateTime.Now;
                    _context.AppointmentFeedbacks.Add(feedback);
                }

                // Update booking status
                booking.Status = feedback.Status;
                _context.Bookings.Update(booking);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Feedback submitted for booking {BookingId}: Rating {Rating}, Status {Status}", 
                    feedback.BookingId, feedback.Rating, feedback.Status);

                return Ok(new { success = true, message = "Feedback submitted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting feedback for booking {BookingId}", feedback.BookingId);
                return StatusCode(500, new { success = false, message = "An error occurred while submitting feedback" });
            }
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetFeedbackByBooking(int bookingId)
        {
            try
            {
                var feedback = await _context.AppointmentFeedbacks
                    .FirstOrDefaultAsync(f => f.BookingId == bookingId);

                if (feedback == null)
                {
                    return NotFound(new { success = false, message = "No feedback found for this booking" });
                }

                return Ok(new { success = true, feedback = feedback });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving feedback for booking {BookingId}", bookingId);
                return StatusCode(500, new { success = false, message = "An error occurred while retrieving feedback" });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllFeedback()
        {
            try
            {
                var feedbacks = await _context.AppointmentFeedbacks
                    .Include(f => f.Booking)
                    .OrderByDescending(f => f.CreatedAt)
                    .ToListAsync();

                return Ok(new { success = true, feedbacks = feedbacks });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all feedback");
                return StatusCode(500, new { success = false, message = "An error occurred while retrieving feedback" });
            }
        }
    }
}
