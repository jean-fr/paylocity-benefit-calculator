using Api.Services.Models;

namespace Api.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponse>> GetAllAsync();
        Task<EmployeeResponse?> GetByIdAsync(int id);
    }
}
