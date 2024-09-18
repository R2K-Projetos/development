using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class FuncionalidadeViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome")]
        [StringLength(60)]
        public string Nome { get; set; } = string.Empty;
    }
}
