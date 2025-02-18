namespace Api.Repositories.Entities;

public class Dependent : EntityBase
{
    public Relationship Relationship { get; set; }
    public int EmployeeId { get; set; }
}
