namespace Inventory.Api.Common.Responses;

public class ApiResponse<T>
{
    public T? Data { get; set; }

    public List<string> Messages { get; set; } = new();
}
