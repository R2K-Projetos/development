using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class ProfissionalViewModel : PessoaViewModel
    {
        [Required]
        [StringLength(10)]
        public string Numero { get; set; } = string.Empty;

        public bool IsAtivo { get; set; } = false;
    }
}
