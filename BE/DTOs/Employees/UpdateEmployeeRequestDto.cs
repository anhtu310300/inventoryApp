namespace Inventory.Api.DTOs.Employees;

public class UpdateEmployeeRequestDto
{
    public string UserName { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string Role { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime? StartDate { get; set; }

    public string? Notes { get; set; }

    public string? Password { get; set; }
}
