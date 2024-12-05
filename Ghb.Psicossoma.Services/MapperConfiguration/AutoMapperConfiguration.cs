using AutoMapper;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Services.Dtos;

namespace Ghb.Psicossoma.Services.MapperConfiguration;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        //Retornos espelhados (idênticos às entidades) 
        CreateMap<Cidade, CidadeDto>().ReverseMap();
        CreateMap<Cid, CidDto>().ReverseMap();
        CreateMap<Convenio, ConvenioDto>().ReverseMap();
        CreateMap<Encaminhamento, EncaminhamentoDto>().ReverseMap();
        CreateMap<Endereco, EnderecoDto>().ReverseMap();
        CreateMap<Especialidade, EspecialidadeDto>().ReverseMap();
        CreateMap<Funcionalidade, FuncionalidadeDto>().ReverseMap();
        CreateMap<GrauParentesco, GrauParentescoDto>().ReverseMap();
        CreateMap<GrupoGuia, GrupoGuiaDto>().ReverseMap();
        CreateMap<Paciente, PacienteDto>().ReverseMap();
        CreateMap<PerfilUsuario, PerfilUsuarioDto>().ReverseMap();
        CreateMap<Pessoa, PessoaDto>().ReverseMap();
        CreateMap<PlanoConvenio, PlanoConvenioDto>().ReverseMap();
        CreateMap<PlanoSaude, PlanoSaudeDto>().ReverseMap();
        CreateMap<ProdutoConvenio, ProdutoConvenioDto>().ReverseMap();
        CreateMap<Profissional, ProfissionalDto>().ReverseMap();
        CreateMap<ProfissionalEspecialidade, ProfissionalEspecialidadeDto>().ReverseMap();
        CreateMap<ProntuarioHistorico, ProntuarioHistoricoDto>().ReverseMap();
        CreateMap<Prontuario, ProntuarioDto>().ReverseMap();
        CreateMap<RegistroProfissional, RegistroProfissionalDto>().ReverseMap();
        CreateMap<StatusUsuario, StatusUsuarioDto>().ReverseMap();
        CreateMap<Telefone, TelefoneDto>().ReverseMap();
        CreateMap<TipoArquivo, TipoArquivoDto>().ReverseMap();
        CreateMap<TipoTelefone, TipoTelefoneDto>().ReverseMap();
        CreateMap<Uf, UfDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();

        //Retornos específicos
        CreateMap<ConvenioResponse, ConvenioResponseDto>().ReverseMap();        
        CreateMap<PacienteResponse, PacienteResponseDto>().ReverseMap();        
        CreateMap<ProfissionalResponse, ProfissionalResponseDto>().ReverseMap();
        CreateMap<TelefoneResponse, TelefoneResponseDto>().ReverseMap();
        CreateMap<UserResponse, UserResponseDto>().ReverseMap();
    }
}
