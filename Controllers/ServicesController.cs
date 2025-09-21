using Microsoft.AspNetCore.Mvc;
using BookingSystem.Models;
using BookingSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ILogger<ServicesController> _logger;
        private readonly ApplicationDbContext _context;

        public ServicesController(ILogger<ServicesController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var services = await GetServicesAsync();
            return View(services);
        }

        [HttpGet]
        public async Task<IActionResult> GetServices()
        {
            try
            {
                var services = await GetServicesAsync();
                return Json(new { success = true, services = services });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving services via API");
                return Json(new { success = false, message = "Error retrieving services" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceModel service)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(service.Name) || string.IsNullOrEmpty(service.Category))
                {
                    return Json(new { success = false, message = "Nama dan kategori perkhidmatan diperlukan." });
                }

                // Set default values
                service.IsActive = true;

                // Save to database
                _context.Services.Add(service);
                await _context.SaveChangesAsync();

                _logger.LogInformation("New service created: {Name} - {Category}", service.Name, service.Category);

                return Json(new { 
                    success = true, 
                    message = "Perkhidmatan berjaya ditambah!",
                    serviceId = service.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating service");
                return Json(new { success = false, message = "Ralat berlaku semasa menambah perkhidmatan. Sila cuba lagi." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] ServiceModel service)
        {
            try
            {
                var existingService = await _context.Services.FindAsync(service.Id);
                if (existingService == null)
                {
                    return Json(new { success = false, message = "Perkhidmatan tidak ditemui." });
                }

                existingService.Name = service.Name;
                existingService.Description = service.Description;
                existingService.Price = service.Price;
                existingService.Duration = service.Duration;
                existingService.Category = service.Category;
                existingService.IsActive = service.IsActive;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Service updated: {Name} - {Category}", service.Name, service.Category);

                return Json(new { success = true, message = "Perkhidmatan berjaya dikemas kini!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating service");
                return Json(new { success = false, message = "Ralat berlaku semasa mengemas kini perkhidmatan. Sila cuba lagi." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var service = await _context.Services.FindAsync(id);
                if (service == null)
                {
                    return Json(new { success = false, message = "Perkhidmatan tidak ditemui." });
                }

                _context.Services.Remove(service);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Service deleted: {Name}", service.Name);

                return Json(new { success = true, message = "Perkhidmatan berjaya dipadam!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting service");
                return Json(new { success = false, message = "Ralat berlaku semasa memadam perkhidmatan. Sila cuba lagi." });
            }
        }

        private async Task<List<ServiceModel>> GetServicesAsync()
        {
            try
            {
                var services = await _context.Services
                    .OrderBy(s => s.Name)
                    .ToListAsync();

                return services;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving services");
                return new List<ServiceModel>();
            }
        }
    }
}
