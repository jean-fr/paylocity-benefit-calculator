using Api.Repositories;
using Api.Repositories.Entities;
using Api.Services.Models;
using AutoMapper;

namespace Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDependentRepository _dependentRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IDependentRepository dependentRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _dependentRepository = dependentRepository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeResponse>> GetAllAsync()
        {
            var employeesResponse = new List<EmployeeResponse>();
            var employees = await _employeeRepository.GetAllAsync();
            foreach (Employee employee in employees)
            {
                employeesResponse.Add(await BuildEmployeeResponse(employee));
            }
            return employeesResponse;
        }

        public async Task<EmployeeResponse?> GetByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            var response = employee != null ? await BuildEmployeeResponse(employee) : null;
            return response;
        }

        private async Task<EmployeeResponse> BuildEmployeeResponse(Employee employee)
        {
            var employeeResponse = _mapper.Map<EmployeeResponse>(employee);

            var dependents = await _dependentRepository.GetByEmployeeIdAsync(employee.Id);

            foreach (var dependent in dependents)
            {
                var dependentResponse = _mapper.Map<DependentResponse>(dependent);
                dependentResponse.Relationship = dependent.Relationship.ToString();
                dependentResponse.EmployeeName = $"{employeeResponse.FirstName} {employeeResponse.LastName}";
                dependentResponse.EmployeeId = employeeResponse.Id;
                employeeResponse.Dependents.Add(dependentResponse);
            }
            return employeeResponse;
        }
        
    }
}
