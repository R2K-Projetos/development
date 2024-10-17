using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class PacienteViewModel : PessoaViewModel
    {
        public int PessoaId { get; set; }

        [Display(Name = "Perfil de Acesso")]
        public string? PerfilUsuario { get; set; }

        [Display(Name = "Status de Acesso")]
        public string? StatuslUsuario { get; set; }

        public bool IsAtivo { get; set; }
    }
}
