using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class ProfissionalResponseDto : BaseDto
    {
        public int PessoaId { get; set; }

        public string RegistroProfissional { get; set; } = string.Empty;

        public string Nome { get; set; } = string.Empty;

        public string Numero { get; set; } = string.Empty;

        public bool Ativo { get; set; } = false;
    }
}
