using Microsoft.AspNetCore.Mvc;
using BookingSystem.Models;
using BookingSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Controllers
{
    public class EmployeeSetupController : Controller
    {
        private readonly ILogger<EmployeeSetupController> _logger;
        private readonly ApplicationDbContext _context;

        public EmployeeSetupController(ILogger<EmployeeSetupController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await GetEmployeesAsync();
            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await GetEmployeesAsync();
                return Json(new { success = true, employees = employees });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employees via API");
                return Json(new { success = false, message = "Error retrieving employees" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EmployeeModel employee, IFormFile imageFile)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(employee.Name) || string.IsNullOrEmpty(employee.Role))
                {
                    return Json(new { success = false, message = "Nama dan jawatan diperlukan." });
                }

                // Handle image upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    
                    employee.ImageUrl = $"/files/{fileName}";
                }

                // Set default values
                employee.Status = employee.Status ?? "Aktif";
                employee.CreatedAt = DateTime.Now;

                // Save to database
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                _logger.LogInformation("New employee created: {Name} - {Role}", employee.Name, employee.Role);

                return Json(new { 
                    success = true, 
                    message = "Ahli kakitangan berjaya ditambah!",
                    employeeId = employee.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                return Json(new { success = false, message = "Ralat berlaku semasa menambah ahli kakitangan. Sila cuba lagi." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] EmployeeModel employee, IFormFile imageFile)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(employee.Name) || string.IsNullOrEmpty(employee.Role))
                {
                    return Json(new { success = false, message = "Nama dan jawatan diperlukan." });
                }

                // Find existing employee
                var existingEmployee = await _context.Employees.FindAsync(employee.Id);
                if (existingEmployee == null)
                {
                    return Json(new { success = false, message = "Ahli kakitangan tidak ditemui." });
                }

                // Handle image upload if new image is provided
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    
                    employee.ImageUrl = $"/files/{fileName}";
                }

                // Update properties
                existingEmployee.Name = employee.Name;
                existingEmployee.Role = employee.Role;
                existingEmployee.Phone = employee.Phone;
                existingEmployee.Email = employee.Email;
                existingEmployee.Specialty = employee.Specialty;
                existingEmployee.Experience = employee.Experience;
                existingEmployee.Status = employee.Status;
                existingEmployee.StartDate = employee.StartDate;
                existingEmployee.Notes = employee.Notes;
                
                // Only update image URL if a new image was uploaded
                if (imageFile != null && imageFile.Length > 0)
                {
                    existingEmployee.ImageUrl = employee.ImageUrl;
                }

                // Save changes
                _context.Employees.Update(existingEmployee);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Employee updated: {Name} - {Role}", employee.Name, employee.Role);

                return Json(new { 
                    success = true, 
                    message = "Ahli kakitangan berjaya dikemas kini!"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee");
                return Json(new { success = false, message = "Ralat berlaku semasa mengemas kini ahli kakitangan. Sila cuba lagi." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return Json(new { success = false, message = "Ahli kakitangan tidak ditemui." });
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Employee deleted: {Name}", employee.Name);

                return Json(new { 
                    success = true, 
                    message = "Ahli kakitangan berjaya dipadam!"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee");
                return Json(new { success = false, message = "Ralat berlaku semasa memadam ahli kakitangan. Sila cuba lagi." });
            }
        }

        private async Task<List<EmployeeModel>> GetEmployeesAsync()
        {
            try
            {
                var employees = await _context.Employees
                    .OrderBy(e => e.Name)
                    .ToListAsync();

                return employees;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employees");
                return new List<EmployeeModel>();
            }
        }
    }
}
