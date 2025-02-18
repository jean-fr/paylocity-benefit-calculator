
using Api.Repositories.Entities;

namespace Api.Repositories
{
    public class DependentRepository : IDependentRepository
    {
        // for the sake of this test, considering this as a datasource
        private readonly List<Dependent> _dependents = new()
        {
           new()
            {
                Id = 1,
                FirstName = "Spouse",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1998, 3, 3),
                EmployeeId=2
            },
            new()
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23),
                EmployeeId=2
            },
            new()
            {
                Id = 3,
                FirstName = "Child2",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2021, 5, 18),
                EmployeeId=2
            },
            new()
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2),
                EmployeeId=3
            }
        };

        public async Task<List<Dependent>> GetAllAsync()
        {
            return await Task.Run(() => _dependents);
        }

        public async Task<List<Dependent>> GetByEmployeeIdAsync(int employeeId)
        {
            return await Task.Run(() => _dependents.FindAll(d => d.EmployeeId == employeeId));
        }

        public async Task<Dependent?> GetByIdAsync(int id)
        {
            return await Task.Run(() => _dependents.FirstOrDefault(e => e.Id == id));
        }
    }
}
