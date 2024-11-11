using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class EspecialidadeViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome")]
        [StringLength(40)]
        public string Nome { get; set; } = string.Empty;

        public string Checked { get; set; } = string.Empty;
    }
}
