namespace Ghb.Psicossoma.Domains.Entities
{
    public class ConvenioResponse : Convenio
    {
        public string? PlanoSaude { get; set; } = string.Empty;
        public string? PlanoConvenio { get; set; } = string.Empty;
        public string? ProdutoConvenio { get; set; } = string.Empty;
    }
}
