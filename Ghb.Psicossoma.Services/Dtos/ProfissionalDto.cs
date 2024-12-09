namespace Ghb.Psicossoma.Services.Dtos
{
    public class ProfissionalDto : PessoaDto
    {
        public int PessoaId { get; set; }
        public int RegistroProfissionalId { get; set; }
        public int UFId { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string CNS { get; set; } = string.Empty;
        public bool IsAtivo { get; set; } = false;
    }
}
