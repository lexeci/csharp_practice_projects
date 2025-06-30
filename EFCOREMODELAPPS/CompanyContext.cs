using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCoreModelApp
{
    public class HRDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
            .UseSqlite("Data Source=Company-db")
            .ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning)
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity
                    .HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DepartmentID);
            });

            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentID = 1, Name = "HR" },
                new Department { DepartmentID = 2, Name = "Engineering" }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeID = 1,
                    FirstName = "Aiko",
                    LastName = "Tanaka",
                    HireDate = DateTime.Now,
                    DepartmentID = 1
                },
                new Employee
                {
                    EmployeeID = 2,
                    FirstName = "Zainab",
                    LastName = "Al-Farsi",
                    HireDate = DateTime.Now,
                    DepartmentID = 2
                }
            );
        }
    }
}