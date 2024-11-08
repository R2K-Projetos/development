using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Abstractions.Base;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Services.Abstractions
{
    public interface IEnderecoService : IBaseService<EnderecoDto>
    {
        ResultDto<EnderecoDto> GetEnderecoPessoa(string PessoaId);
    }
}
