using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class EnderecoViewModel
    {
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(10, MinimumLength = 9, ErrorMessage = "{0} deve possuir pelo menos {2} caracteres")]
        [Display(Name = "CEP")]
        public string CEP { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(80)]
        [Display(Name = "Logradouro")]
        public string Logradouro { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(20)]
        [Display(Name = "Número")]
        public string Numero { get; set; } = string.Empty;

        [StringLength(30)]
        [Display(Name = "Complemento")]
        public string Complemento { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(60)]
        [Display(Name = "Bairro")]
        public string Bairro { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Display(Name = "UF")]
        public IEnumerable<SelectListItem> Ufs { get; set; } = [];

        public bool Ativo { get; set; } = true;

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Display(Name = "Cidade")]
        public int CidadeId { get; set; }

        public int UFId { get; set; }
    }
}
