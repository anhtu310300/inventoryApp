namespace Inventory.Api.DTOs.Warehouses;

public class WarehouseResponseDto
{
    public int Id { get; set; }

    public string WarehouseName { get; set; } = string.Empty;

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
