namespace Inventory.Api.DTOs.Suppliers;

public class SupplierResponseDto
{
    public int Id { get; set; }

    public string SupplierName { get; set; } = string.Empty;

    public string? Gstin { get; set; }

    public string? ContactName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? PaymentTerms { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
