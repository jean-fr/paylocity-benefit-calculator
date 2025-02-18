using Api.Repositories.Entities;

namespace Api.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        // for the sake of this test, considering this as a datasource/DB
        private readonly List<Employee> _employees = new()
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
            },
            new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
            }
        };

        public async Task<List<Employee>> GetAllAsync()
        {
            return await Task.Run(() => _employees);
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await Task.Run(() => _employees.FirstOrDefault(e => e.Id == id));
        }
    }
}
