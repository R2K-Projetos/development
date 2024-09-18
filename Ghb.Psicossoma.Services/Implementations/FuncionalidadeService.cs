using AutoMapper;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Library.Extensions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Data;
using System.Diagnostics;

namespace Ghb.Psicossoma.Services.Implementations
{
    public class FuncionalidadeService : BaseService<FuncionalidadeDto, Funcionalidade>, IFuncionalidadeService
    {
        private readonly IFuncionalidadeRepository _funcionalidadeRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<FuncionalidadeService> _logger;

        public FuncionalidadeService(IFuncionalidadeRepository funcionalidadeRepository,
                             ILogger<FuncionalidadeService> logger,
                             IMapper mapper,
                             IConfiguration configuration) : base(funcionalidadeRepository, mapper)
        {
            _funcionalidadeRepository = funcionalidadeRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<FuncionalidadeDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<FuncionalidadeDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT Id, Nome FROM funcionalidades;";

                DataTable result = _funcionalidadeRepository.GetAll(selectQuery);
                List<Funcionalidade> list = result.CreateListFromTable<Funcionalidade>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Funcionalidade>, IEnumerable<FuncionalidadeDto>>(list ?? Enumerable.Empty<Funcionalidade>());
                    returnValue.WasExecuted = true;
                    returnValue.ResponseCode = 200;
                }
                else
                {
                    returnValue.BindError(404, "Não foram encontrados dados para exibição");
                }
            }
            catch (Exception ex)
            {
                returnValue.BindError(500, ex.GetErrorMessage());
                _logger.LogError(ex, "Erro na recuperação dos dados");
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }

        public override ResultDto<FuncionalidadeDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<FuncionalidadeDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, Nome
                                 FROM funcionalidade
                                 WHERE id = {id};";

                DataTable result = _funcionalidadeRepository.Get(selectQuery);
                List<Funcionalidade> item = result.CreateListFromTable<Funcionalidade>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Funcionalidade>, IEnumerable<FuncionalidadeDto>>(item ?? Enumerable.Empty<Funcionalidade>());
                    returnValue.WasExecuted = true;
                    returnValue.ResponseCode = 200;
                }
                else
                {
                    returnValue.BindError(404, "Não foram encontrados dados para exibição");
                }
            }
            catch (Exception ex)
            {
                returnValue.BindError(500, ex.GetErrorMessage());
                LogContext.PushProperty("Query", selectQuery);
                _logger.LogError(ex, "Erro na recuperação dos dados");
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }

        public override ResultDto<FuncionalidadeDto> Insert(FuncionalidadeDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<FuncionalidadeDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<FuncionalidadeDto, Funcionalidade>(dto);
                insertQuery = $@"INSERT INTO funcionalidade 
                                 (Nome)
                                 VALUES 
                                 ('{entidade.Nome}');";

                long newId = _funcionalidadeRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<Funcionalidade, FuncionalidadeDto>(entidade);

                returnValue.Items = returnValue.Items.Concat(new[] { item });
                returnValue.WasExecuted = true;
                returnValue.ResponseCode = 200;
            }
            catch (Exception ex)
            {
                returnValue.BindError(500, ex.GetErrorMessage());
                LogContext.PushProperty("Query", insertQuery);
                _logger.LogError(ex, "Erro na gravação dos dados");
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }

        public override ResultDto<FuncionalidadeDto> Update(FuncionalidadeDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<FuncionalidadeDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<FuncionalidadeDto, Funcionalidade>(dto);
                updateQuery = $@"UPDATE funcionalidade 
                                 SET Nome = '{entidade.Nome}'
                                 WHERE id = {entidade.Id};";

                _funcionalidadeRepository.Update(updateQuery);
                var item = _mapper.Map<Funcionalidade, FuncionalidadeDto>(entidade);

                returnValue.Items = returnValue.Items.Concat(new[] { item });
                returnValue.WasExecuted = true;
                returnValue.ResponseCode = 200;
            }
            catch (Exception ex)
            {
                returnValue.BindError(500, ex.GetErrorMessage());
                LogContext.PushProperty("Query", updateQuery);
                _logger.LogError(ex, "Erro na gravação dos dados");
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }
    }
}
