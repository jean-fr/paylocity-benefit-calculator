using Api.Repositories;
using Api.Repositories.Entities;
using Api.Services.Models;
using AutoMapper;

namespace Api.Services
{
    public class DependentService : IDependentService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDependentRepository _dependentRepository;
        private readonly IMapper _mapper;
        public DependentService(IEmployeeRepository employeeRepository, IDependentRepository dependentRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _dependentRepository = dependentRepository;
            _mapper = mapper;
        }

        public async Task<List<DependentResponse>> GetAllAsync()
        {
            var result = new List<DependentResponse>();
            var dependents = await _dependentRepository.GetAllAsync();
            foreach (var dependent in dependents)
            {
                result.Add(await BuildDependentResponse(dependent));
            }
            return result;
        }

        public async Task<List<DependentResponse>> GetByEmployeeIdAsync(int employeeId)
        {
            var result = new List<DependentResponse>();
            var dependents = await _dependentRepository.GetByEmployeeIdAsync(employeeId);
            foreach (var dependent in dependents)
            {
                result.Add(await BuildDependentResponse(dependent));
            }
            return result;
        }

        public async Task<DependentResponse?> GetByIdAsync(int id)
        {
            var dependent = await _dependentRepository.GetByIdAsync(id);
            if (dependent == null) return null;

            return await BuildDependentResponse(dependent);
        }

        private async Task<DependentResponse> BuildDependentResponse(Dependent dependent)
        {
            var employee = await _employeeRepository.GetByIdAsync(dependent.EmployeeId);

            var response = _mapper.Map<DependentResponse>(dependent);

            if (employee != null)
            {
                response.EmployeeName = $"{employee.FirstName} {employee.LastName}";
                response.EmployeeId = employee.Id;
                response.Relationship = dependent.Relationship.ToString();
            }
            return response;
        }
    }

}
