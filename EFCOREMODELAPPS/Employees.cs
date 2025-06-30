namespace EFCoreModelApp
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime HireDate { get; set; }
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
    }
}