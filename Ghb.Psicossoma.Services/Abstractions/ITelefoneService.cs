﻿using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Abstractions.Base;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Services.Abstractions
{
    public interface ITelefoneService : IBaseService<TelefoneDto>
    {
        ResultDto<TelefoneResponseDto> GetTelefone(string Id);
        ResultDto<TelefoneResponseDto> GetAllTelefonePessoa(string pessoaId);
        ResultDto<TelefoneDto> Delete(string id);
    }
}
