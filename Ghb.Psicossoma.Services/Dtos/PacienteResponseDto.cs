namespace Ghb.Psicossoma.Services.Dtos
{
    public class PacienteResponseDto : PessoaDto
    {
        public int PessoaId { get; set; }

        public bool IsAtivo { get; set; }

        public string? PerfilUsuario { get; set; }

        public string? StatuslUsuario { get; set; }
    }
}
