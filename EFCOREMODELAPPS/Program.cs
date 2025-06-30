using Microsoft.EntityFrameworkCore;
using EFCoreModelApp;

class Program
{
    static void Main()
    {
        using (var context = new HRDbContext())
        {
            var allEmployees = context.Employees.Include(e => e.Department).ToList();
            foreach (var emp in allEmployees)
            {
                Console.WriteLine($"{emp.FirstName} {emp.LastName} - {emp.Department?.Name ?? "N/A"}");
            }

            var hrEmployees = context.Employees
                                      .Include(e => e.Department)
                                      .Where(e => e.Department.Name == "HR")
                                      .ToList();
            Console.WriteLine("HR Department Employees:");
            foreach (var emp in hrEmployees)
            {
                Console.WriteLine($"{emp.FirstName} {emp.LastName}");
            }

            var newEmployee = new Employee
            {
                FirstName = "New",
                LastName = "Employee",
                HireDate = DateTime.Now,
                DepartmentID = 2
            };
            context.Employees.Add(newEmployee);
            context.SaveChanges();

            Console.WriteLine("New employee added.");
        }
    }
}