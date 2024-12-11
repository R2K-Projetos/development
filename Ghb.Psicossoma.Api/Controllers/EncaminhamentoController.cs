using Ghb.Psicossoma.Api.Controllers.Base;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ghb.Psicossoma.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EncaminhamentoController : BaseApiController
    {
        private readonly IEncaminhamentoService _encaminhamentoService;
        private readonly IConfiguration _configuration;

        public EncaminhamentoController(IEncaminhamentoService encaminhamentoService,
                                        IConfiguration configuration)
        {
            _encaminhamentoService = encaminhamentoService;
            _configuration = configuration;
        }

        /// <summary>
        /// Busca os dados de um determinado encaminhamento
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de um determinado encaminhamento",
        Description = "Busca os dados de um determinado encaminhamento",
        OperationId = "Encaminhamento.Get",
        Tags = new[] { "Encaminhamento" })]
        [ProducesResponseType(typeof(ResultDto<EncaminhamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<EncaminhamentoDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<EncaminhamentoDto> Get(int id)
        {
            ResultDto<EncaminhamentoDto> result = new();

            try
            {
                result = _encaminhamentoService.Get(id.ToString());

                if (!result.HasError)
                    result.Message = "Encaminhamento localizado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de encaminhamento", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Lista todos os encaminhamentos cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todos os encaminhamentos cadastrados",
        Description = "Lista todos os encaminhamentos cadastrados",
        OperationId = "Encaminhamento.GetAll",
        Tags = new[] { "Encaminhamento" })]
        [ProducesResponseType(typeof(ResultDto<EncaminhamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<EncaminhamentoDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<EncaminhamentoDto> GetAll()
        {
            ResultDto<EncaminhamentoDto> result = new();

            try
            {
                result = _encaminhamentoService.GetAll();

                if (!result.HasError)
                    result.Message = "Encaminhamentos listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de encaminhamentos", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Busca os dados de encaminhamentos, por paciente
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByIdPaciente")]
        [SwaggerOperation(
        Summary = "Busca os dados de encaminhamentos, por paciente",
        Description = "Busca os dados encaminhamentos, por paciente",
        OperationId = "Encaminhamento.GetByIdPaciente",
        Tags = new[] { "Encaminhamento" })]
        [ProducesResponseType(typeof(ResultDto<EncaminhamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<EncaminhamentoDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<EncaminhamentoDto> GetByIdPaciente(int id)
        {
            ResultDto<EncaminhamentoDto> result = new();

            try
            {
                result = _encaminhamentoService.GetByIdPaciente(id);

                if (!result.HasError)
                    result.Message = "Encaminhamentos localizado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de encaminhamentos", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Cria um novo encaminhamento
        /// </summary>
        /// <param name="encaminhamentoInfo">Json contendo os dados do novo encaminhamento></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [SwaggerOperation(
        Summary = "Cria um novo encaminhamento",
        Description = "Cria um novo encaminhamento",
        OperationId = "Encaminhamento.Create",
        Tags = new[] { "Encaminhamento" })]
        [ProducesResponseType(typeof(ResultDto<EncaminhamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<EncaminhamentoDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<EncaminhamentoDto> Create([FromBody] EncaminhamentoDto encaminhamentoInfo)
        {
            ResultDto<EncaminhamentoDto> result = new();

            try
            {
                encaminhamentoInfo.Id = 0;
                result = _encaminhamentoService.Insert(encaminhamentoInfo);

                if (!result.HasError)
                    result.Message = "Encaminhamento criado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na criação de encaminhamento", ex);
            }

            return Ok(result);
        }
    }
}

