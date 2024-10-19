using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class TelefoneViewModel
    {
        public int Id { get; set; }

        public int PessoaId { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public int TipoTelefoneId { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "{0} deve possuir pelo menos {2} caracteres")]
        [Display(Name = "Número")]
        public string DDDNum { get; set; } = string.Empty;

        public bool Principal { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
