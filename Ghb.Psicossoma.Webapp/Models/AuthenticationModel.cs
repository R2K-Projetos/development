namespace Ghb.Psicossoma.Webapp.Models
{
    public class AuthenticationModel
    {
        public string Email { get; set; } = string.Empty;

        public string? Token { get; set; }

        public DateTime TokenExpiration { get; set; }
    }
}
