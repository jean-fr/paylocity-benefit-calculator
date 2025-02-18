namespace Api.Dtos.Dependent;

public class GetDependentDto : GetEmployeeListDependentDto
{
    public string? EmployeeName { get; set; }
    public int EmployeeId { get; set; }
}
