namespace Ghb.Psicossoma.Services.Dtos
{
    public class ConvenioResponseDto : ConvenioDto
    {
        public string? PlanoSaude { get; set; } = string.Empty;
        public string? PlanoConvenio { get; set; } = string.Empty;
        public string? ProdutoConvenio { get; set; } = string.Empty;
    }
}
