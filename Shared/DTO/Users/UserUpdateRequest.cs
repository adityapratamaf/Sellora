namespace Shared.DTO.Users;

public class UserUpdateRequest
{
    public string Name { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Password { get; set; }  // optional update password
    public string Address { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Role { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
