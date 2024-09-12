using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class CidViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Código")]
        [StringLength(10)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Código")]
        [StringLength(200)]
        public string Descricao { get; set; } = string.Empty;
    }
}
