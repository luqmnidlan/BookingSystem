namespace BookingSystem.Models
{
    public class EmployeeModel
    {
        public string Title { get; set; } = string.Empty;
        public List<Employee> Employees { get; set; } = new List<Employee>();


    }

    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = string.Empty;
        public int PhoneNumber { get; set; }
    }

}
