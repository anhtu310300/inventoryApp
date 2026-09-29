namespace Inventory.Api.DTOs;

using System.ComponentModel.DataAnnotations;

public class CreateEmployeeDto
{
    public string EmployeeCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Department { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
    public DateTime? StartDate { get; set; }
    public string? Notes { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
}
