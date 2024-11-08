using Microsoft.AspNetCore.Mvc.Rendering;
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

        [Display(Name = "Ativo")]
        public bool IsAtivo { get; set; }

        public GrauParentescoViewModel? GrauParentesco { get; set; }

        [Display(Name = "Tipo de Parentesco")]
        public IEnumerable<SelectListItem> TipoDeParentesco { get; set; } = [];
    }
}
