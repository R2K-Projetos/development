using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class TelefoneViewModel
    {
        public int Id { get; set; }

        [Required]
        public int TipoTelefoneId { get; set; }

        public bool Principal { get; set; }

        [Required]
        [StringLength(80)]
        [Display(Name = "Número")]
        public string DddNumero { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;
    }
}
