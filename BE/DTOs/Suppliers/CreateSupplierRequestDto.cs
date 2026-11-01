namespace Inventory.Api.DTOs.Suppliers;

public class CreateSupplierRequestDto
{
    public string SupplierName { get; set; } = string.Empty;

    public string? Gstin { get; set; }

    public string? ContactName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? PaymentTerms { get; set; }
}
