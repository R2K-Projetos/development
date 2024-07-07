using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;
using Ghb.Psicossoma.SharedAbstractions.Services.Abstractions;

namespace Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

public class ResultDto<Dto> : BaseResult<Dto> where Dto : BaseDto
{
}
