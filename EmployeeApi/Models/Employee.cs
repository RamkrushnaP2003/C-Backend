// Models/Employee.cs
namespace EmployeeApi.Models
{
    public class Employee
    {
        public required long Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public required string Department { get; set; }
    }
}
    