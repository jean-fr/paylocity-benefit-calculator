using Api.Services.Models;

namespace Api.Services
{
    public interface IDependentService
    {
        Task<List<DependentResponse>> GetAllAsync();
        Task<List<DependentResponse>> GetByEmployeeIdAsync(int employeeId);
        Task<DependentResponse?> GetByIdAsync(int id);
    }
}
