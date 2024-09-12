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
    public class ConvenioController : BaseApiController
    {
        private readonly IConvenioService _convenioService;
        private readonly IConfiguration _configuration;

        public ConvenioController(IConvenioService convenioService, IConfiguration configuration)
        {
            _convenioService = convenioService;
            _configuration = configuration;
        }

        /// <summary>
        /// Busca os dados de um determinado convênio
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [SwaggerOperation(
        Summary = "Busca os dados de um determinado convênio",
        Description = "Busca os dados de um determinado convênio",
        OperationId = "Convenio.Get",
        Tags = new[] { "Convenio" })]
        [ProducesResponseType(typeof(ResultDto<ConvenioResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<ConvenioResponseDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<ConvenioResponseDto> Get(int id)
        {
            ResultDto<ConvenioResponseDto> result = new();

            try
            {
                result = _convenioService.Get(id.ToString());

                if (!result.HasError)
                    result.Message = "Convênio localizado com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na localização de convênio", ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// Lista todos os convênios cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [SwaggerOperation(
        Summary = "Lista todos os convênios cadastrados",
        Description = "Lista todos os convênios cadastrados",
        OperationId = "Convenio.GetAll",
        Tags = new[] { "Convenio" })]
        [ProducesResponseType(typeof(ResultDto<ConvenioResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<ConvenioResponseDto>), StatusCodes.Status500InternalServerError)]
        public ActionResult<ConvenioResponseDto> GetAll()
        {
            ResultDto<ConvenioResponseDto> result = new();

            try
            {
                result = _convenioService.GetAll();

                if (!result.HasError)
                    result.Message = "Convênios listados com sucesso!";
            }
            catch (Exception ex)
            {
                result.BindError(500, "Erro na listagem de convênios", ex);
            }

            return Ok(result);
        }

    }
}
