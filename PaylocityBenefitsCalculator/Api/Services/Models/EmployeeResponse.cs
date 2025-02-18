namespace Api.Services.Models;

public class EmployeeResponse : ServiceResponseBase
{
    public decimal Salary { get; set; }
    public List<DependentResponse> Dependents { get; set; } = new List<DependentResponse>();
}
