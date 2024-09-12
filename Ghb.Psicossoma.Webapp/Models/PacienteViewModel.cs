using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class PacienteViewModel
    {
        public int Id { get; set; }

        public int PessoaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Display(Name = "Nome Reduzido")]
        [StringLength(40)]
        public string NomeReduzido { get; set; } = string.Empty;

        [Required]
        [Display(Name = "CPF")]
        [StringLength(14)]
        public string Cpf { get; set; } = string.Empty;

        [Required]
        [StringLength(1)]
        public string Sexo { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> OpcoesSexo { get; set; } = [];

        [Required]
        [Display(Name = "E-mail")]
        [StringLength(80)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }

        public bool Ativo { get; set; }

        [Display(Name = "Perfil de Acesso")]
        public string? PerfilUsuario { get; set; }

        [Display(Name = "Status de Acesso")]
        public string? StatuslUsuario { get; set; }
    }
}
