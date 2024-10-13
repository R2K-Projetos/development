using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class EnderecoViewModel
    {
        [Required]
        [StringLength(10)]
        [Display(Name = "CEP")]
        public string Cep { get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        [Display(Name = "Logradouro")]
        public string Logradouro { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Display(Name = "Número")]
        public string Numero { get; set; } = string.Empty;

        [StringLength(30)]
        [Display(Name = "Complemento")]
        public string Complemento { get; set; } = string.Empty;

        [Required]
        [StringLength(60)]
        [Display(Name = "Bairro")]
        public string Bairro { get; set; } = string.Empty;

        [Display(Name = "Estado")]
        public IEnumerable<SelectListItem> Ufs { get; set; } = [];

        [Required]
        public int UfId { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
