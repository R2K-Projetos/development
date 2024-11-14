namespace Ghb.Psicossoma.Services.Dtos
{
    public class ProfissionalDto : PessoaDto
    {
        public int PessoaId { get; set; }

        public int RegistroProfissionalId { get; set; }

        public string Numero { get; set; } = string.Empty;

        public bool IsAtivo { get; set; } = false;
    }
}
