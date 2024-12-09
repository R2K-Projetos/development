using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class TelefoneDto : BaseDto
    {
        public int PessoaId { get; set; }
        public int TipoTelefoneId { get; set; }
        public string DDDNumero { get; set; } = string.Empty;
        public bool Principal { get; set; }
        public bool Ativo { get; set; }
    }
}
