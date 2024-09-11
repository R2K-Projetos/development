using System.ComponentModel.DataAnnotations;

namespace Ghb.Psicossoma.Webapp.Models
{
    public class ConvenioViewModel
    {
        public int Id { get; set; }
        public int PlanoSaudeId { get; set; }
        public int PlanoConvenioId { get; set; }
        public int ProdutoConvenioId { get; set; }

        [Display(Name = "Identificação")]
        [StringLength(30)]
        public string Identificacao { get; set; } = string.Empty;

        [Display(Name = "Acomodação")]
        [StringLength(20)]
        public string Acomodacao { get; set; } = string.Empty;

        [Display(Name = "CNS")]
        [StringLength(30)]
        public string Cns { get; set; } = string.Empty;
        public string Cobertura { get; set; } = string.Empty;
        public string Empresa { get; set; } = string.Empty;
        public bool Ativo { get; set; }

        [Display(Name = "Plano de Saúde")]
        [StringLength(40)]
        public string PlanoSaude { get; set; } = string.Empty;

        [Display(Name = "Plano do Convênio")]
        [StringLength(40)]
        public string PlanoConvenio { get; set; } = string.Empty;

        [Display(Name = "Produto Convênio")]
        [StringLength(20)]
        public string ProdutoConvenio { get; set; } = string.Empty;
    }
}
