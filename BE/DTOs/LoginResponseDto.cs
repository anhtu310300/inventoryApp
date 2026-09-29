namespace Inventory.Api.DTOs
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string TokenType { get; set; } = "Bearer";

        public int UserId { get; set; }

        public string EmployeeCode { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
