namespace Api.Services.Models;

public class DependentResponse : ServiceResponseBase
{
    public string? Relationship { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
}
