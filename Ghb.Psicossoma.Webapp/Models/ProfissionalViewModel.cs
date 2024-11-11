using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class ProfissionalViewModel : PessoaViewModel
    {
        public int PessoaId { get; set; } 

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Display(Name = "Nº do Registro")]
        [StringLength(10)]
        public string Numero { get; set; } = string.Empty;

        [Display(Name = "Registro")]
        public string RegistroProfissional { get; set; } = string.Empty;

        public bool IsAtivo { get; set; } = false;

        public string Especialidades { get; set; } = string.Empty;

        public int RegistroProfissionalId { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Display(Name = "Tipo de Registro")]
        public IEnumerable<SelectListItem> TiposRegistroProfissional { get; set; } = [];

        public List<EspecialidadeViewModel>? EspecialidadesProfissional { get; set; }
    }
}
