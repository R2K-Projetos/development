using AutoMapper;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.Domains.Entities;

namespace Ghb.Psicossoma.Services.MapperConfiguration;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        //Retornos espelhados (idênticos às entidades) 
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Pessoa, PessoaDto>().ReverseMap();
        CreateMap<Status, StatusDto>().ReverseMap();
        CreateMap<Profissional, ProfissionalDto>().ReverseMap();
        CreateMap<Especialidade, EspecialidadeDto>().ReverseMap();
        CreateMap<PerfilUsuario, PerfilUsuarioDto>().ReverseMap();
        CreateMap<RegistroProfissional, RegistroProfissionalDto>().ReverseMap();
        CreateMap<Endereco, EnderecoDto>().ReverseMap();

        //Retornos específicos
        CreateMap<UserResponse, UserResponseDto>().ReverseMap();
        CreateMap<ProfissionalResponse, ProfissionalResponseDto>().ReverseMap();
    }
}
