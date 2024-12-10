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
        CreateMap<GuiaAutorizacao, GuiaAutorizacaoDto>().ReverseMap();
        CreateMap<LaudoAnamnese, LaudoAnamneseDto>().ReverseMap();
        CreateMap<ModeloLaudoAnamnese, ModeloLaudoAnamneseDto>().ReverseMap();
        CreateMap<Paciente, PacienteDto>().ReverseMap();
        CreateMap<PerfilUsuario, PerfilUsuarioDto>().ReverseMap();
        CreateMap<Pessoa, PessoaDto>().ReverseMap();
        CreateMap<PlanoSaude, PlanoSaudeDto>().ReverseMap();
        CreateMap<ProcedimentoDetalhe, ProcedimentoDetalheDto>().ReverseMap();
        CreateMap<Procedimento, ProcedimentoDto>().ReverseMap();        
        CreateMap<Profissional, ProfissionalDto>().ReverseMap();
        CreateMap<ProfissionalEspecialidade, ProfissionalEspecialidadeDto>().ReverseMap();
        CreateMap<Prontuario, ProntuarioDto>().ReverseMap();
        CreateMap<ProntuarioHistorico, ProntuarioHistoricoDto>().ReverseMap();        
        CreateMap<RegistroProfissional, RegistroProfissionalDto>().ReverseMap();
        CreateMap<RelacionamentoPaciente, RelacionamentoPacienteDto>().ReverseMap();
        CreateMap<RenovacaoEncaminhamento, RenovacaoEncaminhamentoDto>().ReverseMap();
        CreateMap<StatusUsuario, StatusUsuarioDto>().ReverseMap();
        CreateMap<Telefone, TelefoneDto>().ReverseMap();
        CreateMap<TipoAcomodacao, TipoAcomodacaoDto>().ReverseMap();
        CreateMap<TipoArquivo, TipoArquivoDto>().ReverseMap();
        CreateMap<TipoTelefone, TipoTelefoneDto>().ReverseMap();
        CreateMap<Uf, UfDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();

        //Retornos específicos     
        CreateMap<PacienteResponse, PacienteResponseDto>().ReverseMap();        
        CreateMap<ProfissionalResponse, ProfissionalResponseDto>().ReverseMap();
        CreateMap<TelefoneResponse, TelefoneResponseDto>().ReverseMap();
        CreateMap<UserResponse, UserResponseDto>().ReverseMap();
    }
}
