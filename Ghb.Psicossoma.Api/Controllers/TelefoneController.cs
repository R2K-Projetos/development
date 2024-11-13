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
    public class TelefoneController : BaseApiController
    {
        private readonly ITelefoneService _telefoneService;
        private readonly ITipoTelefoneService _tipoTelefoneService;
        private readonly IConfiguration _configuration;

        public TelefoneController(ITelefoneService telefoneService,
                                  ITipoTelefoneService tipoTelefoneService,
                                  IConfiguration configuration)
        {
            _telefoneService = telefoneService;
            _tipoTelefoneService = tipoTelefoneService;
            _configuration = configuration;
        }

        /// <summary>
        /// Busca os dados de um determinado prontuário
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de um determinado telefone",
        Description = "Busca os dados de um determinado telefone",
        OperationId = "Telefone.Get",
        Tags = new[] { "Telefone" })]
        [ProducesResponseType(typeof(ResultDto<TelefoneResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<TelefoneResponseDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<TelefoneResponseDto> Get(int id)
        {
            ResultDto<TelefoneResponseDto> result = new();

            try
            {
                result = _telefoneService.GetTelefone(id.ToString());

                if (!result.HasError)
                    result.Message = "Telefone localizado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de prontuário", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Lista todos os tipos de telefones cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTypes")]
        [SwaggerOperation(
        Summary = "Lista todos os tipos de telefone cadastrados",
        Description = "Lista todos os tipos de telefone cadastrados",
        OperationId = "Telefone.GetTypes",
        Tags = new[] { "Telefone" })]
        [ProducesResponseType(typeof(ResultDto<TipoTelefoneDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<TipoTelefoneDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<TipoTelefoneDto> GetTypes()
        {
            ResultDto<TipoTelefoneDto> result = new();

            try
            {
                result = _tipoTelefoneService.GetAll();

                if (!result.HasError)
                    result.Message = "Tipos de elefone listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de tipos de telefone", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Lista dos telefones de uma pessoa
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllTelefonePessoa/{PessoaId}")]
        [SwaggerOperation(
        Summary = "Lista dos telefones de uma pessoa",
        Description = "Lista dos telefones de uma pessoa",
        OperationId = "Telefone.GetTelefonePessoa",
        Tags = new[] { "Telefone" })]
        [ProducesResponseType(typeof(ResultDto<TelefoneResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<TelefoneResponseDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<TelefoneResponseDto> GetAllTelefonePessoa(int PessoaId)
        {
            ResultDto<TelefoneResponseDto> result = new();

            try
            {
                result = _telefoneService.GetAllTelefonePessoa(PessoaId.ToString());

                if (!result.HasError)
                    result.Message = "Telefones listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de telefones", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Cria um novo telefone
        /// </summary>
        /// <param name="telefoneInfo">Json contendo os dados de um novo telefone></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [SwaggerOperation(
        Summary = "Cria um novo telefone",
        Description = "Cria um novo telefone",
        OperationId = "Telefone.Create",
        Tags = new[] { "Telefone" })]
        [ProducesResponseType(typeof(ResultDto<TelefoneDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<TelefoneDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<TelefoneDto> Create([FromBody] TelefoneDto telefoneInfo)
        {
            ResultDto<TelefoneDto> result = new();

            try
            {
                telefoneInfo.Id = 0;
                result = _telefoneService.Insert(telefoneInfo);

                if (!result.HasError)
                    result.Message = "Telefone criado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na criação do telefone", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Atualiza os dados de um Telefone
        /// </summary>
        /// <param name="telefoneInfo">Json contendo os dados do telefone></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [SwaggerOperation(
        Summary = "Atualiza os dados de um telefone",
        Description = "Atualiza os dados de um telefone",
        OperationId = "Telefone.Update",
        Tags = new[] { "Telefone" })]
        [ProducesResponseType(typeof(ResultDto<TelefoneDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<TelefoneDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<TelefoneDto> Update([FromBody] TelefoneDto telefoneInfo)
        {
            ResultDto<TelefoneDto> result = new();

            try
            {
                result = _telefoneService.Update(telefoneInfo);

                if (!result.HasError)
                    result.Message = "Telefone alterado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na alteração de telefone", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Atualiza os dados de um Telefone
        /// </summary>
        /// <param name="obj">Json contendo os dados do telefone></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        [SwaggerOperation(
        Summary = "Deleta um telefone",
        Description = "Deleta um telefone",
        OperationId = "Telefone.Delete",
        Tags = new[] { "Telefone" })]
        [ProducesResponseType(typeof(ResultDto<TelefoneDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<TelefoneDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<TelefoneDto> Delete([FromBody] TelefoneDto obj)
        {
            ResultDto<TelefoneDto> result = new();

            try
            {
                result = _telefoneService.Delete(obj.Id.ToString());

                if (!result.HasError)
                    result.Message = "Telefone exclusão com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na exclusão do telefone", ex);
            }

            return Ok(result);
        }
    }
}
