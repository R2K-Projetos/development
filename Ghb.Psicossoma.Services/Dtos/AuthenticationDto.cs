
namespace Ghb.Psicossoma.Services.Dtos;

public class AuthenticationDto : UserDto
{
    public string Email { get; set; } = string.Empty;

    public string? Token { get; set; }

    public DateTime TokenExpiration { get; set; }
}
