using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class PlanoSaudeViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome")]
        [StringLength(40)]
        public string Descricao { get; set; } = string.Empty;
    }
}
