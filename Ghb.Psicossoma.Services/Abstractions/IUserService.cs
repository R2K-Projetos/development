using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Abstractions.Base;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Services.Abstractions
{
    public interface IUserService : IBaseService<UserDto>
    {
        ResultDto<AuthenticationDto> Login(string email, string password);

        new ResultDto<UserResponseDto> Get(string id);

        new ResultDto<UserResponseDto> GetAll();

        string HashPasswordToString(string password);

        byte[] HashPassword(string password);

    }
}
