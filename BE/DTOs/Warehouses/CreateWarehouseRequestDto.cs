namespace Inventory.Api.DTOs.Warehouses;

public class CreateWarehouseRequestDto
{
    public string WarehouseName { get; set; } = string.Empty;

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }
}
