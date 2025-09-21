using Microsoft.AspNetCore.Mvc;
using BookingSystem.Models;
using BookingSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly ApplicationDbContext _context;

        public BookingController(ILogger<BookingController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get today's bookings for display
            var todayBookings = await GetTodayBookingsAsync();
            return View(todayBookings);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingModel booking)
        {
            try
            {
                // Validate the booking data
                if (booking == null)
                {
                    return Json(new { success = false, message = "Data tempahan tidak sah." });
                }

                // Check required fields
                if (string.IsNullOrWhiteSpace(booking.CustomerName))
                {
                    return Json(new { success = false, message = "Nama pelanggan diperlukan." });
                }

                if (string.IsNullOrWhiteSpace(booking.CustomerPhone))
                {
                    return Json(new { success = false, message = "Nombor telefon diperlukan." });
                }

                if (string.IsNullOrWhiteSpace(booking.ServiceType))
                {
                    return Json(new { success = false, message = "Jenis perkhidmatan diperlukan." });
                }

                if (booking.AppointmentDate == default(DateTime))
                {
                    return Json(new { success = false, message = "Tarikh janji temu diperlukan." });
                }

                if (string.IsNullOrWhiteSpace(booking.AppointmentTime))
                {
                    return Json(new { success = false, message = "Masa janji temu diperlukan." });
                }

                // Check if appointment date is not in the past
                if (booking.AppointmentDate < DateTime.Today)
                {
                    return Json(new { success = false, message = "Tarikh janji temu tidak boleh pada masa lalu." });
                }

                // Set default values
                booking.Status = "Pending";
                booking.CreatedAt = DateTime.Now;

                // Save to database
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                _logger.LogInformation("New booking created: {CustomerName} - {ServiceType} on {AppointmentDate} at {AppointmentTime}", 
                    booking.CustomerName, booking.ServiceType, booking.AppointmentDate, booking.AppointmentTime);

                return Json(new { 
                    success = true, 
                    message = "Janji temu berjaya ditempah! Kami akan menghubungi anda untuk pengesahan.",
                    bookingId = booking.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating booking");
                return Json(new { success = false, message = "Ralat berlaku semasa menempah janji temu. Sila cuba lagi." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTodayBookings()
        {
            try
            {
                var todayBookings = await GetTodayBookingsAsync();
                return Json(new { success = true, bookings = todayBookings });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving today's bookings via API");
                return Json(new { success = false, message = "Error retrieving bookings" });
            }
        }

        private async Task<List<BookingModel>> GetTodayBookingsAsync()
        {
            try
            {
                // Get today's bookings from database
                var todayBookings = await _context.Bookings
                    .Where(b => b.AppointmentDate.Date == DateTime.Today)
                    .OrderBy(b => b.AppointmentTime)
                    .ToListAsync();

                return todayBookings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving today's bookings");
                // Return empty list if database error
                return new List<BookingModel>();
            }
        }
    }
}
