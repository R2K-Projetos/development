using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Endereço de email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Perfil { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;

        [Required]
        public bool Ativo { get; set; }
    }
}
