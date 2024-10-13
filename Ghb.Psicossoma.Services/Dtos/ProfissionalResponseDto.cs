using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class ProfissionalResponseDto : PessoaDto //BaseDto
    {
        public int PessoaId { get; set; }

        public string Numero { get; set; } = string.Empty;

        public string DddNumero { get; set; } = string.Empty;

        public string RegistroProfissional { get; set; } = string.Empty;

        public string Especialidade { get; set; } = string.Empty;

        public bool Ativo { get; set; } = false;
    }
}
