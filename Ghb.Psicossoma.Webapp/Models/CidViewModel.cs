using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class CidViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Display(Name = "Código")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "{0} deve possuir pelo menos {2} caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Display(Name = "Descrição")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "{0} deve possuir pelo menos {2} caracteres")]
        public string Nome { get; set; } = string.Empty;
    }
}
