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

        public async Task<EmployeePaycheckResponse> GeneratePaycheckAsync(int employeeId)
        {
            var employee = await this.GetByIdAsync(employeeId);

            if (employee == null)
            {
                throw new Exception("NotFound");
            }

            // monthlyTotalCost * 12 to get yearly one
            // 365 days / 14 days (2 weeks) = 26 paychecks
            // a paycheck per 14 days

            // monthly total cost
            var monthlyTotalCost = CalculateTotalMonthlyCost(employee);

            var yearlyTotalCost = monthlyTotalCost * 12;// 12 months
            var yearlyNetSalary = employee.Salary - yearlyTotalCost;

            return new EmployeePaycheckResponse()
            {
                EmployeeId = employeeId,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                Amount = Math.Round(yearlyNetSalary / Constants.NumberOfPaycheckPerYear, 2),
                Cost = Math.Round(yearlyTotalCost / Constants.NumberOfPaycheckPerYear, 2)
            };
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


        private static decimal CalculateTotalMonthlyCost(EmployeeResponse employee)
        {
            // employees have a base cost of $1,000 per month(for benefits)
            var monthlyTotalCost = Constants.MonthlyBaseCost;

            // employees that make more than $80,000 per year will incur an additional 2% of their yearly salary in benefits costs
            if (employee.Salary > Constants.EmployeeMaximumSalaryToBeExempted)
            {
                // 2% of salary
                decimal yearlyCostOver80 = employee.Salary * 0.02m;
                decimal monthlyCostOver80 = yearlyCostOver80 / 12; // 12 months

                monthlyTotalCost += monthlyCostOver80;
            }

            // each dependent represents an additional $600 cost per month(for benefits)
            if (employee.Dependents.Any())
            {
                monthlyTotalCost += Constants.DependentMonthlyCost * employee.Dependents.Count;

                // dependents that are over 50 years old will incur an additional $200 per month

                foreach (var dependent in employee.Dependents)
                {
                    int age = (int)((DateTime.Now - dependent.DateOfBirth).TotalDays / Constants.AgeMagicNumber);

                    if (age > Constants.Age50)
                    {
                        monthlyTotalCost += Constants.DependentOver50MonthlyCost;
                    }
                }
            }

            return monthlyTotalCost;

        }
    }
}
