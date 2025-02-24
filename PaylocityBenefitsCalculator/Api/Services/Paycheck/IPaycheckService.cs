using Api.Repositories.Entities;
using Api.Services.Models;

namespace Api.Services
{
    public interface IPaycheckService
    {
        Task<PaycheckResponse> GeneratePaycheckAsync(Employee employee);
    }
}
