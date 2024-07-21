using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class PacienteDto : BaseDto
    {
        public int PessoaId { get; set; }

        public bool Ativo { get; set; }
    }
}
