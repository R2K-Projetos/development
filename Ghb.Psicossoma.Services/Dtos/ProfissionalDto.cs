using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class ProfissionalDto : BaseDto
    {
        public int PessoaId { get; set; }

        public int RegistroProfissionalId { get; set; }

        public string Numero { get; set; } = string.Empty;

        public bool Ativo { get; set; } = false;
    }
}
