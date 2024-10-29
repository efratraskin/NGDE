namespace MyMvcApp.Models
{
    public class Employee
    {
        public required int EmployeeID { get; set; }
        public required string EmployeeName { get; set; }
        public required string Department { get; set; }
        public required string DateOfJoining { get; set; }
        public required string PhotoFileName { get; set; }
    }
}
