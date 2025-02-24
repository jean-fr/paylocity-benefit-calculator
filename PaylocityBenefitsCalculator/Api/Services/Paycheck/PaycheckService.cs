using Api.Repositories;
using Api.Repositories.Entities;
using Api.Services.Models;
using AutoMapper;

namespace Api.Services
{
    public class PaycheckService : IPaycheckService
    {
        private readonly IDependentRepository _dependentRepository;
        private readonly IMapper _mapper;

        public PaycheckService(IDependentRepository dependentRepository, IMapper mapper)
        {
            _dependentRepository = dependentRepository;
            _mapper = mapper;
        }

        public async Task<PaycheckResponse> GeneratePaycheckAsync(Employee employee)
        {
            // monthlyTotalCost * 12 to get yearly one
            // 365 days / 14 days (2 weeks) = 26 paychecks
            // a paycheck per 14 days

            // monthly total cost
            var monthlyTotalCost = await CalculateTotalMonthlyCost(employee);

            var yearlyTotalCost = monthlyTotalCost * 12;// 12 months
            var yearlyNetSalary = employee.Salary - yearlyTotalCost;

            return new PaycheckResponse()
            {
                EmployeeId = employee.Id,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                Amount = Math.Round(yearlyNetSalary / Constants.NumberOfPaycheckPerYear, 2),
                Cost = Math.Round(yearlyTotalCost / Constants.NumberOfPaycheckPerYear, 2)
            };
        }
        private  async Task<decimal> CalculateTotalMonthlyCost(Employee employee)
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

            var dependents = await _dependentRepository.GetByEmployeeIdAsync(employee.Id);

            // each dependent represents an additional $600 cost per month(for benefits)
            if (dependents.Any())
            {
                monthlyTotalCost += Constants.DependentMonthlyCost * dependents.Count;

                // dependents that are over 50 years old will incur an additional $200 per month

                foreach (var dependent in dependents)
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
