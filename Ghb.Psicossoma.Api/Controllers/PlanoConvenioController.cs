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
    public class PlanoConvenioController : BaseApiController
    {
        private readonly IPlanoConvenioService _planoConvenioService;
        private readonly IConfiguration _configuration;

        public PlanoConvenioController(IPlanoConvenioService planoConvenioService,
                                       IConfiguration configuration)
        {
            _planoConvenioService = planoConvenioService;
            _configuration = configuration;
        }

        /// <summary>
        /// Lista todos os planos conveniados cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todos os planos conveniados cadastrados",
        Description = "Lista todos os planos conveniados cadastrados",
        OperationId = "PlanoConvenio.GetAll",
        Tags = new[] { "PlanoConvenio" })]
        [ProducesResponseType(typeof(ResultDto<PlanoConvenioDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<PlanoConvenioDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<PlanoConvenioDto> GetAll()
        {
            ResultDto<PlanoConvenioDto> result = new();

            try
            {
                result = _planoConvenioService.GetAll();

                if (!result.HasError)
                    result.Message = "Planos conveniados listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de planos conveniados", ex);
            }

            return Ok(result);
        }

    }
}
