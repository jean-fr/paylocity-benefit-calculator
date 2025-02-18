using Api.Repositories.Entities;

namespace Api.Repositories
{
    public interface IDependentRepository
    {
        Task<List<Dependent>> GetAllAsync();
        Task<List<Dependent>> GetByEmployeeIdAsync(int employeeId);
        Task<Dependent?> GetByIdAsync(int id);
    }
}
