using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class PessoaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Display(Name = "Nome Reduzido")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "{0} deve possuir pelo menos {2} caracteres")]
        public string NomeReduzido { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Display(Name = "CPF")]
        [StringLength(20)]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(1)]
        [Display(Name = "Sexo")]
        public string Sexo { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> OpcoesSexo { get; set; } = [];

        [StringLength(80)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Display(Name = "Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }

        public bool Ativo { get; set; }

        public EnderecoViewModel? Endereco { get; set; }

        public TelefoneViewModel? Telefone { get; set; }

        public List<TelefoneViewModel>? TelefonesPessoa { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Display(Name = "Tipo")]
        public IEnumerable<SelectListItem> TiposTelefone { get; set; } = [];

        public int TipoTelefoneId { get; set; }

        [Display(Name = "Tipo de Parentesco")]
        public IEnumerable<SelectListItem> TipoDeParentesco { get; set; } = [];
    }
}
