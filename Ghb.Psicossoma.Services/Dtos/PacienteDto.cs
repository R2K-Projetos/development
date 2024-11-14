namespace Ghb.Psicossoma.Services.Dtos
{
    public class PacienteDto : PessoaDto
    {
        public int PessoaId { get; set; }

        public bool IsAtivo { get; set; }
    }
}
