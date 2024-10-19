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
        [ProducesResponseType(typeof(ResultDto<TelefoneDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<TelefoneDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<TelefoneDto> Get(int id)
        {
            ResultDto<TelefoneDto> result = new();

            try
            {
                result = _telefoneService.Get(id.ToString());

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
        /// Lista todos os telefones cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todos os telefones cadastrados",
        Description = "Lista todos os telefones cadastrados",
        OperationId = "Telefone.GetAll",
        Tags = new[] { "Telefone" })]
        [ProducesResponseType(typeof(ResultDto<TelefoneDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<TelefoneDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<TelefoneDto> GetAll()
        {
            ResultDto<TelefoneDto> result = new();

            try
            {
                result = _telefoneService.GetAll();

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
        [ProducesResponseType(typeof(ResultDto<TelefoneDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<TelefoneDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<TelefoneDto> GetAllTelefonePessoa(int PessoaId)
        {
            ResultDto<TelefoneDto> result = new();

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
    }
}
