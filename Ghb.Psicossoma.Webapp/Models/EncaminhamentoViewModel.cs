using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class EncaminhamentoViewModel
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Display(Name = "Especialidade")]
        public int EspecialidadeId { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Display(Name = "Plano de Saúde")]
        public int PlanoSaudeId { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Display(Name = "CID")]
        public int CidId { get; set; }
        public int EncaminhamentoOrigemId { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Display(Name = "Total de Sessões")]
        public int TotalSessoes { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Display(Name = "Nº máximo de Sessões")]
        public int MaximoSessoes { get; set; }

        public int SessoesRealizadas { get; set; }

        [Display(Name = "Particular")]
        public bool SolicitacaoMedica { get; set; }

        [Display(Name = "Observação")]
        public string Observacao { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }

        public string NomePaciente { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public string PlanoSaude { get; set; } = string.Empty;
        public string Convenio { get; set; } = string.Empty;
        public string CidCodigo { get; set; } = string.Empty;
        public string CidDescricao { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> OpcoesEspecialidade { get; set; } = [];
        public IEnumerable<SelectListItem> OpcoesPlanoSaude { get; set; } = [];
        public IEnumerable<SelectListItem> OpcoesCID { get; set; } = [];
    }
}
