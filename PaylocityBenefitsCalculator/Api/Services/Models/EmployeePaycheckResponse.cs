namespace Api.Services.Models
{
    public class EmployeePaycheckResponse
    {
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public decimal Amount { get; set; }
        public decimal Cost { get; set; }

    }
}
