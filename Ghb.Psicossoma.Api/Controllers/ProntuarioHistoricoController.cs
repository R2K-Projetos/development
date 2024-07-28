using Microsoft.AspNetCore.Mvc;
using Ghb.Psicossoma.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Ghb.Psicossoma.Api.Controllers.Base;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProntuarioHistoricoController : BaseApiController
    {
        private readonly IProntuarioHistoricoService _prontuarioHistoricoService;
        private readonly IConfiguration _configuration;

        public ProntuarioHistoricoController(IProntuarioHistoricoService prontuarioService,
                                             IConfiguration configuration)
        {
            _prontuarioHistoricoService = prontuarioService;
            _configuration = configuration;
        }


        /// <summary>
        /// Busca os dados de um determinado histórico de prontuário
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de um determinado histórico de prontuário",
        Description = "Busca os dados de um determinado histórico de prontuário",
        OperationId = "ProntuarioHistorico.Get",
        Tags = new[] { "ProntuarioHistorico" })]
        [ProducesResponseType(typeof(ResultDto<ProntuarioHistoricoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<ProntuarioHistoricoDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<ProntuarioHistoricoDto> Get(int id)
        {
            ResultDto<ProntuarioHistoricoDto> result = new();

            try
            {
                result = _prontuarioHistoricoService.Get(id.ToString());

                if (!result.HasError)
                    result.Message = "Histórico de prontuário localizado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de histórico de prontuário", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Lista todos os históricos de prontuários cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todos os históricos de prontuários cadastrados",
        Description = "Lista todos os históricos de prontuários cadastrados",
        OperationId = "ProntuarioHistorico.GetAll",
        Tags = new[] { "ProntuarioHistorico" })]
        [ProducesResponseType(typeof(ResultDto<ProntuarioHistoricoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<ProntuarioHistoricoDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<ProntuarioHistoricoDto> GetAll()
        {
            ResultDto<ProntuarioHistoricoDto> result = new();

            try
            {
                result = _prontuarioHistoricoService.GetAll();

                if (!result.HasError)
                    result.Message = "Históricos de prontuários listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de históricos de prontuários", ex);
            }

            return Ok(result);
        }
    }
}
