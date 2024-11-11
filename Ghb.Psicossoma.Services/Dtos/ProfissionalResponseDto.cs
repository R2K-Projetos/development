namespace Ghb.Psicossoma.Services.Dtos
{
    public class ProfissionalResponseDto : PessoaDto
    {
        public int PessoaId { get; set; }

        public string Numero { get; set; } = string.Empty;

        public string RegistroProfissional { get; set; } = string.Empty;

        public bool IsAtivo { get; set; } = false;

        public string Especialidades { get; set; } = string.Empty;
    }
}
