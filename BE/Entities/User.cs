using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Api.Entities;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("employee_code")]
    public string EmployeeCode { get; set; } = string.Empty;

    [Column("full_name")]
    public string FullName { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("phone")]
    public string? Phone { get; set; }

    [Column("department")]
    public string Department { get; set; } = string.Empty;

    [Column("role")]
    public string Role { get; set; } = string.Empty;

    [Column("status")]
    public string Status { get; set; } = "Active";

    [Column("start_date")]
    public DateTime? StartDate { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("password_hash")]
    public string? PasswordHash { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
