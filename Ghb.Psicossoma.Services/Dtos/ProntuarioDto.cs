using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class ProntuarioDto :BaseDto
    {
        public int EncaminhamentoId { get; set; }
        public int ProfissionalId { get; set; }
        public int LaudoAnamneseId { get; set; }
        public DateTime DataEntrada { get; set; }
        public bool Ativo { get; set; }
    }
}
