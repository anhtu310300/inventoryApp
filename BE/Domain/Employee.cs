using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Api.Domain;

[Table("employees")]
public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("userName")]
    public string UserName { get; set; } = string.Empty;

    [Column("full_name")]
    public string FullName { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("phone")]
    public string? Phone { get; set; }

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

    [Column("update_at")]
    public DateTime UpdatedAt { get; set; }
}
