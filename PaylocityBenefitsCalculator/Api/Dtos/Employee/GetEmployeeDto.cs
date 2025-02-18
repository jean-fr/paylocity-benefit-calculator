using Api.Dtos.Dependent;

namespace Api.Dtos.Employee;

public class GetEmployeeDto : DtoBase
{
    public decimal Salary { get; set; }
    public List<GetEmployeeListDependentDto> Dependents { get; set; } = new List<GetEmployeeListDependentDto>();
}
