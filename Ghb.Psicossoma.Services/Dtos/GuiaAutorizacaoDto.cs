using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class GuiaAutorizacaoDto : BaseDto
    {
        public int EncaminhamentoId { get; set; }
        public int GrupoGuiaId { get; set; }
        public string Numero { get; set; } = string.Empty;
        public DateTime DataEmissao { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
        public int TotalSessoes { get; set; }
        public bool Ativo { get; set; }
    }
}
