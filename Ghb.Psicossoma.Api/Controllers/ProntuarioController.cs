using Microsoft.AspNetCore.Mvc;
using Ghb.Psicossoma.Services.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Ghb.Psicossoma.Api.Controllers.Base;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProntuarioController : BaseApiController
    {
        private readonly IProntuarioService _prontuarioService;
        private readonly IConfiguration _configuration;

        public ProntuarioController(IProntuarioService prontuarioService,
                                    IConfiguration configuration)
        {
            _prontuarioService = prontuarioService;
            _configuration = configuration;
        }

        /// <summary>
        /// Busca os dados de um determinado prontuário
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de um determinado prontuário",
        Description = "Busca os dados de um determinado prontuário",
        OperationId = "Prontuario.Get",
        Tags = new[] { "Prontuario" })]
        [ProducesResponseType(typeof(ResultDto<ProntuarioDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<ProntuarioDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<ProntuarioDto> Get(int id)
        {
            ResultDto<ProntuarioDto> result = new();

            try
            {
                result = _prontuarioService.Get(id.ToString());

                if (!result.HasError)
                    result.Message = "Prontuário localizado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de prontuário", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Lista todos os prontuários cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todos os prontuários cadastrados",
        Description = "Lista todos os prontuários cadastrados",
        OperationId = "Prontuario.GetAll",
        Tags = new[] { "Prontuario" })]
        [ProducesResponseType(typeof(ResultDto<ProntuarioDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<ProntuarioDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<ProntuarioDto> GetAll()
        {
            ResultDto<ProntuarioDto> result = new();

            try
            {
                result = _prontuarioService.GetAll();

                if (!result.HasError)
                    result.Message = "Prontuários listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de prontuários", ex);
            }

            return Ok(result);
        }
    }
}
