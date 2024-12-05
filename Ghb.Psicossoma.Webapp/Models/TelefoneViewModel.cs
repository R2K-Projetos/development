using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class TelefoneViewModel
    {
        public int Id { get; set; }

        public int PessoaId { get; set; }

        public int TipoTelefoneId { get; set; }

        public bool Principal { get; set; }

        [Display(Name = "Número")]
        public string DDDNumero { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        public string? TipoTelefone { get; set; } = string.Empty;

        [Display(Name = "Tipo")]
        public IEnumerable<SelectListItem> TiposTelefone { get; set; } = [];
    }
}
